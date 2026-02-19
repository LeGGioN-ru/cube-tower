using UnityEngine;

public class PhantomCube : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private LayerMask _zoneLayer;
    [SerializeField] private Vector2 _detectionSize = new Vector2(0.5f, 0.5f);

    public Vector3 EndPosition { get; private set; }
    public Vector3 StartPosition { get; private set; }
    public CubePresenter Presenter { get; private set; }

    public void Initialize(CubePresenter presenter, Vector3 startPosition)
    {
        _renderer.sprite = presenter.CubeView.CurrentSprite;
        Presenter = presenter;
        StartPosition = startPosition;
        transform.position = startPosition;
    }

    public void SetEndPosition(Vector3 endPosition)
    {
        EndPosition = endPosition;
    }

    public bool TryGetPlaceableZone(out PlacableZone<PhantomCube> foundZone)
    {
        return Utility.TryGetOverlapComponent(transform.position, _detectionSize, _zoneLayer, out foundZone);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(_detectionSize.x, _detectionSize.y, 0.1f));
    }
}