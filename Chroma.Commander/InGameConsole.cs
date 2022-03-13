using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;
using Chroma.MemoryManagement;
using Chroma.Windowing;

namespace Chroma.Commander
{
    public class InGameConsole : DisposableResource
    {
        private enum State
        {
            SlidingUp,
            SlidingDown,
            Hidden,
            Visible
        }

        private Window _window;
        private RenderTarget _target;
        private Vector2 _offset;
        private TrueTypeFont _ttf;
        private int _maxLines;

        private ScrollBuffer _scrollBuffer;
        private List<ConsoleLine>? _scrollBufferWindow;

        private InputLine _inputLine;

        private State _state = State.Hidden;

        private CommandRegistry _registry;

        public string Motd { get; set; } = 
            $"{Assembly.GetEntryAssembly()!.GetName().Name} Version {Assembly.GetEntryAssembly()!.GetName().Version}";

        public float SlidingSpeed { get; set; } = 2000;

        public InGameConsole(Window window, int maxLines = 20, Assembly? assembly = null, string? motd = null)
        {
            _window = window;
            _maxLines = maxLines;

            LoadFont();
            _target = new RenderTarget(
                window.Size.Width,
                maxLines * _ttf.Height + 2 + _ttf.Height + 2
            );

            _offset.Y = -_target.Height;
            _scrollBuffer = new ScrollBuffer(maxLines);
            _inputLine = new InputLine(
                new(0, maxLines * _ttf.Height),
                _ttf, 
                _target.Width / 8,
                HandleUserInput
            );
            _registry = new CommandRegistry(assembly ?? Assembly.GetCallingAssembly());
            if (motd is not null)
                Motd = motd;
            PushStrings(Motd.Split("\n"));
        }

        public void RefreshConVars()
        {
            _registry.RefreshItems();
        }

        public void Draw(RenderContext context)
        {
            if (_state == State.Hidden)
                return;

            context.RenderTo(_target, () =>
            {
                context.Clear(ColorScheme.Background);

                for (var i = 0; i < _scrollBufferWindow.Count; i++)
                {
                    context.DrawString(
                        _ttf,
                        _scrollBufferWindow[i],
                        new(0, _ttf.Height * i),
                        _scrollBufferWindow[i].Color
                    );
                }

                _inputLine.Draw(context);

                RenderSettings.LineThickness = 2;
                context.Line(
                    new(0, _target.Height - 1),
                    new(_target.Width, _target.Height - 1),
                    ColorScheme.Border
                );
            });

            context.DrawTexture(
                _target,
                _offset,
                Vector2.One,
                Vector2.Zero,
                0
            );
        }

        public void Update(float delta)
        {
            if (_state == State.SlidingDown)
            {
                if (_offset.Y == 0)
                {
                    _state = State.Visible;
                }
                else
                {
                    _offset.Y += SlidingSpeed * delta;

                    if (_offset.Y > 0)
                        _offset.Y = 0;
                }
            }
            else if (_state == State.SlidingUp)
            {
                if (_offset.Y <= -_target.Height)
                {
                    _state = State.Hidden;
                }
                else
                {
                    _offset.Y -= SlidingSpeed * delta;

                    if (_offset.Y <= -_target.Height)
                        _offset.Y = -_target.Height;
                }
            }

            if (_state == State.Hidden)
                return;

            _scrollBufferWindow = _scrollBuffer.GetWindow();
            _inputLine.Update(delta);
        }

        public void KeyPressed(KeyEventArgs e)
        {
            if (_state != State.Visible)
                return;

            _inputLine.KeyPressed(e);
        }

        public void Toggle()
        {
            if (_state == State.Hidden || _state == State.SlidingUp)
            {
                _state = State.SlidingDown;
            }
            else if (_state == State.Visible || _state == State.SlidingDown)
            {
                _state = State.SlidingUp;
            }
        }

        public bool IsOpen() => _state != State.Hidden;

        public void TextInput(TextInputEventArgs e)
        {
            if (_state != State.Visible)
                return;

            _inputLine.TextInput(e);
        }

        public void WheelMoved(MouseWheelEventArgs e)
        {
            if (_state != State.Visible)
                return;

            if (e.Motion.Y > 0)
            {
                _scrollBuffer.ScrollUp();
            }
            else if (e.Motion.Y < 0)
            {
                _scrollBuffer.ScrollDown();
            }
        }

        private void LoadFont()
        {
            using var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Commander.Resources.PxPlus_ToshibaSat_8x14.ttf");

            _ttf = new TrueTypeFont(stream, 16, string.Join("", CodePage.BuildCodePage437Plus()));
        }

        private void HandleUserInput(string input)
        {
            var sb = new StringBuilder();
            var strings = new List<string>();
            
            PushString(input);
            
            PushString(ProcessCommand(input));
        }

        public void PushString(ConsoleLine line)
        {
            if (string.IsNullOrEmpty(line.Line))
                line.Line = string.Empty;
            
            var sb = new StringBuilder();
            var lines = new List<ConsoleLine>();
            
            for (var i = 0; i < line.Line.Length; i++)
            {
                sb.Append(line.Line[i]);
                            
                if (line.Line[i] == '\n' || sb.Length >= _target.Width / 8 || i == line.Line.Length - 1)
                {
                    lines.Add(new ConsoleLine(sb.ToString(), line.Color));
                    sb.Clear();
                }
            }

            foreach (var s in lines)
            {
                _scrollBuffer.Push(s);
            }
        }

        public void PushString(string str) => PushString(new ConsoleLine(str));

        public void PushStrings(string[] strings)
        {
            foreach (var s in strings)
            {
                PushString(s);
            }
        }

        private string ProcessCommand(string input)
        {
            var split = input.Split(' ');

            var args = new object[split.Length - 1];
            for (var i = 1; i < split.Length; i++)
            {
                if (int.TryParse(split[i], out var integer))
                {
                    args[i - 1] = integer;
                }
                else if (float.TryParse(split[i], out var floating))
                {
                    args[i - 1] = floating;
                }
                else
                {
                    args[i - 1] = split[i];
                }
            }

            return _registry.Call(split.First(), args);
        }

        protected override void FreeManagedResources()
        {
            _ttf.Dispose();
        }
    }
}