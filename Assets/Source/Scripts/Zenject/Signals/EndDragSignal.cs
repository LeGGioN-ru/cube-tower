public struct EndDragSignal<T>
{
    public readonly T Element;

    public EndDragSignal(T presenter)
    {
        Element = presenter;
    }
}