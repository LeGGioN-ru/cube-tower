using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CubesListLoader : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private UICubeView _template;

    [Inject] private ICubesDatabase _cubesDatabase;
    [Inject] private CubeFactory _factory;

    private async void Start()
    {
        List<CubeModel> models = await _cubesDatabase.TakeCubes();
        
        foreach (CubeModel model in models)
        {
            _factory.Create(_template, model, _container);
        }
    }
}