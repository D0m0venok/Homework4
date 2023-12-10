using UniRx;

namespace Homework4
{
    public sealed class CharacterStatPM : ICharacterStatPM
    {
        private readonly CharacterStat _stat;
        
        private readonly ReactiveProperty<int> _value;
        
        public string Name { get; private set; }
        public IReadOnlyReactiveProperty<int> Value => _value;

        public CharacterStatPM(CharacterStat stat)
        {
            _stat = stat;
            Name = _stat.Name;

            _value = new ReactiveProperty<int>(_stat.Value);
            _stat.OnValueChanged += ChangeValue;
        }
        public void ChangeValue(int value)
        {
            _value.Value = value;
        }
        public void Dispose()
        {
            _stat.OnValueChanged -= ChangeValue;
        }
    }
}