using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EventManager.ViewModels
{
    public class MainViewModel : ObservableObject
    {
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
        public ShopViewModel Shop { get; private set; }
        //public RelayCommand GoToLoginCommand { get; private set; }
        //public RelayCommand GoToAdminCommand { get; private set; }


        private RelayCommand _changePageCommand;

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;


        public MainViewModel()
        {
            Login = new LoginViewModel(this);
            Admin = new AdminViewModel();
            Apps = new AppsViewModel();
            Camping = new CampingViewModel();
            CheckIn = new CheckinViewModel();
            CheckOut = new CheckOutViewModel();
            Converter = new ConverterViewModel();
            Employee = new EmployeeViewModel();
            LoanStand = new LoanStandViewModel();
            Status = new StatusViewModel();
            Shop = new ShopViewModel();

            PageViewModels.Add(Login);
            CurrentPageViewModel = _pageViewModels[0];

            //GoToLoginCommand = new RelayCommand(LoginScreen);
            //GoToAdminCommand = new RelayCommand(AdminScreen);
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
            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

    }

    //private object _currentView;

    //public object CurrentView
    //{
    //    get { return _currentView; }
    //    set
    //    {
    //        _currentView = value; this.OnPropertyChanged("CurrentView");
    //    }
    //}

    //public void LoginScreen(object obj)
    //{
    //    CurrentView = Login;
    //}
    //public void AdminScreen(object obj)
    //{
    //    CurrentView = Admin;
    //}
}

