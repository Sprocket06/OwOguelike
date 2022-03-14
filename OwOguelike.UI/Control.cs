namespace OwOguelike.UI;

public abstract class Control
{
    public static Control? FocusedControl { get; set; }
    public bool Focused => FocusedControl == this;
    
    private readonly List<Control> _children;
    public ImmutableList<Control> Children => _children.ToImmutableList();
    
    public Control? Parent { get; private set; }

    private int _x;
    public int X
    {
        get => _x;
        set
        {
            _x = value;
            InvalidateLayout();
        }
    }

    private int _y;
    public int Y
    {
        get => _y;
        set
        {
            _y = value;
            InvalidateLayout();
        }
    }

    public Vector2 Position
    {
        get => new(X, Y);
        set
        {
            X = (int)value.X;
            Y = (int)value.Y;
        }
    }

    public Vector2 ScreenPosition
    {
        get
        {
            var pos = new Vector2();
            var next = Parent;
            while (next is not null)
            {
                pos += next.Position;
                next = next.Parent;
            }

            return pos;
        }
    }

    private int _w;
    public int Width
    {
        get => _w;
        set
        {
            _w = value;
            InvalidateLayout();
        }
    }
    
    private int _h;
    public int Height
    {
        get => _h;
        set
        {
            _h = value;
            InvalidateLayout();
        }
    }

    public Size Size
    {
        get => new(Width, Height);
        set
        {
            Width = value.Width;
            Height = value.Height;
        }
    }
    
    public bool ValidationEnabled { get; set; }

    public Control(int x = 0, int y = 0, int w = 0, int h = 0)
    {
        _x = x;
        _y = y;
        _w = w;
        _h = h;
        _children = new();
    }

    public Control(Vector2 pos, Size size) : this((int)pos.X, (int)pos.Y, size.Width, size.Height)
    {
    }

    public Control(Vector2 pos, int w = 0, int h = 0) : this((int)pos.X, (int)pos.Y, w, h)
    {
    }

    public Control(int x, int y, Size size) : this(x, y, size.Width, size.Height)
    {
    }

    public Control AddChild(Control child)
    {
        child.Parent = this;
        _children.Add(child);
        InvalidateLayout();
        return child;
    }

    public Control RemoveChild(Control child)
    {
        child.Parent = null;
        _children.Remove(child);
        InvalidateLayout();
        return child;
    }

    public Control RemoveChild(int index) => RemoveChild(_children[index]);

    public virtual void InvalidateLayout()
    {
        if(ValidationEnabled)
            Parent?.InvalidateLayout();
    }
    
    public virtual void Draw(RenderContext context)
    {
        RenderTransform.Translate(Position);
        foreach (var control in _children)
        {
            control.Draw(context);
        }
        RenderTransform.Translate(-Position);
    }

    public virtual void MouseButtonPressed(MouseButtonEventArgs e)
    {
        foreach (var control in _children)
        {
            control.MouseButtonPressed(e);
        }
    }

    public virtual void MouseButtonReleased(MouseButtonEventArgs e)
    {
        foreach (var control in _children)
        {
            control.MouseButtonReleased(e);
        }
    }
}