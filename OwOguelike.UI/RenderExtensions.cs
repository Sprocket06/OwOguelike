using System.Diagnostics;

namespace OwOguelike.UI;

public static class RenderExtensions
{
    public static void RoundedRect(this RenderContext ctx, ShapeMode mode, float x, float y, float width, float height,
        float radius, Color color)
    {
        if (radius * 2 > height || radius * 2 > width)
            throw new Exception(
                "Tried to draw a rounded rect with a edge diameter of greater than it's width or height");

        if (mode == ShapeMode.Fill)
        {
            ctx.Rectangle(mode, x, y + radius, width, height - (radius * 2), color);
            ctx.Rectangle(mode, x + radius, y, width - (radius * 2), height, color);
        }
        else
        {
            ctx.Line(x + radius, y, x + (width - radius), y, color);
            ctx.Line(x + width, y + radius, x + width, y + (height - radius), color);
            ctx.Line(x + (width - radius), y + height, x + radius, y + height, color);
            ctx.Line(x, y + (height - radius), x, y + radius, color);
        }
        
        ctx.Arc(mode, x + (width - radius), y + (height - radius), radius, 0, 90, color);
        ctx.Arc(mode, x + radius, y + (height - radius), radius, 90, 180, color);
        ctx.Arc(mode, x + radius, y + radius, radius, 180, 270, color);
        ctx.Arc(mode, x + (width - radius), y + radius, radius, -90, 0, color);
    }

    public static void RoundedRect(this RenderContext ctx, ShapeMode mode, Vector2 pos, float width, float height,
        float radius, Color color) => ctx.RoundedRect(mode, pos.X, pos.Y, width, height, radius, color);

    public static void RoundedRect(this RenderContext ctx, ShapeMode mode, Vector2 pos, Size size, float radius,
        Color color) => ctx.RoundedRect(mode, pos.X, pos.Y, size.Width, size.Height, radius, color);
}