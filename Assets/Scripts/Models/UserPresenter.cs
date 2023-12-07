using UniRx;
using Unity.VisualScripting;
using UnityEngine;

namespace Homework4
{
    public sealed class UserPresenter : IPresenter
    {
        private readonly PlayerLevel _playerLevel;
        private readonly CompositeDisposable _disposable = new ();
        private readonly StringReactiveProperty _name = new();
        private readonly StringReactiveProperty _description = new();
        private readonly ReactiveProperty<Sprite> _icon = new();
        private readonly StringReactiveProperty _currentLevel = new();
        private readonly IntReactiveProperty _currentExperience = new();
         private readonly ReactiveCollection<CharacterStat> _stats = new();

        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;
        public IReadOnlyReactiveProperty<string> CurrentLevel => _currentLevel;
        public IReadOnlyReactiveProperty<int> CurrentExperience => _currentExperience;
        public int RequiredExperience => _playerLevel.RequiredExperience;
        public IReadOnlyReactiveCollection<CharacterStat> Stats => _stats;
        public bool CanLevelUp => _playerLevel.CanLevelUp();
        
        public UserPresenter(UserInfo userInfo, PlayerLevel playerLevel, CharacterInfo characterInfo)
        {
            _playerLevel = playerLevel;

            userInfo.Name.Subscribe(OnChangeName).AddTo(_disposable);
            userInfo.Description.Subscribe(OnChangeDescription).AddTo(_disposable);
            userInfo.Icon.Subscribe(OnChangeIcon).AddTo(_disposable);
            
            _playerLevel.CurrentLevel.Subscribe(OnLevelUp).AddTo(_disposable);
            _playerLevel.CurrentExperience.Subscribe(OnExperienceChanged).AddTo(_disposable);
            
            _stats.AddRange(characterInfo.Stats);
            characterInfo.Stats.ObserveAdd().Subscribe(action => OnCharacterStatAdded(action.Value)).AddTo(_disposable);
            characterInfo.Stats.ObserveRemove().Subscribe(action => OnCharacterStatRemoved(action.Value)).AddTo(_disposable);
        }
        
        public void Hide()
        {
            _disposable.Dispose();
        }
        public void LevelUp()
        {
            _playerLevel.LevelUp();
        }
        
        private void OnChangeName(string userName)
        {
            _name.Value = userName;
        }
        private void OnChangeDescription(string description)
        {
            _description.Value = description;
        }
        private void OnChangeIcon(Sprite sprite)
        {
            _icon.Value = sprite;
        }
        private void OnLevelUp(int level)
        {
            _currentLevel.Value = level.ToString();
        }
        private void OnExperienceChanged(int experience)
        {
            _currentExperience.Value = experience;
        }
        private void OnCharacterStatAdded(CharacterStat stat)
        {
            _stats.Add(stat);
        }
        private void OnCharacterStatRemoved(CharacterStat stat)
        {
            _stats.Remove(stat);
        }
    }
}