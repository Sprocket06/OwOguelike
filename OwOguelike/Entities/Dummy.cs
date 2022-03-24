namespace OwOguelike.Entities;

public class Dummy : Entity, ICollisionEntity, IDrawable
{
    public Collider Collider { get; set; }

    public Dummy(Vector2 pos)
    {
        this.Position = pos;
        Collider = new RectangleCollider(pos, new Vector2(50,50));
    }
    
    public void OnCollision(ICollisionEntity other)
    {
    }

    public DrawLayer Layer { get; set; }
    public void Update(float delta)
    {
        throw new NotImplementedException();
    }

    public void Draw(RenderContext context)
    {
        context.Rectangle(ShapeMode.Fill, this.Position, 50, 50, Color.Red);
    }
}