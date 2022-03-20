namespace OwOguelike.Entities;

public abstract class Entity
{
    public Vector2 Position;

    public float X
    {
        get => Position.X;
        set => Position.X = value;
    }

    public float Y
    {
        get => Position.Y;
        set => Position.Y = value;
    }

    private float _facingAngle;
    public float FacingAngle
    {
        get => _facingAngle;
        set
        {
            _facingAngle = value % 360;
            
            if (_facingAngle < 0)
            {
                _facingAngle += 360;
            }
        }
    }
    
    public T? GetClosestOfType<T>() where T : Entity
    {
        Entity? tMin = null;
        var minDist = float.MaxValue;
        foreach (var e in LevelManager.ActiveLevel.Entities.Where(e => e is T))
        {
            var dist = Vector2.Distance(e.Position, Position);
            if (dist < minDist)
            {
                tMin = e;
                minDist = dist;
            }
        }
        
        return tMin as T;
    }
}
