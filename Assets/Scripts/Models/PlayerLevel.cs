using System;
using UniRx;

namespace Homework4
{
    public sealed class PlayerLevel
    {
        private readonly ReactiveProperty<int> _currentLevel = new(1);
        private readonly ReactiveProperty<int> _currentExperience = new();
        
        public IReadOnlyReactiveProperty<int> CurrentLevel => _currentLevel;
        public IReadOnlyReactiveProperty<int> CurrentExperience => _currentExperience;
        public int RequiredExperience => 100 * (CurrentLevel.Value + 1);

        public void AddExperience(int range)
        {
            var xp = Math.Min(CurrentExperience.Value + range, RequiredExperience);
            _currentExperience.Value = xp;
        }
        public void LevelUp()
        {
            if (CanLevelUp())
            {
                _currentLevel.Value++;
                _currentExperience.Value = 0;
            }
        }
        public bool CanLevelUp()
        {
            return CurrentExperience.Value == RequiredExperience;
        }
    }
}