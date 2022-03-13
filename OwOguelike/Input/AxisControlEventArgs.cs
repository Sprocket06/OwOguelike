namespace OwOguelike.Input;

public class AxisControlEventArgs
{
    public string DeviceId { get; set; }
    public ControlAxis ControlAxis { get; set; }
    public short Value { get; set; }

    public AxisControlEventArgs(string device, ControlAxis axis, short val)
    {
        DeviceId = device;
        ControlAxis = axis;
        Value = val;
    }
}