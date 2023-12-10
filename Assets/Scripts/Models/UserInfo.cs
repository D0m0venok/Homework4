using System;
using UnityEngine;

namespace Homework4
{
    public sealed class UserInfo
    {
        public event Action<string> OnNameChanged;
        public event Action<string> OnDescriptionChanged;
        public event Action<Sprite> OnIconChanged; 

        public string Name { get; private set; }
        public string Description { get; private set; }
        public Sprite Icon { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
            OnNameChanged?.Invoke(name);
        }
        public void ChangeDescription(string description)
        {
            Description = description;
            OnDescriptionChanged?.Invoke(description);
        }
        public void ChangeIcon(Sprite icon)
        {
            Icon = icon;
            OnIconChanged?.Invoke(icon);
        }
    }
}