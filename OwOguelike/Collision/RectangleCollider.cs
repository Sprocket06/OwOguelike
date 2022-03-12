namespace OwOguelike.Collision;

public class RectangleCollider : Collider
{
    public Vector2 Size;

    public RectangleCollider(Vector2 pos, Vector2 size)
    {
        this.Position = pos;
        this.Size = size;
    }

    public override bool CollidesWith(Collider other)
    {
        switch (other)
        {
            case RectangleCollider r:
            {
                var rectA = new c2AABB(this.Position, this.Position+this.Size);
                var rectB = new c2AABB(r.Position, r.Position + r.Size);

                return c2AABBtoAABB(rectA,rectB);
            }
            case CircleCollider c:
                var rect = new c2AABB(this.Position, this.Position + this.Size);
                var circle = new c2Circle(c.Position, c.Radius);

                return c2CircletoAABB(circle, rect);
            default:
                return false;
            
        }
    }
}