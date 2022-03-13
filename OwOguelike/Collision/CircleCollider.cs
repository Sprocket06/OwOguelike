namespace OwOguelike.Collision;

public class CircleCollider : Collider
{
    public float Radius;

    public CircleCollider(Vector2 pos, float r)
    {
        this.Radius = r;
        this.Position = pos;
    }
    
    public override bool CollidesWith(Collider other)
    {
        switch (other)
        {
            case RectangleCollider r:
            {
                var rect = new c2AABB(r.Position, r.Position + r.Size);
                var circle = new c2Circle(this.Position, this.Radius);

                return c2CircletoAABB(circle, rect);
            }
            case CircleCollider c:
            {
                var circleA = new c2Circle(this.Position, this.Radius);
                var circleB = new c2Circle(c.Position, c.Radius);

                return c2CircletoCircle(circleA, circleB);
            }
            default:
                return false;
        }
    }
}