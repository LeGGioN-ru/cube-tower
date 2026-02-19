using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class CubeView : MonoBehaviour
{
    [SerializeField] private DragDetector _dragDetector;

    public DragDetector DragDetector => _dragDetector;

    public abstract Sprite CurrentSprite { get; }

    public abstract void UpdateSprite(Sprite sprite);
}