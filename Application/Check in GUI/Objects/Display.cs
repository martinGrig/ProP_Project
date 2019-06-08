using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EventManager.Objects
{
    public class Display : ObservableObject
    {
        public Brush Color { get; private set; }
        private string text;
        public string Text { get { return text; } set { text = value; OnPropertyChanged("Text"); } }
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
