namespace OwOguelike.Entities;

public class Actor : Entity, IDrawable, ICollidable, IMovable
{
    public DrawLayer Layer { get; set; } = DrawLayer.Characters;

    public float Friction { get; set; } = IMovable.DEFAULT_FRICTION;
    public float Velocity { get; set; } = IMovable.DEFAULT_VELOCITY;
    
    public void Update(float delta)
    {
        throw new NotImplementedException();
    }

    public void Draw(RenderContext context)
    {
        throw new NotImplementedException();
    }
}