using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phidget22;
using Phidget22.Events;

namespace EventManager.ViewModels
{
    public class CampingViewModel : IPageViewModel
    {
        private RFID myRFIDReader;

        public MainViewModel _mainViewModel { get; set; }
        public CampingViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            try
            {
                //myRFIDReader = new RFID();
                //myRFIDReader.Tag += new RFIDTagEventHandler(GetCampingSpot);
               // myRFIDReader.Open();
            }
            catch (PhidgetException) { }
        }
        private void GetCampingSpot(object sender, RFIDTagEventArgs e)
        {
            _mainViewModel.dataModel.SelectedCampingSpot = _mainViewModel.dataHelper.GetCampingSpotByRFID(e.Tag);
            if(_mainViewModel.dataModel.SelectedCampingSpot != null)
            {
                System.Windows.Forms.MessageBox.Show((_mainViewModel.dataModel.SelectedCampingSpot.AmountOfParticpants*20+20).ToString());
            }
        }


    }
}
