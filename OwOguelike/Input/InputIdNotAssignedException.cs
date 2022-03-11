namespace OwOguelike.Input;

public class InputIdNotAssignedException : Exception
{
    public InputIdNotAssignedException()
    {
    }

    public InputIdNotAssignedException(string message) : base(message)
    {
    }

    public InputIdNotAssignedException(string message, Exception inner) : base(message, inner)
    {
    }
    
}