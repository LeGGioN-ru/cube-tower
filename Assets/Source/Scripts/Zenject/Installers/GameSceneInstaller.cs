using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BaseDragAndDropManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<NotificationManager>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<IGameSaveManager>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}