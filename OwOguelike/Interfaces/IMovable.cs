namespace OwOguelike.Interfaces;

public interface IMovable
{
    public const float DEFAULT_FRICTION = 10f;
    public const float DEFAULT_VELOCITY = 0f;
    
    public abstract float Friction { get; set; }
    public abstract float Velocity { get; set; }
}