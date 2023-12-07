using System;
using UniRx;
using UnityEngine;

namespace Homework4
{
    public sealed class UserInfo
    {
        private readonly ReactiveProperty<string> _name = new();
        private readonly ReactiveProperty<string> _description = new();
        private readonly ReactiveProperty<Sprite> _icon = new();

        public IReadOnlyReactiveProperty<string> Name => _name;
        public IReadOnlyReactiveProperty<string> Description => _description;
        public IReadOnlyReactiveProperty<Sprite> Icon => _icon;

        public void ChangeName(string name)
        {
            _name.Value = name;
        }
        public void ChangeDescription(string description)
        {
            _description.Value = description;
        }
        public void ChangeIcon(Sprite icon)
        {
            _icon.Value = icon;
        }
    }
}