using UnityEngine;
using Zenject;

namespace Homework4
{
    public sealed class UIInstaller : MonoInstaller
    {
        [SerializeField] private UserPopup _userPopup;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UserPopup>().FromInstance(_userPopup).AsCached();
        }
    }
}