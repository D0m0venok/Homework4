using System;

namespace Homework4
{
    public sealed class CharacterStat
    {
        public event Action<int> OnValueChanged; 
        
        public string Name { get; private set; }
        public int Value { get; private set; }


        public CharacterStat(string name)
        {
            Name = name;
        }
        public void ChangeValue(int value)
        {
            Value = value;
            OnValueChanged?.Invoke(value);
        }
    }
}