public readonly struct BeginDragSignal<T>
{
    public readonly T Element;

    public BeginDragSignal(T element)
    {
        Element = element;
    }
}
