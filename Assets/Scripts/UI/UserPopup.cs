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
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            TL.Play(TLNames.ScaleZero, gameObject).OnComplete(() => gameObject.SetActive(false));
            
            _characterInfoView.Dispose();
            _characterStatsView.Dispose();
            _presenter.Dispose();
            _disposable.Dispose();
        }
    }
}