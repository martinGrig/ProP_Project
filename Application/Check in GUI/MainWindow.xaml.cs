﻿using EventManager.ViewModels;
using SourceWeave.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EventManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SWWindow
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SWWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainViewModel vm = (MainViewModel)DataContext;
            vm.CheckIn.StopQrScannerCommand.Execute(null);
        }
    }
}
