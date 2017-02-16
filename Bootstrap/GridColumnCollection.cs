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
using System.Collections;
using System.Collections.Generic;

namespace BWakaBats.Bootstrap
{
    public class GridColumnCollection<TColumn, TRow> : IEnumerable<TColumn>
        where TColumn : GridColumn<TColumn, TRow>, new()
    {
        private IList<TColumn> _items = new List<TColumn>();

        internal GridColumnCollection()
        {
        }

        public TColumn Add(string header)
        {
            var column = new TColumn();
            column.Header(header);
            _items.Add(column);
            return column;
        }

        public IEnumerator<TColumn> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
