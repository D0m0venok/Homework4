using UniRx;
using UnityEngine;

namespace Homework4
{
    public sealed class CharacterInfoPresenter : ICharacterInfoPresenter
    {
        private readonly UserInfo _userInfo;
        private readonly PlayerLevel _playerLevel;
        private readonly StringReactiveProperty _name = new();
        private readonly StringReactiveProperty _description = new();
        private readonly ReactiveProperty<Sprite> _icon = new();
        private readonly StringReactiveProperty _currentLevel = new();
        private readonly IntReactiveProperty _currentExperience = new();

        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;
        public IReadOnlyReactiveProperty<string> CurrentLevel => _currentLevel;
        public IReadOnlyReactiveProperty<int> CurrentExperience => _currentExperience;
        public int RequiredExperience => _playerLevel.RequiredExperience;
        public bool CanLevelUp => _playerLevel.CanLevelUp();
        
        public CharacterInfoPresenter(UserInfo userInfo, PlayerLevel playerLevel)
        {
            _userInfo = userInfo;
            _playerLevel = playerLevel;
            
            _userInfo.OnNameChanged += OnChangeName;
            _name.Value = _userInfo.Name;
            
            _userInfo.OnDescriptionChanged += OnChangeDescription;
            _description.Value = _userInfo.Description;
            
            _userInfo.OnIconChanged += OnChangeIcon;
            _icon.Value = _userInfo.Icon;
            
            _playerLevel.OnLevelUp += OnLevelUp;
            _currentLevel.Value = _playerLevel.CurrentLevel.ToString();
            
            _playerLevel.OnExperienceChanged += OnExperienceChanged;
            _currentExperience.Value = _playerLevel.CurrentExperience;
        }
        
        public void Dispose()
        {
            _userInfo.OnNameChanged -= OnChangeName;
            _userInfo.OnDescriptionChanged -= OnChangeDescription;
            _userInfo.OnIconChanged -= OnChangeIcon;
            _playerLevel.OnLevelUp -= OnLevelUp;
            _playerLevel.OnExperienceChanged -= OnExperienceChanged;
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
        private void OnLevelUp()
        {
            _currentLevel.Value = _playerLevel.CurrentLevel.ToString();
            OnExperienceChanged(_playerLevel.CurrentExperience);
        }
        private void OnExperienceChanged(int experience)
        {
            _currentExperience.Value = experience;
        }
    }
}