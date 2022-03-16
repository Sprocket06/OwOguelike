namespace OwOguelike.UI.Controls;

public class Button : TextControl, IClickable
{
    public event IClickable.ClickEventHandler? Click;

    public Button(string text = "Button", IClickable.ClickEventHandler? action = null)
    {
        Text = text;
        Click += action ?? ((_, _) => false);
    }

    public override void Draw(RenderContext context)
    {
        context.DrawString(Font, Text, Vector2.Zero, Color.White);
        RenderSettings.LineThickness = 2;
        context.Rectangle(ShapeMode.Stroke, Vector2.Zero, Size, Color.Red);
        base.Draw(context);
    }

    public bool OnReleased(MouseButtonEventArgs e)
    {
        return Click?.Invoke(this, e) ?? false;
    }
}