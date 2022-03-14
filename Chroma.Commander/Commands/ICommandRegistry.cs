namespace Chroma.Commander;

public interface ICommandRegistry
{
    public void RefreshItems();
    public string? GetAutoComplete(string input, int offset);
    public string Call(string name, params object[] args);
}