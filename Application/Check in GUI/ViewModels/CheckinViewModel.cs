using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Aztec;
using System.Drawing;
using System.Media;
using System.Windows;
using EventManager.ViewModels.Commands;
using System.Windows.Threading;
using Phidget22;
using Phidget22.Events;
using NAudio.Wave;
using System.Windows.Media.Imaging;
using EventManager.Objects;
using System.Threading;
using System.Net.Mail;

namespace EventManager.ViewModels
{
    public class CheckinViewModel : ObservableObject, IPageViewModel
    {
        WaveOut waveOut;
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer resetTimer = new DispatcherTimer();

        public MainViewModel _mainViewModel { get; set; }

        private string _tempEmail;
        public string TempEmail
        {
            get
            {
                if (_tempEmail == null)
                {
                    return "";
                }
                return _tempEmail;
            }
            set
            {
                _tempEmail = value;
                OnPropertyChanged("TempEmail");
            }
        }

        private BitmapImage _videoImage;
        public BitmapImage VideoImage
        {
            get
            {
                return _videoImage;
            }
            set
            {
                _videoImage = value;
                OnPropertyChanged("VideoImage");
            }
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

        private bool _canSeeDisplay2;
        public bool CanSeeDisplay2
        {
            get
            {
                return _canSeeDisplay2;
            }
            set
            {
                _canSeeDisplay2 = value;
                OnPropertyChanged("CanSeeDisplay2");
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

        public CheckinViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            FinalFrame = new VideoCaptureDevice();
            FinalFrame = new VideoCaptureDevice(CaptureDevice[0].MonikerString);
            QrImageSource = "/Images/burger.ico";
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            resetTimer.Tick += new EventHandler(Reset);
            resetTimer.Interval = new TimeSpan(0, 0, 3);

        }


        private void FinalFrame_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            BitmapImage bi;
            BarcodeReader Reader = new BarcodeReader();
            Result result = Reader.Decode((Bitmap)eventArgs.Frame.Clone());
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            bi = bitmap.ToBitmapImage();
            bi.Freeze();
            VideoImage = bi;
            try
            {
                if (result != null)
                {
                    string decoded = (result.Text);
                    if (decoded != null)
                    {

                        _mainViewModel.dataModel.SelectedVisitor = _mainViewModel.dataHelper.GetVisitor(Convert.ToInt32(decoded));
                        if (_mainViewModel.dataModel.SelectedVisitor != null)
                        {

                            _mainViewModel.dataModel.ShowScanQrCode = true;
                            FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
                            _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(CheckIn);
                            _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(LinkRfid);
                            _mainViewModel._MyRFIDReader.AntennaEnabled = true;
                            Display = new Display(System.Windows.Media.Brushes.Black, "Scan RFID Bracelet to link to visitors account", "", true, false);
                        }
                        else
                        {
                            QrImageSource = "/Images/CheckinUnsuccessful.png";
                            FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
                            dispatcherTimer.Start();
                        }

                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

        private RelayCommand _startQrScanner;
        public RelayCommand StartQrScannerCommand
        {
            get
            {
                if (_startQrScanner == null) _startQrScanner = new RelayCommand(new Action<object>(StartQrScanner));
                return _startQrScanner;
            }
        }

        private RelayCommand _stopQrScanner;
        public RelayCommand StopQrScannerCommand
        {
            get
            {
                if (_stopQrScanner == null) _stopQrScanner = new RelayCommand(new Action<object>(StopQrScanner));
                return _stopQrScanner;
            }
        }

        private RelayCommand _sellTicketCommand;
        public RelayCommand SellTicketCommand
        {
            get
            {
                if (_sellTicketCommand == null) _sellTicketCommand = new RelayCommand(new Action<object>(SellTicket));
                return _sellTicketCommand;
            }
        }

        private RelayCommand _clearVisitorCommand;
        public RelayCommand ClearVisitorCommand
        {
            get
            {
                if (_clearVisitorCommand == null) _clearVisitorCommand = new RelayCommand(new Action<object>(ClearEmployee));
                return _clearVisitorCommand;
            }
        }

        private void ClearEmployee(object obj)
        {
            Reset(null, null);
        }

        private void SellTicket(object obj)
        {
            try
            {
                _mainViewModel.dataModel.SelectedVisitor = _mainViewModel.dataHelper.CreateTemporaryAccount(TempEmail);
                if (_mainViewModel.dataModel.SelectedVisitor != null)
                {

                    SmtpClient client = new SmtpClient();
                    client.Port = 25;
                    client.Host = "smtp.gmail.com";
                    client.EnableSsl = true;
                    client.Timeout = 1000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential("ovelking@gmail.com", "welcome18255");

                    MailMessage mm = new MailMessage("ovelking@gmail.com", $"{TempEmail}", "Temporary MAD Projects Account Creation", $"Welcome visitor To the Board Game event of the Year where you will be anything but Board \n This is your account password: {_mainViewModel.dataHelper.GetVisitorPassword(TempEmail)} for the Event's website http://i415306.hera.fhict.nl/");
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.SendMailAsync(mm);

                    _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(CheckIn);
                    _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(LinkRfid);
                    _mainViewModel._MyRFIDReader.AntennaEnabled = true;
                    QrImageSource = "/Images/CancelOrComplete.png";
                    Display = new Display(System.Windows.Media.Brushes.Black, "Scan RFID Bracelet to link to visitors account", "", true, false);
                    TempEmail = null;

                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        private void StartQrScanner(object obj)
        {
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
            _mainViewModel.dataModel.SelectedVisitor = null;
            TempEmail = null;
            QrImageSource = "/Images/FinalQrCode.png";
            _mainViewModel.dataModel.ShowScanQrCode = false;
            Display = new Display(System.Windows.Media.Brushes.Black, "Scan Or Buy ticket/ Scan RFID Bracelet", "", false, false);
            try
            {
                _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(CheckIn);
                _mainViewModel._MyRFIDReader.Detach += new DetachEventHandler(DetachRfid);
                _mainViewModel._MyRFIDReader.Attach += new AttachEventHandler(AttachRfid);
                _mainViewModel._MyRFIDReader.Open();
                _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            }
            catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(System.Windows.Media.Brushes.Red, "Please connect rfid reader", "", false, false); }
        }

        private void AttachRfid(object sender, AttachEventArgs e)
        {

            CanSeeDisplay2 = false;
            _mainViewModel._MyRFIDReader.AntennaEnabled = true;

        }
        private void DetachRfid(object sender, DetachEventArgs e)
        {
            Display2 = new Display(System.Windows.Media.Brushes.Red, "Please connect rfid reader", "", false, false);
            CanSeeDisplay2 = true;
        }

        public void StopQrScanner(object obj)
        {
            FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
            _mainViewModel._MyRFIDReader.Close();
            FinalFrame.Stop();
        }

        private string _qrImageSource;

        public string QrImageSource
        {
            get
            {
                return _qrImageSource;
            }

            set
            {
                _qrImageSource = value;
                OnPropertyChanged("QrImageSource");
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            // logic to hide your image
            QrImageSource = "/Images/FinalQrCode.png";
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            dispatcherTimer.Stop();
        }

        private void LinkRfid(object sender, RFIDTagEventArgs e)
        {
            if (_mainViewModel.dataModel.SelectedVisitor.RfidCode != null)
            {
                try
                {

                    int check = _mainViewModel.dataHelper.SetRfidTag(e.Tag, _mainViewModel.dataModel.SelectedVisitor.TicketNr);

                    resetTimer.Start();
                    Display = new Display(System.Windows.Media.Brushes.Green, "Checkin Successful, \nLink succesful", "check", false, true);
                    _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(LinkRfid);
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;

                }
                catch (Exception ex)
                {
                    Display = new Display(System.Windows.Media.Brushes.Red, "Scanned RFID Bracelet is already linked to a visitor's, Try again", "times", false, true);
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                    Task.Delay(1000).ContinueWith(_ =>
                    {
                        Display = new Display(System.Windows.Media.Brushes.Black, "Scan RFID Bracelet to link to visitors account", "", true, false);
                        try
                        {
                            _mainViewModel._MyRFIDReader.AntennaEnabled = true;
                        }
                        catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(System.Windows.Media.Brushes.Red, "Please connect rfid reader", "", false, false); }
                    }
    );


                }
                finally
                {
                    _mainViewModel.dataModel.SelectedVisitor = _mainViewModel.dataModel.SelectedVisitor;

                }
            }
            else
            {
                Display = new Display(System.Windows.Media.Brushes.Red, "RFID Bracelet is already linked to visitor", "times", false, true);
                _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(LinkRfid);
                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                resetTimer.Start();
            }


        }

        private void Reset(object sender, EventArgs e)
        {
            _mainViewModel.dataModel.SelectedVisitor = null;
            try
            {
                _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(CheckIn);
                _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(LinkRfid);
                _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            }
            catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(System.Windows.Media.Brushes.Red, "Please connect rfid reader", "", false, false); }
            QrImageSource = "/Images/FinalQrCode.png";
            _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(CheckIn);
            Display = new Display(System.Windows.Media.Brushes.Black, "Scan Or Buy ticket/ Scan RFID Bracelet", "", false, false);
            _mainViewModel.dataModel.ShowScanQrCode = false;
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            resetTimer.Stop();
            if (resetTimer.Interval != new TimeSpan(0, 0, 3))
            {
                resetTimer.Interval = new TimeSpan(0, 0, 3);
            }
        }

        private void CheckIn(object sender, RFIDTagEventArgs e)
        {
            try
            {
                _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(CheckIn);
                Visitor temp = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
                if (temp != null)
                {
                    if (temp.IsScanned == false && temp.IsValid)
                    {
                        //reset
                        _mainViewModel.dataHelper.CheckIn(temp.TicketNr);
                        Display = new Display(System.Windows.Media.Brushes.Green, "Checkin Successful", "check", false, true);
                        _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                        resetTimer.Start();
                    }
                    else
                    {
                        Display = new Display(System.Windows.Media.Brushes.Red, "Checkin Unsuccessful, \nVisitor already Checked In", "times", false, true);
                        resetTimer.Interval = new TimeSpan(0, 0, 1);
                        resetTimer.Start();
                    }
                }
                else
                {
                    Display = new Display(System.Windows.Media.Brushes.Red, "Visitor not recognised, try again", "times", false, true);
                    resetTimer.Interval = new TimeSpan(0, 0, 1);
                    resetTimer.Start();
                }

            }
            catch
            {

            }
        }

    }
}
