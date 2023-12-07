using UnityEngine;
using Zenject;

namespace Homework4
{
    public sealed class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private UserPopupShower _popupShower;
        [SerializeField] private UserPopup _userPopup;
        [SerializeField] private CharacterStatView _characterStatView;
        [SerializeField] private Transform _activeContainer;
        [SerializeField] private Transform _disableContainer;

        public override void InstallBindings()
        {
            Container.Bind<UserPresenterFactory>().AsSingle();
            Container.Bind<StatPool>().AsSingle().WithArguments(_characterStatView, _activeContainer, _disableContainer).NonLazy();
            Container.Bind<UserPopup>().FromInstance(_userPopup).AsSingle();
            Container.Bind<UserPopupShower>().FromInstance(_popupShower).AsSingle();
        }
    }
}
