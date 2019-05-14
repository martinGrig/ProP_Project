using EventManager.Objects;
using EventManager.ViewModels.Commands;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class ConverterViewModel : ObservableObject, IPageViewModel
    {
        public MainViewModel _mainViewModel { get; private set; }

        private DateTime dt;

        private string _logFileTitle;
        public string LogFileTitle
        {
            get
            {
                return _logFileTitle;
            }
            set
            {
                _logFileTitle = value;
                OnPropertyChanged("LogFileTitle");
            }
        }

        public ConverterViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _mainViewModel.dataModel._logFileLines = new List<LogLine>();
        }

        #region Commands

        private RelayCommand _loadFileCommand;

        public RelayCommand LoadFileCommand
        {
            get
            {
                if (_loadFileCommand == null) _loadFileCommand = new RelayCommand(new Action<object>(LoadFile));
                return _loadFileCommand;
            }
        }
        #endregion

        private void LoadFile(object obj)
        {
            Regex ibanCheck = new Regex(@"^[a-zA-Z]{2}[0-9]{2}[ ][a-zA-Z]{4}[ ][0-9]{4}[ ][0-9]{4}[ ][0-9]{2}");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream fs = null;
                StreamReader sr = null;
                _mainViewModel.dataModel._logFileLines.Clear();

                try
                {
                    fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs);
                    String s;
                    s = sr.ReadLine();
                    _mainViewModel.dataModel._logFileLines.Add(new LogLine(s, "iban"));
                    s = sr.ReadLine();
                    _mainViewModel.dataModel._logFileLines.Add(new LogLine(s, "start"));
                    s = sr.ReadLine();
                    _mainViewModel.dataModel._logFileLines.Add(new LogLine(s, "end"));
                    s = sr.ReadLine();
                    _mainViewModel.dataModel._logFileLines.Add(new LogLine(s, "amount"));
                    s = sr.ReadLine();
                    while (s != null)
                    {
                        _mainViewModel.dataModel._logFileLines.Add(new LogLine(s, "user"));
                        s = sr.ReadLine();
                    }
                }
                catch (IOException)
                {
                    System.Windows.Forms.MessageBox.Show("something went wrong, IOException was thrown");
                }
                finally
                {
                    if (sr != null) sr.Close();
                    if (fs != null) fs.Close();
                }
                foreach(LogLine line in _mainViewModel.dataModel._logFileLines)
                {
                    Regex regex;
                    switch (line.Type)
                    {
                        case "iban":
                            regex = new Regex(@"^[a-zA-Z]{2}[0-9]{2}[ ][a-zA-Z]{4}[ ][0-9]{4}[ ][0-9]{4}[ ][0-9]{2}");
                            if (regex.Match(line.Line).ToString() == line.Line.Trim())
                            {
                                line.IsEnabled = true;
                            }
                            break;
                        case "start":
                            regex = new Regex(@"^(?:(?:(?:(?:(?:[1-9]\d)(?:0[48]|[2468][048]|[13579][26])|(?:(?:[2468][048]|[13579][26])00))(\/|-|\.)(?:0?2\1(?:29)))|(?:(?:[1-9]\d{3})(\/|-|\.)(?:(?:(?:0?[13578]|1[02])\2(?:31))|(?:(?:0?[13-9]|1[0-2])\2(?:29|30))|(?:(?:0?[1-9])|(?:1[0-2]))\2(?:0?[1-9]|1\d|2[0-8])[/](?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)))))$");
                            if (regex.Match(line.Line).ToString() == line.Line.Trim())
                            {
                                int year = Convert.ToInt32(line.Line.Trim().Substring(0,4));
                                int month = Convert.ToInt32(line.Line.Trim().Substring(5, 2));
                                int day = Convert.ToInt32(line.Line.Trim().Substring(8, 2));
                                dt = new DateTime(year, month, day);
                                LogFileTitle = $"Logfile of {dt.DayOfWeek} the {dt.Day}{GetDaySuffix(day)} of {dt.ToString("MMMM")} {year}";
                                line.IsEnabled = true;
                            }
                            break;
                        case "end":
                            regex = new Regex(@"^(?:(?:(?:(?:(?:[1-9]\d)(?:0[48]|[2468][048]|[13579][26])|(?:(?:[2468][048]|[13579][26])00))(\/|-|\.)(?:0?2\1(?:29)))|(?:(?:[1-9]\d{3})(\/|-|\.)(?:(?:(?:0?[13578]|1[02])\2(?:31))|(?:(?:0?[13-9]|1[0-2])\2(?:29|30))|(?:(?:0?[1-9])|(?:1[0-2]))\2(?:0?[1-9]|1\d|2[0-8])[/](?:(?:([01]?\d|2[0-3]):)?([0-5]?\d):)?([0-5]?\d)))))$");
                            if (regex.Match(line.Line).ToString() == line.Line.Trim())
                            {
                                line.IsEnabled = true;
                            }
                            break;
                        case "amount":
                            regex = new Regex(@"[0-9][0-9]*");
                            if (regex.Match(line.Line).ToString() == line.Line.Trim())
                            {
                                line.IsEnabled = true;
                            }
                            break;
                        case "user":
                            regex = new Regex(@"[0-9]*[ ][0-9]*[.][0-9]{2}");
                            if (regex.Match(line.Line).ToString() == line.Line.Trim())
                            {
                                line.IsEnabled = true;
                            }
                            break;


                    }
                }
                _mainViewModel.dataModel.LogFileLines = _mainViewModel.dataModel._logFileLines;
            }
        }
        private string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }
    }
}
