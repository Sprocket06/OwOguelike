namespace OwOguelike.Entities;

public class Dummy : Entity, IDrawable, IMovable
{
    public DrawLayer Layer { get; set; } = DrawLayer.Characters;
    
    public float MovementAcceleration { get; set; } = 2000f;
    public float MovementDeceleration { get; set; } = 2000f;
    public Vector2 Velocity { get; set; } = new(0,0);
    public float Acceleration { get; set; } = 0;
    
    public void Update(float delta)
    {
        var closest = GetClosestOfType<Actor>();
        if (closest is not null)
            Position += Vector2.Normalize(closest.Position - Position) * 50 * delta;
    }

    public void Draw(RenderContext context)
    {
        context.Rectangle(ShapeMode.Fill, Position, 20, 20, Color.Green);
    }
}