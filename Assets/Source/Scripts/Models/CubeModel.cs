using System;
using UnityEngine;

[Serializable]
public class CubeModel
{
    [HideInInspector] public string Name;
    public CubeColorType ColorType;
    [HideInInspector] public float PositionX;
    [HideInInspector] public float PositionY;
    [HideInInspector] public float PositionZ;
}