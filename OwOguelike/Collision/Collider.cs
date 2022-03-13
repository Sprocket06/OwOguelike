namespace OwOguelike.Collision;

public abstract class Collider
{
    public Vector2 Position;
    public abstract bool CollidesWith(Collider other);
}