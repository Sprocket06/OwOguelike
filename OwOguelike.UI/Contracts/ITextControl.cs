namespace OwOguelike.UI.Contracts;

public interface ITextControl
{
    public IFontProvider Font { get; set; }
    
    public string Text { get; set; }
    
    public virtual void Measure()
    {
    }
}