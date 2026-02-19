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

    public CubePresenter Create(CubeView template, CubeModel cubeModel, Transform parent)
    {
        CubeView cubeView = _diContainer.InstantiatePrefabForComponent<CubeView>(template, parent);
        CubePresenter cubePresenter = new CubePresenter(cubeModel, cubeView, _frontendData.TakeSpriteByKey(cubeModel.ColorType));
        _diContainer.Inject(cubePresenter);

        return cubePresenter;
    }
}