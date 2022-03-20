namespace OwOguelike.Levels;

public class LevelManager
{
    private const string TILEMAP_PATH = "Textures/TileMap.png";
    private const int TILE_SIZE = 16;
    
    private static SpriteSheet? _tilesSpriteSheet;

    public static SpriteSheet TileSpriteSheet
    {
        get
        {
            if(_tilesSpriteSheet is null)
                LoadTiles();
            return _tilesSpriteSheet!;
        }
    }

    private static Level? _level;

    public static Level ActiveLevel => _level ?? throw new LevelNotLoadedException();

    public static void LoadTiles()
    {
        try
        {
            _tilesSpriteSheet = new SpriteSheet(ContentProvider!.Load<Texture>(TILEMAP_PATH), TILE_SIZE, TILE_SIZE);
        }
        catch (Exception e)
        {
            GameCore.Log.Error($"Could not load tiles, this will cause major issues!\n{e}");
        }
    }

    public static Level LoadLevel(Level l)
    {
        _level = l;
        return l;
    }

    public static Entity SpawnEntity<T>(Vector2? pos = null) where T : Entity, new()
    {
        if (ActiveLevel is null)
            throw new LevelNotLoadedException();

        var entity = new T();
        if (pos is not null)
            entity.Position = pos.Value;
        ActiveLevel.Entities.Add(entity);
        return entity;
    }

    [ConVar("spawn_entity")]
    public static Entity SpawnEntity(Entity? e)
    {
        if (e is null)
            throw new ArgumentNullException();
        
        if (ActiveLevel is null)
            throw new LevelNotLoadedException();
        
        ActiveLevel.Entities.Add(e);
        return e;
    }

    [TypeConverter]
    public static Entity? StringToEntity(string name)
    {
        var type = Type.GetType($"{nameof(OwOguelike)}.{nameof(Entities)}.{name}");
        if (type is not null && type.IsAssignableTo(typeof(Entity)))
        {
            return SpawnEntity(Activator.CreateInstance(type) as Entity);
        }

        return null;
    }
}