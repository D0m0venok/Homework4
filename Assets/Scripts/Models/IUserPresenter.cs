using UniRx;
using UnityEngine;

namespace Homework4
{
    public interface IPresenter
    {
        public IReadOnlyReactiveProperty<string> Name { get; }
        public IReadOnlyReactiveProperty<string> Description { get; }
        public IReadOnlyReactiveProperty<Sprite> Icon { get; }
        public IReadOnlyReactiveProperty<string> CurrentLevel { get; }
        public IReadOnlyReactiveProperty<int> CurrentExperience { get; }
        public int RequiredExperience { get; }
        public IReadOnlyReactiveCollection<CharacterStat> Stats { get; }
        public bool CanLevelUp { get; }
        public void Hide();
        public void LevelUp();
    }
}