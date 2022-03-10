namespace OwOguelike.Input;

public class InputNotBoundException : Exception
{
    public InputNotBoundException()
    {
        
    }

    public InputNotBoundException(string message) : base(message)
    {
        
    }

    public InputNotBoundException(string message, Exception inner) : base(message, inner)
    {
        
    }
    
}