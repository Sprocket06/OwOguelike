namespace OwOguelike.Attributes;

public class DrawableAttribute : Attribute
{
    public int Layer;
    
    public DrawableAttribute(int layer)
    {
        Layer = layer;
    }
}