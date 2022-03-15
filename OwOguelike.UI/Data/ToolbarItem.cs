namespace OwOguelike.UI.Data;

public class ToolbarItem
{
    public string Name { get; set; }
    public List<ToolbarItem> Children { get; }

    public ToolbarItem(string name, params ToolbarItem[] children)
    {
        Name = name;
        Children = children.ToList();
    }
}