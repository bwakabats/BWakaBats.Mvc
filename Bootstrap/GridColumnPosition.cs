// *****************************************************
//               MVC EXPANSION - BOOTSTRAP
//                  by  Shane Whitehead
//                  bwakabats@gmail.com
// *****************************************************
//      The software is released under the GNU GPL:
//          http://www.gnu.org/licenses/gpl.txt
//
// Feel free to use, modify and distribute this software
// I only ask you to keep this comment intact.
// Please contact me with bugs, ideas, modification etc.
// *****************************************************

namespace BWakaBats.Bootstrap
{
    public class GridColumnPosition
    {
        public const int Sizes = 4;

        private int[] _width;
        private int[] _offset;
        private bool[] _endOfRow;

        public GridColumnPosition()
        {
            _width = new int[Sizes];
            _offset = new int[Sizes];
            _endOfRow = new bool[Sizes];
        }

        public int Width(int size)
        {
            return _width[size];
        }

        public void Width(int size, int value)
        {
            _width[size] = value;
        }

        public int Offset(int size)
        {
            return _offset[size];
        }

        public void Offset(int size, int value)
        {
            _offset[size] = value;
        }

        public bool EndOfRow(int size)
        {
            return _endOfRow[size];
        }

        public void EndOfRow(int size, bool value)
        {
            _endOfRow[size] = value;
        }
    }
}
