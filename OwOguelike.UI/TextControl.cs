using Chroma.Graphics.TextRendering.TrueType;

namespace OwOguelike.UI;

public class TextControl : Control
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

    private string _text;

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

    public TextControl(int x, int y, Size size) : base(x, y, size)
    {
    }

    public virtual void Measure()
    {
        Size = Font.Measure(Text);
    }
}