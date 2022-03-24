namespace OwOguelike.Entities;

public class Actor : Entity, IDrawable, ICollisionEntity, IMovable
{
    public Collider Collider { get; set; } = new RectangleCollider(new(0,0), new(20, 20));
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

    public Actor(Vector2 pos)
    {
        this.Position = pos;
        this.Collider.Position = pos;
    }
    
    public void Update(float delta)
    {
        throw new NotImplementedException();
    }

    public void Draw(RenderContext context)
    {
        context.Rectangle(ShapeMode.Fill, this.Position, 20, 20, Color.Aqua);
    }
    
    public void OnCollision(ICollisionEntity other)
    {
        if (other is Dummy d)
        {
            CollisionData data = this.Collider.GetCollisionData(other.Collider);

            this.Position += -data.Normal * (data.Depths[0]);
        }
    }
}