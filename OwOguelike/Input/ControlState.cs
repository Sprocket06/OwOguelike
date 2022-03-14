namespace OwOguelike.Input;

public class ControlState
{
    public Dictionary<ControlButton, bool> Buttons { get; private set; }
        = new(Enum.GetValues<ControlButton>().Select(v => new KeyValuePair<ControlButton, bool>(v, false)));

    public Dictionary<ControlAxis, short> Axes { get; private set; } =
        new(Enum.GetValues<ControlAxis>().Select(v => new KeyValuePair<ControlAxis, short>(v, 0)));

    public void SetButton(ControlButton btn, bool value)
    {
        Buttons[btn] = value;

        if (btn is ControlButton.MoveDown or ControlButton.MoveLeft or ControlButton.MoveRight or ControlButton.MoveUp)
        {
            SetAxis(ControlAxis.LeftAxisX, (short)((Buttons[ControlButton.MoveLeft] ? -short.MaxValue : 0) +
                                                    (Buttons[ControlButton.MoveRight] ? short.MaxValue : 0)));
            SetAxis(ControlAxis.LeftAxisY, (short)((Buttons[ControlButton.MoveUp] ? -short.MaxValue : 0) +
                                                   (Buttons[ControlButton.MoveDown] ? short.MaxValue : 0)));
        }
    }

    public bool GetButton(ControlButton btn) => Buttons[btn];

    public void SetAxis(ControlAxis axis, short value) => Axes[axis] = value;

    public short GetRawAxis(ControlAxis axis) => Axes[axis];

    public short GetAxis(ControlAxis axis)
    {
        var val = GetRawAxis(axis);
        return Math.Abs((float)val) / short.MaxValue < Configuration.CurrentConfig.StickDeadzone ? (short)0 : val;
    }

    private Vector2 JoinAxes(short x, short y) =>
        new(x != 0 ? (float)x / short.MaxValue : 0,
            y != 0 ? (float)y / short.MaxValue : 0);

    public Vector2 AssembleRawVector(ControlAxis x, ControlAxis y) => JoinAxes(GetRawAxis(x), GetRawAxis(y));

    public Vector2 AssembleVector(ControlAxis x, ControlAxis y) => JoinAxes(GetAxis(x), GetAxis(y));
}