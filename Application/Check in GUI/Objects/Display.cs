using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EventManager.Objects
{
    public class Display
    {
        public Brush Color { get; private set; }
        public string Text { get; private set; }
        public string Icon { get; private set; }
        public bool ShowImage { get; private set; }
        public bool ShowIcon { get; private set; }

        public Display(Brush _color, string _text, string _icon, bool showBracelet, bool showIcon)
        {
            Color = _color;
            Text = _text;
            Icon = _icon;
            ShowImage = showBracelet;
            ShowIcon = showIcon;
        }
    }
}
