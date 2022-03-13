using System;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;

namespace Chroma.Commander;

internal class InputLine
{
    internal Vector2 Position;
    private TrueTypeFont _ttf;
    private Action<string> _inputHandler;

    private int _currentCol;
    private int _maxCols;
    private int _margin;

    private string _input = string.Empty;
    private string _inputVisual = string.Empty;

    private int _currentIndex;

    private int _blinkClock;
    private bool _showCursor;

    public InputLine(Vector2 position, TrueTypeFont ttf, int maxCols, Action<string> inputHandler)
    {
        Position = position;
        _ttf = ttf;

        _inputHandler = inputHandler;
        _maxCols = maxCols;
    }

    public void Update(float delta)
    {
        if (_blinkClock > 500)
        {
            _blinkClock = 0;
            _showCursor = !_showCursor;
        }
        else
        {
            _blinkClock += (int)(1000 * delta);
        }

        _inputVisual = _input.Substring(_margin, _input.Length - _margin);
    }

    public void Draw(RenderContext context)
    {
        context.DrawString(
            _ttf,
            _inputVisual,
            Position,
            Color.White
        );

        if (_showCursor)
        {
            RenderSettings.ShapeBlendingEnabled = true;
                
            RenderSettings.SetShapeBlendingEquations(
                BlendingEquation.Subtract,
                BlendingEquation.Add
            );
                
            RenderSettings.SetShapeBlendingFunctions(
                BlendingFunction.SourceColor,
                BlendingFunction.OneMinusDestinationColor,
                BlendingFunction.DestinationColor,
                BlendingFunction.DestinationAlpha
            );
                
            context.Rectangle(
                ShapeMode.Fill,
                Position + new Vector2(_currentCol * 8, 0),
                8, 16, Color.White
            );
                
            RenderSettings.ResetShapeBlending();
        }
    }

    public void TextInput(TextInputEventArgs e)
    {
        _input = _input.Insert(_currentIndex++, e.Text);

        if (_currentCol + 1 >= _maxCols)
        {
            _margin++;
        }
        else
        {
            _currentCol++;
        }
    }

    public void KeyPressed(KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case KeyCode.Return:
            case KeyCode.NumEnter:
            {
                _inputHandler?.Invoke(_input);
                _input = string.Empty;

                _currentIndex = 0;
                _currentCol = 0;
                _margin = 0;
                break;
            }

            case KeyCode.Backspace:
            {
                if (_currentIndex == 0)
                    return;

                _input = _input.Substring(0, _currentIndex - 1) +
                         _input.Substring(_currentIndex, _input.Length - _currentIndex);

                _currentIndex--;

                if (_currentCol > 0)
                {
                    _currentCol--;
                }
                else
                {
                    _margin--;
                }
                break;
            }
                
            case KeyCode.Left:
            {
                if (_currentCol <= 0)
                    return;

                _currentIndex--;

                if (_currentCol == 0)
                {
                    _margin--;
                }
                else
                {
                    _currentCol--;
                }
                break;
            }
                
            case KeyCode.Right:
            {
                if (_currentIndex >= _input.Length)
                    return;

                _currentIndex++;

                if (_currentCol + 1 >= _maxCols)
                {
                    _margin++;
                }
                else
                {
                    _currentCol++;
                }
                break;
            }
                
            case KeyCode.End:
            {
                _currentIndex = _input.Length;

                if (_input.Length >= _maxCols)
                {
                    _currentCol = _maxCols - 1;
                    _margin = _input.Length - _maxCols + 1;
                }
                else
                {
                    _currentCol = _input.Length;
                }
                break;
            }
                
            case KeyCode.Home:
            {
                _currentIndex = 0;
                _currentCol = 0;
                _margin = 0;
                break;
            }
                
            case KeyCode.Delete:
            {
                if (_currentIndex >= _input.Length)
                    return;

                _input = _input.Remove(_currentIndex, 1);
                break;
            }
        }
    }
}