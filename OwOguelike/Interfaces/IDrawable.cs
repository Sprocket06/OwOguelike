namespace OwOguelike.Interfaces;

public interface IDrawable
{
    public abstract DrawLayer Layer { get; set; }
    
    public abstract void Update(float delta);

    public abstract void Draw(RenderContext context);
}