namespace OwOguelike.Entities;

public class Actor : Entity, IDrawable, ICollidable, IMovable
{

    public DrawLayer Layer { get; set; } = DrawLayer.Characters;
    
    /* Current Movement Physics:
     * Max Vel: 200px/sec
     * Accel: 2000px/sec
     * 0-Max Speed in: 0.1 sec
     * Decel same as Accel
     */
    public float MovementAcceleration { get; set; } = 2000f;
    public float MovementDeceleration { get; set; } = 2000f;
    public Vector2 Velocity { get; set; } = new(0,0);
    public float Acceleration { get; set; } = 0;
    
    
    public void Update(float delta)
    {
        throw new NotImplementedException();
    }

    public void Draw(RenderContext context)
    {
        throw new NotImplementedException();
    }
}