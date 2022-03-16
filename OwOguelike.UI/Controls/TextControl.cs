namespace OwOguelike.UI.Controls;

public abstract class TextControl : Control
{
    private IFontProvider? _font;

    public IFontProvider Font
    {
        get => _font ?? TrueTypeFont.Default;
        set
        {
            _font = value;
            Measure();
        }
    }

    private string _text = null!;

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            Measure();
        }
    }
    
    public TextControl(int x = 0, int y = 0, int w = 0, int h = 0) : base(x, y, w, h)
    {
    }

    public TextControl(Vector2 pos, Size size) : base(pos, size)
    {
    }

    public TextControl(Vector2 pos, int w = 0, int h = 0) : base(pos, w, h)
    {
    }

    public virtual void Measure()
    {
        Size = Font.Measure(Text);
    }
}