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
}
