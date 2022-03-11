namespace OwOguelike.Scenes;

public class GameplayScene : Scene
{
    public override void LoadStep()
    {
    }

    public override int GetLoadSteps() => 0;

    public override void Draw(RenderContext context)
    {
        context.Clear(Color.Green);
    }
}