using UniRx;
using Unity.VisualScripting;

namespace Homework4
{
    public sealed class CharacterStatsPresenter : ICharacterStatsPresenter
    {
        private readonly CharacterInfo _characterInfo;
        private readonly ReactiveDictionary<CharacterStat, ICharacterStatPM> _stats = new();

        public IReadOnlyReactiveDictionary<CharacterStat, ICharacterStatPM> Stats => _stats;

        public CharacterStatsPresenter(CharacterInfo characterInfo)
        {
            _characterInfo = characterInfo;
            foreach (var stat in _characterInfo.GetStats())
            {
                _stats.Add(stat, new CharacterStatPM(stat));
            }

            _characterInfo.OnStatAdded += OnCharacterStatAdded;
            _characterInfo.OnStatRemoved += OnCharacterStatRemoved;
        }
        public void Dispose()
        {
            _characterInfo.OnStatAdded -= OnCharacterStatAdded;
            _characterInfo.OnStatRemoved -= OnCharacterStatRemoved;
            foreach (var statPm in _stats.Values)   
            {
                statPm.Dispose();
            }
        }
        
        private void OnCharacterStatAdded(CharacterStat stat)
        {
            _stats.Add(stat, new CharacterStatPM(stat));
        }
        private void OnCharacterStatRemoved(CharacterStat stat)
        {
            if(!_stats.TryGetValue(stat, out var statPm))
                return;
            
            statPm.Dispose();
            _stats.Remove(stat);
        }
    }
}