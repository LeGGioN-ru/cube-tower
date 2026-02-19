using UnityEngine;

public abstract class PlacableZone<T> : MonoBehaviour where T : Component
{
    public virtual void PlaceElement(T element)
    {
        
    }
}