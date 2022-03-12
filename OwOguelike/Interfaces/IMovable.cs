namespace OwOguelike.Interfaces;

public interface IMovable
{
    public const float MAX_VELOCITY = 200f;
    
    public abstract float MovementDeceleration { get; set; }
    public abstract float Acceleration { get; set; }
    public abstract float MovementAcceleration { get; set; }
    public abstract Vector2 Velocity { get; set; }
}