using UniRx;

namespace Homework4
{
    public interface ICharacterStatPM
    {
        public string Name { get; }
        public IReadOnlyReactiveProperty<int> Value { get; }

        public void ChangeValue(int value);
    }
}