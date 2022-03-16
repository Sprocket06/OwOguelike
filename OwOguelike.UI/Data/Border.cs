namespace OwOguelike.UI.Data;

public class Border
{
    public Color Color = Color.White;
    public float LineThickness = RenderSettings.LineThickness;
    public float Radius = 0;

    public Border()
    {
    }

    public Border(Color color, float thickness, float radius)
    {
        Color = color;
        LineThickness = thickness;
        Radius = radius;
    }
}