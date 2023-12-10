using System;
using UniRx;

namespace Homework4
{
    public sealed class CharacterInfo
    {
        private readonly ReactiveCollection<CharacterStat> _stats = new();
        public IReadOnlyReactiveCollection<CharacterStat> Stats => _stats;

        public void AddStat(CharacterStat stat)
        {
            _stats.Add(stat);
        }
        public void RemoveStat(CharacterStat stat)
        {
            _stats.Remove(stat);
        }
        public CharacterStat GetStat(string name)
        {
            foreach (var stat in _stats)
            {
                if (stat.Name == name)
                {
                    return stat;
                }
            }

            throw new Exception($"Stat {name} is not found!");
        }
    }
}