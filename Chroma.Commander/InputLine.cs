using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;

namespace Chroma.Commander;

internal class InputLine
{
    internal Vector2 Position;

    private ICommandRegistry _registry;
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
    
    private Stack<string> _historyBuffer;
    private int _historyPointer;
    private int _autocompletePointer;
    private string _previousInput = string.Empty;

    public InputLine(ICommandRegistry registry, Vector2 position, TrueTypeFont ttf, int maxCols, Action<string> inputHandler, int historySize)
    {
        Position = position;
        _registry = registry;
        _ttf = ttf;

        _inputHandler = inputHandler;
        _maxCols = maxCols;
        _historyBuffer = new(historySize);
        ResetHistory(true);
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
        ResetHistory(true);
    }

    public void KeyPressed(KeyEventArgs e)
    {
        switch (e.KeyCode)
        {
            case KeyCode.Return:
            case KeyCode.NumEnter:
            {
                _inputHandler?.Invoke(_input);
                _historyBuffer.Push(_input);
                _input = string.Empty;
                ResetHistory(true);

                _currentIndex = 0;
                _currentCol = 0;
                _margin = 0;
                break;
            }

            case KeyCode.Backspace:
            {
                if (_currentIndex == 0)
                {
                    ResetHistory(true);
                    return;
                }

                _input = _input.Substring(0, _currentIndex - 1) +
                         _input.Substring(_currentIndex, _input.Length - _currentIndex);
                ResetHistory(true);

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
            
            case KeyCode.Tab:
                var next = _autocompletePointer + ((e.Modifiers & KeyModifiers.Shift) != 0 ? -1 : 1);
                if(next < 0)
                {
                    _autocompletePointer = -1;
                    SetInput(string.Empty);
                    return;
                }

                var fill = _registry.GetAutoComplete(_previousInput, next);
                if (fill is null)
                    return;

                _autocompletePointer = next;
                SetInput(fill);
                break;
                
            case KeyCode.Left:
            {
                ResetHistory();
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
                ResetHistory();
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

            case KeyCode.Up:
            {
                ScrollHistory(true);
                break;
            }

            case KeyCode.Down:
            {
                ScrollHistory(false);
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

    private void ResetHistory(bool force = false)
    {
        _historyPointer = -1;
        _autocompletePointer = -1;
        if(force || !string.IsNullOrWhiteSpace(_input))
            _previousInput = _input;
    }

    private void ScrollHistory(bool up)
    {
        if (!up && _historyPointer <= 0)
        {
            // Set it to previous input so if you clear, then press up again
            // It uses your previous filter until another key is pressed
            _historyPointer = -1;
            SetInput(_previousInput);
            return;
        }
        
        var filteredStack = _historyBuffer.Where(h => h.StartsWith(_previousInput));
        var newPointer = _historyPointer + (up ? 1 : -1);
        if (filteredStack.Count() - 1 >= newPointer)
        {
            _historyPointer = newPointer;
            SetInput(filteredStack.ElementAt(_historyPointer));
        }
    }

    internal void SetInput(string input)
    {
        _input = input;
        _currentIndex = _input.Length;
                    
        _currentCol = Math.Min(_currentIndex, _maxCols);
        if (_currentCol + 1 >= _maxCols)
        {
            _currentCol--;
            _margin = _input.Length - _maxCols + 1;
        }
    }
}