namespace OwOguelike.UI.Controls;

public class Panel : LayoutControl
{
    public float Radius { get; set; }
    public Color Color { get; set; }
    
    public Panel(int x = 0, int y = 0, int w = 0, int h = 0) : base(x, y, w, h)
    {
    }

    public Panel(Vector2 pos, Size size) : base(pos, size)
    {
    }

    public Panel(Vector2 pos, int w = 0, int h = 0) : base(pos, w, h)
    {
    }

    public override void Draw(RenderContext context)
    {
        if(Radius > 0)
            context.RoundedRect(ShapeMode.Fill, Position, Size, Radius, Color);
        else
            context.Rectangle(ShapeMode.Fill, X, Y, Width, Height, Color);
        base.Draw(context);
    }
}