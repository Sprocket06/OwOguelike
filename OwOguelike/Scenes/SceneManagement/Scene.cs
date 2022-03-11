namespace OwOguelike.Scenes.SceneManagement;

public abstract class Scene
{
    public virtual void LoadStep()
    {
    }

    public virtual int GetLoadSteps() => 0;
    
    public virtual void Draw(RenderContext context)
    {
    }

    public virtual void Update(float delta)
    {
    }

    public virtual void ControlAction(ControlEventArgs e)
    {
        
    }
    
    public virtual void MouseMoved(MouseMoveEventArgs e)
    {
    }

    public virtual void MousePressed(MouseButtonEventArgs e)
    {
    }

    public virtual void MouseReleased(MouseButtonEventArgs e)
    {
    }

    public virtual void WheelMoved(MouseWheelEventArgs e)
    {
    }

    public virtual void KeyPressed(KeyEventArgs e)
    {
    }

    public virtual void KeyReleased(KeyEventArgs e)
    {
    }

    public virtual void TextInput(TextInputEventArgs e)
    {
    }
    
    public virtual void ControllerConnected(ControllerEventArgs e)
    {
    }

    public virtual void ControllerDisconnected(ControllerEventArgs e)
    {
    }

    public virtual void ControllerButtonPressed(ControllerButtonEventArgs e)
    {
    }

    public virtual void ControllerButtonReleased(ControllerButtonEventArgs e)
    {
    }

    public virtual void ControllerAxisMoved(ControllerAxisEventArgs e)
    {
    }
}