﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class AppsViewModel : IPageViewModel
    {
        public MainViewModel _mainViewModel { get; set; }

        public AppsViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}
