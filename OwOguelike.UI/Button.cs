namespace OwOguelike.UI;

public class Button : TextControl
{
    public delegate bool ButtonDelegate(object sender);

    public ButtonDelegate OnPress { get; set; }

    public Button(string text = "Button", ButtonDelegate? action = null)
    {
        Text = text;
        OnPress = action ?? (_ => false);
    }

    public override void Draw(RenderContext context)
    {
        context.DrawString(Text, Vector2.Zero, Color.White);
        RenderSettings.LineThickness = 2;
        context.Rectangle(ShapeMode.Stroke, Vector2.Zero, Size, Color.Red);
        base.Draw(context);
    }
}