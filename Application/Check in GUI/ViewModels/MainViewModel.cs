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

namespace EventManager.ViewModels
{
    

    public class MainViewModel : ObservableObject
    {
        private bool isConnected;
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
            dataModel = new DataModel();
            dataHelper = new DataHelper();
            dataModel.Items = new List<ShopItem>();
            Login = new LoginViewModel(this, dataModel);
            Admin = new AdminViewModel(this);
            Apps = new AppsViewModel(this);
            Camping = new CampingViewModel(this);
            CheckIn = new CheckinViewModel(this);
            CheckOut = new CheckOutViewModel();
            Converter = new ConverterViewModel(this);
            Employee = new EmployeeViewModel(this);
            LoanStand = new LoanStandViewModel();
            Status = new StatusViewModel(this);
            Shop = new ShopViewModel(dataModel);
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
//<<<<<<< HEAD
            
//=======

//            var reader = new Mp3FileReader("C:/Users/David/project-p-phase_group17/Application/Check in GUI/Sounds/ele.mp3");
//            LoopStream loop = new LoopStream(reader);
//            waveOut = new WaveOut();
//            waveOut.Volume = 0.05F;
//            waveOut.Init(loop);
//            waveOut.Play();
//>>>>>>> a8fdf94c36b123b7abb25c99443ac5da29613100

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

