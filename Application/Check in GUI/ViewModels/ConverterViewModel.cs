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
    public class ConverterViewModel : IPageViewModel
    {
        public MainViewModel _mainViewModel { get; private set; }

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
                
                _mainViewModel.dataModel.LogFileLines = _mainViewModel.dataModel._logFileLines;
            }
        }
    }
}
