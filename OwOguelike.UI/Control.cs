namespace OwOguelike.UI;

public abstract class Control
{
    private readonly List<Control> _children;
    public ImmutableList<Control> Children => _children.ToImmutableList();
    
    public static Control? FocusedControl { get; set; }
    public bool Focused => FocusedControl == this;

    public Control()
    {
        _children = new();
    }

    public void AddChild(Control child)
    {
        _children.Add(child);
    }
    
    public virtual void Draw(RenderContext context)
    {
        foreach (var control in Children)
        {
            control.Draw(context);
        }
    }

    public virtual void MouseButtonPressed(MouseButtonEventArgs e)
    {
        foreach (var control in Children)
        {
            control.MouseButtonPressed(e);
        }
    }

    public virtual void MouseButtonReleased(MouseButtonEventArgs e)
    {
        foreach (var control in Children)
        {
            control.MouseButtonReleased(e);
        }
    }
}