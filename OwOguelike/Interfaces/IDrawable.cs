namespace OwOguelike.Interfaces;

public interface IDrawable : IUpdating
{
    public abstract DrawLayer Layer { get; set; }

    public abstract void Draw(RenderContext context);
}