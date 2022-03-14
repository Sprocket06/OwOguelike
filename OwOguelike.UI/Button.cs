namespace OwOguelike.UI;

public class Button : Control
{
    public delegate bool ButtonDelegate(object sender);

    public string Text { get; set; }
    public ButtonDelegate OnPress { get; set; }

    public Button(string text = "Button", ButtonDelegate? action = null)
    {
        Text = text;
        OnPress = action ?? (_ => false) ;
    }
}