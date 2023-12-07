﻿using System;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Homework4
{
    public sealed class UserPopup : MonoBehaviour
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _descriptionText;
        [SerializeField] private Image _iconImage;
        [Space]
        [SerializeField] private LevelSliderView _levelSliderView;
        [SerializeField] private Text _levelText;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private Button _closeButton;

        private IPresenter _presenter;
        private StatPool _statPool;
        private readonly Dictionary<CharacterStat, CharacterStatView> _characterStats = new();
        private readonly Dictionary<CharacterStat, IDisposable> _disposableStats = new();
        private readonly CompositeDisposable _disposable = new ();
        private readonly float _duration = 0.3f;
        private readonly Vector3 _startScale = Vector3.zero;
        private readonly Vector3 _endScale = Vector3.one;

        [Inject]
        public void Construct(StatPool statPool)
        {
            _statPool = statPool;
            gameObject.SetActive(false);
        }
        public void Show(object args)
        {
            if(gameObject.activeSelf)
                return;
            
            if (args is not IPresenter presenter)
                throw new Exception("Expected IProductPresenter");
            
            _presenter = presenter;
            
            _presenter.Name.Subscribe(NameUpdate).AddTo(_disposable);
            _presenter.Description.Subscribe(DescriptionUpdate).AddTo(_disposable);
            _presenter.Icon.Subscribe(IconUpdate).AddTo(_disposable);

            _presenter.CurrentLevel.Subscribe(LevelUpdate).AddTo(_disposable);
            RequireExperienceUpdate(_presenter.RequiredExperience);
            _presenter.CurrentExperience.Subscribe(CurrentExperienceUpdate).AddTo(_disposable);
            _levelUpButton.OnClickAsObservable().Subscribe(_ => OnLevelUpClick()).AddTo(_disposable);

            _presenter.Stats.ObserveAdd().Subscribe(action => AddState(action.Value)).AddTo(_disposable);
            _presenter.Stats.ObserveRemove().Subscribe(action => RemoveState(action.Value)).AddTo(_disposable);

            foreach (var stat in _presenter.Stats)
            {
                AddState(stat);
            }

            _closeButton.OnClickAsObservable().Subscribe(_ => Hide()).AddTo(_disposable);
            
            transform.DOScale(_endScale, _duration).ChangeStartValue(_startScale);
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            _statPool.PutAll();
            transform.DOScale(_startScale, _duration).OnComplete(() =>
            {
                gameObject.SetActive(false);
                transform.localScale = _endScale;
            });
            
            foreach (var disposable in _disposableStats)
            {
                disposable.Value.Dispose();
            }

            _disposable.Clear();
            _disposableStats.Clear();
            _characterStats.Clear();
            _presenter.Hide();
        }
        private void NameUpdate(string userName)
        {
            TweenText(_nameText, userName);
        }
        private void DescriptionUpdate(string description)
        {
            TweenText(_descriptionText, description);
        }
        private void IconUpdate(Sprite icon)
        {
            _iconImage.sprite = icon;
            TweenScale(_iconImage.transform, () => _iconImage.sprite = icon);
        }
        private void LevelUpdate(string level)
        {
            TweenScale(_levelText.transform, () => _levelText.text = level);
            RequireExperienceUpdate(_presenter.RequiredExperience);
        }
        private void RequireExperienceUpdate(int experience)
        {
            _levelSliderView.RequireExperienceUpdate(experience);
        }
        private void CurrentExperienceUpdate(int experience)
        {
            _levelSliderView.CurrentExperienceUpdate(experience);
            LevelButtonUpdate();
        }
        private void AddState(CharacterStat stat)
        {
            var statView = _statPool.Get();
            statView.Init(stat.Name, stat.Value.ToString());
            var disposable = stat.Value.SkipLatestValueOnSubscribe().
                Subscribe(i => statView.UpdateValue(i.ToString()));
            _disposableStats.Add(stat, disposable);
            _characterStats.Add(stat, statView);
        }
        private void RemoveState(CharacterStat stat)
        {
            _statPool.Put(_characterStats[stat]);
            _disposableStats[stat].Dispose();
            _characterStats.Remove(stat);
        }
        private void LevelButtonUpdate()
        {
            _levelUpButton.interactable = _presenter.CanLevelUp;
        }
        private void OnLevelUpClick()
        {
            if (!_presenter.CanLevelUp)
                return;

            _presenter.LevelUp();
        }
        private void TweenText(Text text, string endValue)
        {
            text.text = "";
            text.DOText(endValue, _duration);
        }
        private void TweenScale(Transform goTransform, Action onComplete)
        {
            goTransform.DOScale(0f, _duration * 0.5f).SetLoops(2, LoopType.Yoyo).
                OnStepComplete(() => onComplete?.Invoke());
        }
    }
}