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
        private RFID myRFIDReader;

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
            myRFIDReader = _mainViewModel._MyRFIDReader;

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
                            //var reader = new Mp3FileReader("C:/Users/David/project-p-phase_group17/Application/Check in GUI/Sounds/ding.mp3");
                            //waveOut = new WaveOut();
                            //waveOut.Volume = 0.8F;
                            //waveOut.Init(reader);
                            //waveOut.Play();
                            myRFIDReader.Tag -= new RFIDTagEventHandler(CheckIn);
                            myRFIDReader.Tag += new RFIDTagEventHandler(LinkRfid);
                            myRFIDReader.AntennaEnabled = true;
                            Display = new Display(System.Windows.Media.Brushes.Black, "Scan RFID Bracelet to link to visitors account", "", true, false);
                        }
                        else
                        {
                            var reader = new Mp3FileReader("C:/Users/David/project-p-phase_group17/Application/Check in GUI/Sounds/fart.mp3");
                            waveOut = new WaveOut();
                            waveOut.Volume = 0.8F;
                            waveOut.Init(reader);
                            waveOut.Play();

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

                    MailMessage mm = new MailMessage("ovelking@gmail.com", $"{TempEmail}", "test", "test");
                    mm.BodyEncoding = UTF8Encoding.UTF8;
                    mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                    client.Send(mm);

                    TempEmail = null;
                    myRFIDReader.Tag -= new RFIDTagEventHandler(CheckIn);
                    myRFIDReader.Tag += new RFIDTagEventHandler(LinkRfid);
                    myRFIDReader.AntennaEnabled = true;
                    QrImageSource = "/Images/waiting.jpg";
                    Display = new Display(System.Windows.Media.Brushes.Black, "Scan RFID Bracelet to link to visitors account", "", true, false);
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
            myRFIDReader.Open();
            QrImageSource = "/Images/ScanQrCode.png";
            Display = new Display(System.Windows.Media.Brushes.Black, "Scan Or Buy ticket/ Scan RFID Bracelet", "", false, false);
            try
            {
                myRFIDReader.Tag += new RFIDTagEventHandler(CheckIn);

                myRFIDReader.Open();
            }
            catch (PhidgetException) { System.Windows.Forms.MessageBox.Show("Please connect rfid reader"); }
        }

        public void StopQrScanner(object obj)
        {
            FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
            myRFIDReader.Close();
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
            QrImageSource = "/Images/ScanQrCode.png";
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            dispatcherTimer.Stop();
        }

        private void LinkRfid(object sender, RFIDTagEventArgs e)
        {
            try
            {
                int check = _mainViewModel.dataHelper.SetRfidTag(e.Tag, _mainViewModel.dataModel.SelectedVisitor.TicketNr);
                if (check == 0)//there is already a visitor linked to this code
                {
                    //What should happen?
                    Display = new Display(System.Windows.Media.Brushes.Red, "Scanned RFID Bracelet is already linked to visitor, Try again", "times", false, true);
                }
                else
                {
                    resetTimer.Start();
                    Display = new Display(System.Windows.Media.Brushes.Green, "Checkin Successful", "check", false, true);
                    myRFIDReader.Tag -= new RFIDTagEventHandler(LinkRfid);
                    myRFIDReader.AntennaEnabled = false;
                }


            }
            catch (Exception ex)
            {
                Display = new Display(System.Windows.Media.Brushes.Red, "Checkin Unsuccessful", "times", false, true);
                myRFIDReader.AntennaEnabled = false;
                Task.Delay(1000).ContinueWith(_ =>
                {
                    Display = new Display(System.Windows.Media.Brushes.Black, "Scan RFID Bracelet to link to visitors account", "", true, false);
                    myRFIDReader.AntennaEnabled = true;
                }
);
                

            }
            finally
            {
                _mainViewModel.dataModel.SelectedVisitor = _mainViewModel.dataModel.SelectedVisitor;
                
            }

        }

        private void Reset(object sender, EventArgs e)
        {
            _mainViewModel.dataModel.SelectedVisitor = null;
            myRFIDReader.AntennaEnabled = true;
            QrImageSource = "/Images/ScanQrCode.png";
            myRFIDReader.Tag += new RFIDTagEventHandler(CheckIn);
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
                myRFIDReader.Tag -= new RFIDTagEventHandler(CheckIn);
                Visitor temp = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
                if (temp != null)
                {
                    if (temp.IsScanned == false && temp.IsValid)
                    {
                        //reset
                        Display = new Display(System.Windows.Media.Brushes.Green, "Checkin Successful", "check", false, true);
                        resetTimer.Start();
                    }
                    else
                    {
                        Display = new Display(System.Windows.Media.Brushes.Red, "Checkin Unsuccessful", "times", false, true);
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
