namespace OwOguelike.Attributes;

public class MusicAttribute : NoPreloadAttribute
{
    public string Name;
    public string Author;
    
    public MusicAttribute(string name = "", string author = "")
    {
        Name = name;
        Author = author;
    }
}