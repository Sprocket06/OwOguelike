namespace OwOguelike.Scenes;

public class GameplayScene : Scene
{
    public override bool LoadStep()
    {
        return base.LoadStep();
    }

    public override void Draw(RenderContext context)
    {
        context.Clear(Color.Green);
    }
}