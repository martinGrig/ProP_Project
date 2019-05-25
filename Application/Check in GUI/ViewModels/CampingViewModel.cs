using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using EventManager.Objects;
using EventManager.ViewModels.Commands;
using Phidget22;
using Phidget22.Events;

namespace EventManager.ViewModels
{
    public class CampingViewModel : ObservableObject, IPageViewModel
    {
        private RFID myRFIDReader;

        public MainViewModel _mainViewModel { get; set; }

        private Visitor _visitor;
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

        private Display _display2;
        public Display Display2
        {
            get
            {
                return _display2;
            }
            set
            {
                _display2 = value;
                OnPropertyChanged("Display2");
            }
        }

        private CampingSpot _selectedCampingSpot;
        public CampingSpot SelectedCampingSpot
        {
            get
            {
                return _selectedCampingSpot;
            }
            set
            {
                _selectedCampingSpot = value;
                OnPropertyChanged("SelectedCampingSpot");
            }
        }
        public CampingViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            myRFIDReader = _mainViewModel._MyRFIDReader;
        }
        private void GetCampingSpot(object sender, RFIDTagEventArgs e)
        {
            try
            {
                SelectedCampingSpot = _mainViewModel.dataHelper.GetCampingSpotByRFID(e.Tag);
                if (SelectedCampingSpot != null)
                {
                    myRFIDReader.AntennaEnabled = false;
                    Display2 = new Display(Brushes.Black, $"", "", false, false);
                    if (SelectedCampingSpot.PaymentStatus)
                    {
                        //Start reset timer
                        Display = new Display(Brushes.Green, "Visitor succesfully checked in", "", false, false);
                        _mainViewModel.ResetTimer.Start();
                    }
                    else
                    {
                        _visitor = _mainViewModel.dataHelper.GetVisitor(e.Tag);
                        if (_visitor.Balance >= SelectedCampingSpot.AmountOfParticpants * 20 + 20)
                        {
                            Display = new Display(Brushes.Black, $"Visitor must first pay for Spot \nAmount to be paid:{SelectedCampingSpot.AmountOfParticpants * 20 + 20}", "", false, false);
                        }
                        else
                        {
                            //Start reset timer
                            Display = new Display(Brushes.Red, $"Visitor has insufficient balance", "", false, false);
                            _mainViewModel.ResetTimer.Start();
                        }

                    }
                }
                else
                {
                    //there is no camping spot
                    _visitor = _mainViewModel.dataHelper.GetVisitor(e.Tag);
                    if (_visitor == null)
                    {
                        Display2 = new Display(Brushes.Red, $"There is not a visitor linked to this rfid code", "times", false, true);
                    }
                    else
                    {
                        Display2 = new Display(Brushes.Red, $"Visitor does not have a camping reservation", "times", false, true);
                    }
                    
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public void Start()
        {

            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
            Display2 = new Display(Brushes.Black, $"Scan visitors \nRFID bracelet", "", false, false);
            try
            {
                myRFIDReader.Tag += new RFIDTagEventHandler(GetCampingSpot);

                myRFIDReader.Open();
            }
            catch (PhidgetException) { System.Windows.Forms.MessageBox.Show("Please connect rfid reader"); }
        }

        private RelayCommand _payForCampingSpotCommand;
        public RelayCommand PayForCampingSpotCommand
        {

            get
            {
                if (_payForCampingSpotCommand == null) _payForCampingSpotCommand = new RelayCommand(new Action<object>(PayForCampingSpot));
                return _payForCampingSpotCommand;
            }

        }

        private void PayForCampingSpot(object obj)
        {
            try
            {
                _mainViewModel.dataHelper.PayForCampingSpot(_visitor,SelectedCampingSpot);
                Display = new Display(Brushes.Green, "Visitor succesfully checked in", "", false, false);
                _mainViewModel.ResetTimer.Start();
            }
            catch
            {
                Display = new Display(Brushes.Red, $"Something when wrong with payment", "", false, false);
                _mainViewModel.ResetTimer.Start();
            }
            finally
            {

            }
        }
        

        private void Reset(object sender, EventArgs e)
        {
            Display2 = new Display(Brushes.Black, $"Scan visitors \nRFID bracelet", "", false, false);
            _mainViewModel.ResetTimer.Stop();
            SelectedCampingSpot = null;
            myRFIDReader.AntennaEnabled = true;
        }

    }
}
