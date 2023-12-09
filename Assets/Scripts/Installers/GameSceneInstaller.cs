using UnityEngine;
using Zenject;

namespace Homework4
{
    public sealed class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private UserPopupShower _popupShower;
        

        public override void InstallBindings()
        {
            Container.Bind<UserPresenterFactory>().AsSingle();
            Container.Bind<UserPopupShower>().FromInstance(_popupShower).AsSingle();
        }
    }
}
