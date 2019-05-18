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
using System.Windows;
using EventManager.ViewModels.Commands;
using System.Windows.Threading;
using Phidget22;
using Phidget22.Events;
using NAudio.Wave;
using System.Windows.Media.Imaging;

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

            try
            {
                myRFIDReader = new RFID();
            }
            catch (PhidgetException) { }
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
                            myRFIDReader.Tag += new RFIDTagEventHandler(LinkRfid);
                            myRFIDReader.AntennaEnabled = true;
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
                    TempEmail = null;
                    myRFIDReader.Tag += new RFIDTagEventHandler(LinkRfid);
                    myRFIDReader.AntennaEnabled = true;
                    QrImageSource = "/Images/waiting.jpg";
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
        }

        private void StopQrScanner(object obj)
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
                    _mainViewModel.dataModel.SelectedVisitor.RfidCode = "2";
                }
                else
                {
                    resetTimer.Start();
                    _mainViewModel.dataModel.SelectedVisitor.RfidCode = "6";
                    myRFIDReader.AntennaEnabled = false;
                }

            }
            catch (Exception ex)
            {
                _mainViewModel.dataModel.SelectedVisitor.RfidCode = "1";

            }
            finally
            {
                _mainViewModel.dataModel.SelectedVisitor = _mainViewModel.dataModel.SelectedVisitor;
            }

        }

        private void Reset(object sender, EventArgs e)
        {
            _mainViewModel.dataModel.SelectedVisitor = null;
            QrImageSource = "/Images/ScanQrCode.png";
            _mainViewModel.dataModel.ShowScanQrCode = false;
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            resetTimer.Stop();
        }

    }
}
