using UniRx;

namespace Homework4
{
    public interface ICharacterStatsPresenter
    {
        public IReadOnlyReactiveDictionary<CharacterStat, ICharacterStatPM> Stats { get; }
        
        public void Dispose();
    }
}