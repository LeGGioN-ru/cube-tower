using UnityEngine;
using UnityEngine.UI;

public class UICubeView : CubeView
{
    [SerializeField] private Image _image;

    public override Sprite CurrentSprite => _image.sprite;

    public override void UpdateSprite(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}