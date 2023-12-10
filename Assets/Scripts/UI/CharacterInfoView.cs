using System;
using DG.Tweening;
using Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Homework4
{
    [Serializable]
    public sealed class CharacterInfoView
    {
        [SerializeField] private Text _nameText;
        [SerializeField] private Text _descriptionText;
        [SerializeField] private Image _iconImage;
        [Space]
        [SerializeField] private LevelSliderView _levelSliderView;
        [SerializeField] private Text _levelText;
        [SerializeField] private Button _levelUpButton;
        
        private ICharacterInfoPresenter _presenter;
        private readonly CompositeDisposable _disposable = new ();
        
        public void Show(ICharacterInfoPresenter presenter)
        {
            _presenter = presenter;
            
            _presenter.Name.Subscribe(NameUpdate).AddTo(_disposable);
            _presenter.Description.Subscribe(DescriptionUpdate).AddTo(_disposable);
            _presenter.Icon.Subscribe(IconUpdate).AddTo(_disposable);

            _presenter.CurrentLevel.Subscribe(LevelUpdate).AddTo(_disposable);
            RequireExperienceUpdate(_presenter.RequiredExperience);
            _presenter.CurrentExperience.Subscribe(CurrentExperienceUpdate).AddTo(_disposable);
            
            _levelUpButton.OnClickAsObservable().Subscribe(_ => OnLevelUpClick()).AddTo(_disposable);
        }
        public void Dispose()
        {
            _disposable.Clear();
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
            var tween = TL.Get(TLNames.Text);
            text.DOText(endValue, tween.GetTime()).SetEase(tween.Ease);
        }
        private void TweenScale(Transform goTransform, Action onComplete)
        {
            var tween = TL.Get(TLNames.ScaleZero);
            goTransform.DOScale((Vector3)tween.To, tween.GetTime() * 0.5f).SetLoops(2, LoopType.Yoyo)
                .SetEase(tween.Ease).OnStepComplete(() => onComplete?.Invoke());
        }
    }
}