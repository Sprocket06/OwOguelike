namespace OwOguelike.Entities;

[Drawable(5)]
public abstract class Entity
{
    public Vector2 Position;

    public float X
    {
        get => Position.X;
        set => Position.X = value;
    }

    public float Y
    {
        get => Position.Y;
        set => Position.Y = value;
    }

    public virtual void Update(float delta)
    {
    }

    public virtual void Draw(RenderContext context)
    {
    }
}