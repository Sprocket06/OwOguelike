using System.Collections.Generic;
using System.Linq;

namespace Chroma.Commander
{
    internal class ScrollBuffer
    {
        private int _window;
        private int _bottom;

        private List<string> _lines = new();

        public ScrollBuffer(int window = 20)
        {
            _window = window;
            _bottom = 0;
        }

        public void Push(string line)
        {
            _lines.Add(line);

            if (_lines.Count > _window && _bottom == _lines.Count - _window - 1)
                _bottom++;
        }

        public void ScrollUp()
        {
            if (_bottom - 1 >= 0)
                _bottom--;
        }

        public void ScrollDown()
        {
            if (_bottom + 1 <= _lines.Count - _window)
                _bottom++;
        }

        public void ScrollToEnd()
        {
            if (_lines.Count - _window <= 0)
                return;
            
            _bottom = _lines.Count - _window - 1;
        }

        public List<string> GetWindow()
        {
            var lines = _lines.Skip(_bottom).Take(_window).ToList();

            while (lines.Count != _window)
                lines.Insert(0, string.Empty);

            return lines;
        }
    }
}