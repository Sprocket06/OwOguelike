namespace OwOguelike.Levels;

public class Level
{
    public List<Entity> Entities { get; private set; }
    public int[,] TileMap { get; private set; }

    public Level(Size size)
    {
        Entities = new();
        TileMap = new int[size.Width, size.Height];
    }

    public override string ToString()
    {
        return base.ToString();
    }
}