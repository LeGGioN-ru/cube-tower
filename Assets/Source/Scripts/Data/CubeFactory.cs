using UnityEngine;
using Zenject;

public class CubeFactory
{
    private DiContainer _diContainer;
    private CubesFrontendData _frontendData;

    public CubeFactory(DiContainer diContainer, CubesFrontendData cubesFrontendData)
    {
        _frontendData = cubesFrontendData;
        _diContainer = diContainer;
    }

    public CubePresenter Create(CubeView template, CubeModel sourceModel, Transform parent)
    {
        CubeView cubeView = _diContainer.InstantiatePrefabForComponent<CubeView>(template, parent);
        CubeModel cubeModel = new CubeModel()
        {
            ColorType = sourceModel.ColorType,
            PositionX = sourceModel.PositionX,
            PositionY = sourceModel.PositionY,
            PositionZ = sourceModel.PositionZ
        };
        CubePresenter cubePresenter = new CubePresenter(cubeModel, cubeView, _frontendData.TakeSpriteByKey(sourceModel.ColorType));
        _diContainer.Inject(cubePresenter);

        return cubePresenter;
    }
}