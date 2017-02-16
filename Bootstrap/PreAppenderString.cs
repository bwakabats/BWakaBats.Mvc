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
    class PreAppenderString : IPreAppender
    {
        internal string Value { get; set; }

        internal PreAppenderString(string value)
        {
            Value = value;
        }

        public virtual string ToPreAppenderString()
        {
            return "<span class='input-group-addon'>" + Value + "</span>";
        }
    }
}
