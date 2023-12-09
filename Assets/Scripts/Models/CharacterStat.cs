using UniRx;

namespace Homework4
{
    public sealed class CharacterStat : ICharacterStatPM
    {
        private readonly ReactiveProperty<int> _value = new();
        
        public string Name { get; private set; }
        public IReadOnlyReactiveProperty<int> Value => _value;

        public CharacterStat(string name)
        {
            Name = name;
        }
        public void ChangeValue(int value)
        {
            _value.Value = value;
        }
    }
}