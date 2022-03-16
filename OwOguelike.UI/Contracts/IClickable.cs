namespace OwOguelike.UI.Contracts;

public interface IClickable : IInteractable
{
    public delegate bool ClickEventHandler(object? sender, MouseButtonEventArgs e);
    
    public event ClickEventHandler? Click;
    
    public virtual bool OnPressed(MouseButtonEventArgs e)
    {
        return false;
    }

    public virtual bool OnReleased(MouseButtonEventArgs e)
    {
        return false;
    }
}