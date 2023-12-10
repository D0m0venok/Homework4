using System;
using DG.Tweening;
using Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Homework4
{
    public sealed class UserPopup : MonoBehaviour, IInitializable
    {
        [SerializeField] private CharacterInfoView _characterInfoView;
        [SerializeField] private CharacterStatsView _characterStatsView;
        [SerializeField] private Button _closeButton;

        private IUserPresenter _presenter;
        private IDisposable _disposable;
        private readonly float _duration = 0.3f;
        private readonly Vector3 _startScale = Vector3.zero;
        private readonly Vector3 _endScale = Vector3.one;

        public void Initialize()
        {
            gameObject.SetActive(false);
            _characterStatsView.Init();
        }
        public void Show(object args)
        {
            if(gameObject.activeSelf)
                return;
            
            if (args is not IUserPresenter presenter)
                throw new Exception("Expected IProductPresenter");

            _presenter = presenter;
            _characterInfoView.Set(presenter.InfoPresenter);
            _characterStatsView.Set(presenter.StatsPresenter);
            
            _disposable = _closeButton.OnClickAsObservable().Subscribe(_ => Hide());
            TL.Play(TLNames.ScaleOne, gameObject);
            //transform.DOScale(_endScale, _duration).ChangeStartValue(_startScale);
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            TL.Play(TLNames.ScaleZero, gameObject).OnComplete(() =>
            {
                gameObject.SetActive(false);
                transform.localScale = _endScale;
            });
            
            _characterInfoView.Dispose();
            _characterStatsView.Dispose();
            _presenter.Dispose();
            _disposable.Dispose();
        }
    }
}