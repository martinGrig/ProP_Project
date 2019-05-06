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

using MessagingToolkit.Barcode.QRCode.Decoder;
using MessagingToolkit.Barcode.QRCode;

namespace EventManager.ViewModels
{
    public class CheckinViewModel : ObservableObject, IPageViewModel
    {
        private FilterInfoCollection CaptureDevice;
        private VideoCaptureDevice FinalFrame;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer resetTimer = new DispatcherTimer();
        private RFID myRFIDReader;

        QRCodeDecoder dec = new QRCodeDecoder();

        public MainViewModel _mainViewModel { get; set; }
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
            //BarcodeReader Reader = new BarcodeReader();
            //Result result = Reader.Decode((Bitmap)eventArgs.Frame.Clone());

            try
            {
                string decoded = (dec.Decode((MessagingToolkit.Barcode.BinaryBitmap)eventArgs.Frame.Clone())).ToString();
                if (decoded != null)
                {

                    _mainViewModel.dataModel.SelectedVisitor = _mainViewModel.dataHelper.GetVisitor(Convert.ToInt32(decoded));
                    if (_mainViewModel.dataModel.SelectedVisitor != null)
                    {
                        FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
                        myRFIDReader.Tag += new RFIDTagEventHandler(LinkRfid);
                        myRFIDReader.Open();
                        _mainViewModel.dataModel.ShowScanQrCode = true;
                    }
                    else
                    {
                        MessageBox.Show("This ticket is not valid");
                        QrImageSource = "/Images/CheckinUnsuccessful.png";
                        FinalFrame.NewFrame -= new NewFrameEventHandler(FinalFrame_NewFrame);
                        dispatcherTimer.Start();
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

        private void StartQrScanner(object obj)
        {
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            FinalFrame.Start();
            QrImageSource = "/Images/ScanQrCode.png";
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
            _mainViewModel.dataModel.SelectedVisitor.RfidCode = e.Tag;
            _mainViewModel.dataModel.SelectedVisitor = _mainViewModel.dataModel.SelectedVisitor;
            resetTimer.Start();
            myRFIDReader.Close();
        }

        private void Reset(object sender, EventArgs e)
        {
            _mainViewModel.dataModel.SelectedVisitor = null;
            _mainViewModel.dataModel.ShowScanQrCode = false;
            FinalFrame.NewFrame += new NewFrameEventHandler(FinalFrame_NewFrame);
            resetTimer.Stop();
        }
    }
}
