namespace OwOguelike.Scenes;

public class GameplayScene : Scene
{
    public Level CurrentLevel;
    public Camera GameCam;
    public Dummy TestDummy;
    
    public override void LoadStep()
    {
    }

    public override int GetLoadSteps() => 0;

    public GameplayScene()
    {
        CurrentLevel = new(new Size(100, 100));
        GameCam = new Camera(0, 0, 1) // TODO: Figure out how Z axis works with camera (Not even sure cookie knows)
        {
            UseCenteredOrigin = true
        };
        TestDummy = new(new(250, 250));
        CurrentLevel.Entities.Add(TestDummy);
        GameCore.CollisionManager.Register(TestDummy);
    }

    public override void Update(float delta)
    {
        foreach (var p in SheepleManager.Players)
        {
            var puppet = p.Puppet;
            if (puppet is IMovable movable)
            {
                var playerMovement = p.AssembleVector(ControlAxis.LeftAxisX, ControlAxis.LeftAxisY);

                // intended angle of acceleration
                if (playerMovement.X == 0 && playerMovement.Y == 0)
                {
                    float speed = movable.Velocity.Length();
                    speed = Math.Max(0, speed - movable.MovementDeceleration * delta);
                    if (speed == 0) movable.Velocity = Vector2.Zero;
                    else movable.Velocity = Vector2.Normalize(movable.Velocity) * speed;
                }
                else
                {
                    playerMovement = Vector2.Normalize(playerMovement);
                    //now we multiply by our base accel * dt
                    playerMovement = playerMovement * movable.MovementAcceleration * delta;

                    // we now have our actual acceleration (change in velocity) for this frame
                    // we then apply this change in velocity to our velocity
                    movable.Velocity = playerMovement + movable.Velocity;

                    // now we need to make sure the total magnitude of our velocity is capped at our maximum
                    float speed = movable.Velocity.Length();
                    if (speed > IMovable.MAX_VELOCITY)
                        movable.Velocity = Vector2.Normalize(movable.Velocity) * IMovable.MAX_VELOCITY;
                }

                // now we apply velocity as movement over time as per usual
                puppet.Position += movable.Velocity * delta;
                if (puppet is ICollisionEntity entity)
                {
                    entity.Collider.Position = puppet.Position;
                    GameCore.CollisionManager.ProcessMovement(entity);
                }
            }
        }
    }

    public override void Draw(RenderContext context)
    {
        // Render world
        context.WithCamera(GameCam, () =>
        {
            for (var x = 0; x < CurrentLevel.TileMap.GetUpperBound(0); x++)
            {
                for (var y = 0; y < CurrentLevel.TileMap.GetUpperBound(1); y++)
                {
                    // TODO: Will just crash until we actually have tiles to draw
                    //DrawTile(context, x, y, CurrentLevel.TileMap[x,y]);
                }
            }

            foreach (var e in CurrentLevel.Entities)
            {
                if (e is IDrawable drawable)
                {
                    drawable.Draw(context);
                }
            }
        });
        
        // UI / Overlay
        context.DrawString("Press Enter or Start to join.", new(0, 0), Color.White);
    }

    private void DrawTile(RenderContext context, int x, int y, int type)
    {
        LevelManager.TileSpriteSheet.Position = new Vector2(x, y);
        LevelManager.TileSpriteSheet.CurrentFrame = type;
        LevelManager.TileSpriteSheet.Draw(context);
    }

    public override void ButtonControlPressed(ButtonControlEventArgs e)
    {
        if (SheepleManager.HasPlayer(e.DeviceId))
        {
            var player = SheepleManager.GetPlayer(e.DeviceId);
            SetButtonAndMap(player, e.ControlButton, true);
        }
        else if (e.ControlButton is ControlButton.Menu) // yeah this control flow is a bit wack deal with it
        {
            var newPlayer = SheepleManager.AddPlayer(e.DeviceId);
            newPlayer.Puppet = new Actor(new(100, 100));

            CurrentLevel.Entities.Add(newPlayer.Puppet);
            if (newPlayer.Puppet is ICollisionEntity entity)
            {
                GameCore.CollisionManager.Register(entity);
            }
        }
    }

    public override void ButtonControlReleased(ButtonControlEventArgs e)
    {
        if (SheepleManager.HasPlayer(e.DeviceId))
        {
            var player = SheepleManager.GetPlayer(e.DeviceId);
            SetButtonAndMap(player, e.ControlButton, false);
        }
        else if (e.ControlButton is ControlButton.Menu) // yeah this control flow is a bit wack deal with it
        {
            var newPlayer = SheepleManager.AddPlayer(e.DeviceId);
            newPlayer.Puppet = new Actor(new(100, 100));

            CurrentLevel.Entities.Add(newPlayer.Puppet);
            if (newPlayer.Puppet is ICollisionEntity entity)
            {
                GameCore.CollisionManager.Register(entity);
            }
        }
    }

    public override void AxisControlMoved(AxisControlEventArgs e)
    {
        if (SheepleManager.HasPlayer(e.DeviceId))
        {
            var player = SheepleManager.GetPlayer(e.DeviceId);
            player.ControlState.Axes[e.ControlAxis] = e.Value;
        }
    }

    private void SetButtonAndMap(Player p, ControlButton b, bool pressed)
    {
        var btns = p.ControlState.Buttons;
        var axes = p.ControlState.Axes;
        btns[b] = pressed;

        if (b is ControlButton.MoveDown or ControlButton.MoveLeft or ControlButton.MoveRight or ControlButton.MoveUp)
        {
            axes[ControlAxis.LeftAxisX] = (short)((btns[ControlButton.MoveLeft] ? -short.MaxValue : 0) +
                                                  (btns[ControlButton.MoveRight] ? short.MaxValue : 0));
            axes[ControlAxis.LeftAxisY] = (short)((btns[ControlButton.MoveUp] ? -short.MaxValue : 0) +
                                                  (btns[ControlButton.MoveDown] ? short.MaxValue : 0));
        }
    }
}