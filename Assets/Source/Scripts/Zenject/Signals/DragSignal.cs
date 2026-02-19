public readonly struct DragSignal<T>
{
   public readonly T Element;

   public DragSignal(T presenter)
   {
      Element = presenter;
   }
}
