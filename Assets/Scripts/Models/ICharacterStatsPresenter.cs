using UniRx;

namespace Homework4
{
    public interface ICharacterStatsPresenter
    {
        public IReadOnlyReactiveCollection<ICharacterStatPM> Stats { get; }
        
        public void Dispose();
    }
}