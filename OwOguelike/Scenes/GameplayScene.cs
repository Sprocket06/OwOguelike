namespace OwOguelike.Scenes;

public class GameplayScene : Scene
{
    public Level CurrentLevel;

    public override void LoadStep()
    {
    }

    public override int GetLoadSteps() => 0;

    public GameplayScene()
    {
        CurrentLevel = new(new Size(100, 100));
    }

    public override void Update(float delta)
    {
        foreach (var p in SheepleManager.Players)
        {
            
            var puppet = p.Puppet;
            if (puppet is IMovable movable)
            {
                
                Vector2 playerMovement = new(0,0);
                if (p.ControlState.Buttons[ControlButton.MoveUp])
                {
                    playerMovement.Y -= 1;
                }

                if (p.ControlState.Buttons[ControlButton.MoveDown])
                {
                    playerMovement.Y += 1;
                }

                if (p.ControlState.Buttons[ControlButton.MoveLeft])
                {
                    playerMovement.X -= 1;
                }

                if (p.ControlState.Buttons[ControlButton.MoveRight])
                {
                    playerMovement.X += 1;
                }

                Vector2 newVelocity = new(0, 0);
                if (playerMovement.X != 0)
                {
                    newVelocity.X = Math.Min(IMovable.MAX_VELOCITY, movable.Velocity.X + (movable.MovementAcceleration * delta));
                }
                else
                {
                    newVelocity.X = Math.Max(0, movable.Velocity.X - (movable.MovementDeceleration * delta));
                }
                if (playerMovement.Y != 0)
                {
                    newVelocity.Y = Math.Min(IMovable.MAX_VELOCITY,
                        movable.Velocity.Y + (movable.MovementAcceleration * delta));
                }
                else
                {
                    newVelocity.Y = Math.Max(0, movable.Velocity.Y - (movable.MovementDeceleration * delta));
                }

                movable.Velocity = newVelocity;

                puppet.Position = puppet.Position + movable.Velocity * playerMovement * delta;
            }
            
        }
    }

    public override void Draw(RenderContext context)
    {
        context.DrawString("Press Enter or Start to join.", new(0,0), Color.White);
        
        for (var x = 0; x < CurrentLevel.TileMap.GetUpperBound(0); x++)
        {
            for (var y = 0; y < CurrentLevel.TileMap.GetUpperBound(1); y++)
            {
                // TODO: Will just crash until we actually have tiles to draw
                //DrawTile(context, x, y, CurrentLevel.TileMap[x,y]);
            }
        }
        
        foreach(var e in CurrentLevel.Entities)
        {
            if (e is IDrawable)
            {
                context.Rectangle(ShapeMode.Fill, e.Position, 10, 10, Color.Aqua);
            }
        }
    }

    private void DrawTile(RenderContext context, int x, int y, int type)
    {
        LevelManager.TileSpriteSheet.Position = new Vector2(x, y);
        LevelManager.TileSpriteSheet.CurrentFrame = type;
        LevelManager.TileSpriteSheet.Draw(context);
    }

    public override void ButtonControlPressed(ButtonControlEventArgs e)
    {
        bool test = SheepleManager.HasPlayer(e.DeviceId);
        if (SheepleManager.HasPlayer(e.DeviceId))
        {
            var player = SheepleManager.GetPlayer(e.DeviceId);
            player.ControlState.Buttons[e.ControlButton] = true;
        } 
        else if (e.ControlButton is ControlButton.Menu) // yeah this control flow is a bit wack deal with it
        {
            var newPlayer = SheepleManager.AddPlayer(e.DeviceId);
            newPlayer.Puppet = new Actor();

            newPlayer.Puppet.Position = new(100, 100);

            CurrentLevel.Entities.Add(newPlayer.Puppet);
        }
    }

    public override void ButtonControlReleased(ButtonControlEventArgs e)
    {
        if (SheepleManager.HasPlayer(e.DeviceId))
        {
            var player = SheepleManager.GetPlayer(e.DeviceId);
            player.ControlState.Buttons[e.ControlButton] = false;
        } 
        else if (e.ControlButton is ControlButton.Menu) // yeah this control flow is a bit wack deal with it
        {
            var newPlayer = SheepleManager.AddPlayer(e.DeviceId);
            newPlayer.Puppet = new Actor();

            newPlayer.Puppet.Position = new(100, 100);

            CurrentLevel.Entities.Add(newPlayer.Puppet);
        }
    }
}