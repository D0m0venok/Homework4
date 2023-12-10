using UniRx;

namespace Homework4
{
    public interface ICharacterStatPM
    {
        string Name { get; }
        IReadOnlyReactiveProperty<int> Value { get; }

        void ChangeValue(int value);
        void Dispose();
        
    }
}