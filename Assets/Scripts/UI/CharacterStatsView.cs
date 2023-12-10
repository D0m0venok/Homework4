using System;
using System.Collections.Generic;
using Extensions;
using UniRx;
using UnityEngine;

namespace Homework4
{
    [Serializable]
    public sealed class CharacterStatsView
    {
        [SerializeField] private CharacterStatView _characterStatView;
        [SerializeField] private Transform _activeContainer;
        [SerializeField] private Transform _disableContainer;

        private StatViewPool _statPool;
        private readonly CompositeDisposable _disposable = new ();
        private readonly Dictionary<ICharacterStatPM, CharacterStatView> _characterStats = new();
        
        public void Init()
        {
            _statPool = new StatViewPool(_characterStatView, _activeContainer, _disableContainer);
        }
        public void Set(ICharacterStatsPresenter presenter)
        {
            presenter.Stats.ObserveAdd().Subscribe(action => AddState(action.Value)).AddTo(_disposable); 
            presenter.Stats.ObserveRemove().Subscribe(action => RemoveState(action.Value)).AddTo(_disposable);
            foreach (var stat in presenter.Stats)
            {
                AddState(stat);
            }
        }
        public void Dispose()
        {
            _statPool.PutAll();

            foreach (var statView in _characterStats.Values)
            {
                statView.Dispose();
            }
            
            _characterStats.Clear();
            _disposable.Clear();
        }
        
        private void AddState(ICharacterStatPM stat)
        {
            var statView = _statPool.Get();
            statView.Init(stat);
            _characterStats.Add(stat, statView);
            TL.Play(TLNames.ScaleOne, statView.gameObject);
        }
        private void RemoveState(ICharacterStatPM stat)
        {
            var view = _characterStats[stat];
            view.Dispose();
            _characterStats.Remove(stat);
            TL.Play(TLNames.ScaleZero, view.gameObject).OnComplete(() => _statPool.Put(view));
        }
    }
}