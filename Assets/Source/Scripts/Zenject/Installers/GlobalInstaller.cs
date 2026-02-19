using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private CubesDatabase _cubesDatabase;
    [SerializeField] private CubesFrontendData _cubesFrontendData;

    public override void InstallBindings()
    {
        Container.BindInstance(_cubesFrontendData).AsSingle();
        Container.Bind<ICubesDatabase>().FromInstance(_cubesDatabase).AsSingle();
        SignalBusInstaller.Install(Container);
        Container.Bind<CubeFactory>().AsSingle();

        Container.DeclareSignal<DragSignal<CubePresenter>>();
        Container.DeclareSignal<BeginDragSignal<CubePresenter>>();
        Container.DeclareSignal<EndDragSignal<CubePresenter>>();
    }
}