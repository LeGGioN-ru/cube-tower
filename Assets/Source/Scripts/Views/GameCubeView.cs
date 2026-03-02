using UnityEngine;

public class GameCubeView : CubeView
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Collider2D _collider2D;

    public override Sprite CurrentSprite => _spriteRenderer.sprite;
    public Collider2D Collider2D => _collider2D;
    public SpriteRenderer SpriteRenderer => _spriteRenderer;

    public override void UpdateSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
    }
}