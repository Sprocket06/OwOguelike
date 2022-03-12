namespace OwOguelike.Input;

public class ControlState
{
    public Dictionary<ControlButton, bool> Buttons { get; private set; } 
        = new(Enum.GetValues<ControlButton>().Select(v => new KeyValuePair<ControlButton, bool>(v, false)));

    public Dictionary<ControlAxis, short> Axes { get; private set; } =
        new(Enum.GetValues<ControlAxis>().Select(v => new KeyValuePair<ControlAxis, short>(v, 0)));
}