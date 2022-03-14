namespace OwOguelike.Input;

public class Player
{
    public int PlayerNum { get; set; }
    public ControlProfile Profile { get; set; }
    public string InputID { get; set; }

    public ControlState ControlState { get; set; } = new();
    public Entity? Puppet { get; set; }

    public Player(int index, ControlProfile map, string inputId)
    {
        PlayerNum = index;
        Profile = map;
        InputID = inputId;
    }

    public bool GetButtonDown(ControlButton btn) => ControlState.Buttons[btn];
    
    public bool GetButtonUp(ControlButton btn) => !ControlState.Buttons[btn];

    public short GetRawAxis(ControlAxis axis) => ControlState.Axes[axis];

    public float GetRawNormalizedAxis(ControlAxis axis) =>
        ControlState.Axes[axis] != 0 ? (float)ControlState.Axes[axis] / short.MaxValue : 0;

    public short GetAxis(ControlAxis axis)
    {
        var val = GetRawAxis(axis);
        return Math.Abs((float)val) / short.MaxValue < Configuration.CurrentConfig.StickDeadzone ? (short)0 : val;
    }

    public float GetNormalizedAxis(ControlAxis axis)
    {
        var val = GetRawAxis(axis);
        return val != 0 ? (float)val / short.MaxValue : 0;
    }

    public Vector2 AssembleRawVector(ControlAxis x, ControlAxis y) =>
        new(GetRawNormalizedAxis(x), GetRawNormalizedAxis(y));

    public Vector2 AssembleVector(ControlAxis x, ControlAxis y) => new(GetNormalizedAxis(x), GetNormalizedAxis(y));
}