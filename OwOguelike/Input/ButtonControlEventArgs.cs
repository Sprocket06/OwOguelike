namespace OwOguelike.Input;

public class ButtonControlEventArgs
{
    public string DeviceId { get; set; }
    
    public ControlButton ControlButton { get; set; }

    public ButtonControlEventArgs(string device, ControlButton btn)
    {
        DeviceId = device;
        ControlButton = btn;
    }
}