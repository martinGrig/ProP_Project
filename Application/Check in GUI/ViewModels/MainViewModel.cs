using EventManager.Models;
using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NAudio.Wave;
using EventManager.Objects;
using System.Windows.Threading;
using EventManager.Views;
using Phidget22;
using Phidget22.Events;
using System.Windows.Media;

namespace EventManager.ViewModels
{
    

    public class MainViewModel : ObservableObject
    {



        DispatcherTimer timer;
        int attemptCount;
        int timeTillRetry;
        public DispatcherTimer ResetTimer { get; private set; }
        public bool isConnected {  get; set; }
        public RFID _MyRFIDReader;
        private WaveOut waveOut;
        private DispatcherTimer databaseChecker;
        public LoginViewModel Login { get; private set; }
        public AdminViewModel Admin { get; private set; }
        public AppsViewModel Apps { get; private set; }
        public CampingViewModel Camping { get; private set; }
        public CheckinViewModel CheckIn { get; private set; }
        public CheckOutViewModel CheckOut { get; private set; }
        public ConverterViewModel Converter { get; private set; }
        public EmployeeViewModel Employee { get; private set; }
        public LoanStandViewModel LoanStand { get; private set; }
        public StatusViewModel Status { get; private set; }
        public ShopViewModel Shop { get; set; }

        public DataModel dataModel { get; set; }
        public DataHelper dataHelper;

        private RelayCommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;


        public MainViewModel()
        {
            ResetTimer = new DispatcherTimer();
            _MyRFIDReader = new RFID();
            dataModel = new DataModel();
            dataHelper = new DataHelper();
            dataModel.Items = new List<ShopItem>();
            Login = new LoginViewModel(this);
            Admin = new AdminViewModel(this);
            Apps = new AppsViewModel(this);
            Camping = new CampingViewModel(this);
            CheckIn = new CheckinViewModel(this);
            CheckOut = new CheckOutViewModel(this);
            Converter = new ConverterViewModel(this);
            Employee = new EmployeeViewModel(this);
            LoanStand = new LoanStandViewModel(this);
            Status = new StatusViewModel(this);
            Shop = new ShopViewModel(this);
            PageViewModels.Add(Login);
            CurrentPageViewModel = _pageViewModels[0];
            databaseChecker = new DispatcherTimer();
            databaseChecker.Interval = new TimeSpan(0, 0, 10);
            databaseChecker.Tick += new EventHandler(CheckDatabaseConnection);
            databaseChecker.IsEnabled = true;
            databaseChecker.Start();

            timeTillRetry = 10;
            attemptCount = 0;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(UpdateTime);
            Display = new Display(Brushes.Black, "0", "", false, false);
            dataModel._jobs = new List<Job>();
            dataModel.Jobs = dataModel._jobs;
            isConnected = true;
        }

        private Display _display;
        public Display Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                OnPropertyChanged("Display");
            }
        }
        private bool _canSeeDisplay;
        public bool CanSeeDisplay
        {
            get
            {
                return _canSeeDisplay;
            }
            set
            {
                _canSeeDisplay = value;
                OnPropertyChanged("CanSeeDisplay");
            }
        }
        public RelayCommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }
                
                return _changePageCommand;
            }
        }

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }
        

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            ResetTimer.Stop();
            ResetTimer = new DispatcherTimer();
            _MyRFIDReader.Close();
            _MyRFIDReader = new RFID();

            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);
            if (viewModel.GetType() == typeof(CheckinViewModel))
            {
                CheckIn.StartQrScannerCommand.Execute(null);
            }
            else
            {
                CheckIn.StopQrScannerCommand.Execute(null);
            }
            if(viewModel.GetType() == typeof(AdminViewModel))
            {
                Admin.GetDatabaseStuff();
            }
            if(viewModel.GetType() == typeof(StatusViewModel))
            {
                Status.Start();
            }
            if (viewModel.GetType() == typeof(ShopViewModel))
            {
                Shop.Start();
            }
            if (viewModel.GetType() == typeof(LoanStandViewModel))
            {
                LoanStand.Start();
            }
            if (viewModel.GetType() == typeof(EmployeeViewModel))
            {
                Employee.Start();
            }
            if (viewModel.GetType() == typeof(CampingViewModel))
            {
                Camping.Start();
            }
            if (viewModel.GetType() == typeof(CheckOutViewModel))
            {
                CheckOut.Start();
            }
            if (viewModel.GetType() == typeof(ConverterViewModel))
            {
                Converter.Start();
            }
            if (viewModel.GetType() == typeof(LoginViewModel))
            {
                dataModel = new DataModel();
                Login.Start();
            }
            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);

            
        }



        private void CheckDatabaseConnection(object sender, EventArgs e)
        {
            bool check = dataHelper.IsServerConnected();
            if (check == false)
            {
                databaseChecker.Stop();
                Display = new Display(Brushes.Red, "Conection attempt fail", "", false, false);
                Display = Display;
            }
            else
            {
                Display = new Display(Brushes.Green, "Conection succesfull", "", false, false);
                Display = Display;
            }
            if (check != isConnected)
            {
                if(check == false)
                {
                    //Show display
                    databaseChecker.Stop();
                    timer.Start();
                    Display = new Display(Brushes.Red, "Conection attempt fail", "", false, false);
                    Display = Display;
                    CanSeeDisplay = true;
                }
                else
                {
                    Display = new Display(Brushes.Green, "Conection succesfull", "", false, false);
                    Display = Display;
                    CanSeeDisplay = false;
                    timer.Stop();
                    databaseChecker.Start();
                    timeTillRetry = 10;
                }
                isConnected = check;
            }
            
        }

            
          

        private void UpdateTime(object sender, EventArgs e)
        {
            timeTillRetry -= 1;
            if (timeTillRetry <= 0)
            {
                CheckDatabaseConnection(null, null);
                if (timeTillRetry == -1)
                {
                    if (attemptCount == 0)
                    {
                        timeTillRetry = 10;
                    }
                    else if (attemptCount < 4)
                    {
                        timeTillRetry = 30;
                    }
                    else
                    {
                        timeTillRetry = 60;
                    }
                    attemptCount++;
                    Display.Text = $"{timeTillRetry.ToString()}";
                    Display = Display;

                }

            }
            else
            {
                Display.Text = $"{timeTillRetry.ToString()}";
                Display = Display;
            }

        }

        private RelayCommand _retryConnectionCommand;
        public RelayCommand RetryConnectionCommand
        {
            get
            {
                if (_retryConnectionCommand == null)
                {
                    _retryConnectionCommand = new RelayCommand(new Action<object>(RetryConnection));
                }
                return _retryConnectionCommand;
            }
        }

        private void RetryConnection(object obj)
        {
            CheckDatabaseConnection(null, null);
        }

    }
    
}

