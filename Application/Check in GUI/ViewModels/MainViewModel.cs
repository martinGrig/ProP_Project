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

namespace EventManager.ViewModels
{
    

    public class MainViewModel : ObservableObject
    {
        public DispatcherTimer ResetTimer { get; private set; }
        private bool isConnected;
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
            Login = new LoginViewModel(this, dataModel);
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
            

            dataModel._jobs = new List<Job>();
            dataModel._jobs.Add(new Job("GateKeeper", "io"));
            dataModel._jobs.Add(new Job("Shop Worker", "sl"));
            dataModel._jobs.Add(new Job("Camping worker", "c"));
            dataModel._jobs.Add(new Job("Manager", "ieosclv"));
            dataModel.Jobs = dataModel._jobs;
            isConnected = true;
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
                dataModel.Shops = dataHelper.GetShops();
                Admin.GetDatabaseStuff();
            }
            if(viewModel.GetType() == typeof(StatusViewModel))
            {
                Status.Start();
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
            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);

            
        }

        private void CheckDatabaseConnection(object sender, EventArgs e)
        {
            bool check = dataHelper.IsServerConnected();
            if (check != isConnected)
            {
                if(check == false)
                {
                    PopUpView popUp = new PopUpView(dataHelper);
                    popUp.Height = 200;
                    popUp.Width = 400;
                    popUp.ShowDialog();
                }
                isConnected = check;
            }
        }
    }
    
}

