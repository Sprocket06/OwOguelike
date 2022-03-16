namespace OwOguelike.UI.Contracts;

public interface IInteractable
{
    public virtual bool OnHoverEnter(MouseMoveEventArgs e)
    {
        return false;
    }

    public virtual bool OnHoverExit(MouseMoveEventArgs e)
    {
        return false;
    }
}