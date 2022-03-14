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

    public static Level? ActiveLevel;

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
}