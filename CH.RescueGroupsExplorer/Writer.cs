using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CH.RescueGroupsExplorer
{
    public class Writer
    {
        private TextBox _txt;

        public Writer(TextBox txtBox)
        {
            this._txt = txtBox;
        }

        public void Write(string text)
        {
            if (text != null)
            {
                _txt.AppendText(text);
            }
        }

        public void WriteLine(string text = null)
        {
            Write(text);
            _txt.AppendText(Environment.NewLine);
        }
    }
}
