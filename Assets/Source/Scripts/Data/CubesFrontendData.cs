using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Cube Tower/Cubes Frontend Data", fileName = "Cubes Frontend Data")]
public class CubesFrontendData : ScriptableObject
{
    [SerializeField] private List<CubeFrontendData> _cubesData;
    
    public IReadOnlyList<CubeFrontendData> CubesData => _cubesData;

    public Sprite TakeSpriteByKey(CubeColorType colorType)
    {
        return _cubesData.Find(data => data.Key == colorType).Sprite;
    }

    private void OnValidate()
    {
        foreach (CubeFrontendData frontendData in _cubesData)
        {
            frontendData.Name = frontendData.Key.ToString();
        }
    }
}

[Serializable]
public class CubeFrontendData
{
    [HideInInspector] public string Name;
    [SerializeField] private CubeColorType _key;
    [SerializeField] private Sprite _sprite;
    
    public CubeColorType Key => _key;
    public Sprite Sprite => _sprite;
}