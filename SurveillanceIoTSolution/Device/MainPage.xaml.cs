using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Restup.Webserver.Attributes;
using Restup.Webserver.Http;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;
using Restup.Webserver.Rest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Device
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        static DeviceClient deviceClient;
        static ServiceClient serviceClient;
        static string iotHubUri = "<IoTHubUrl>";
        static string deviceKey = "<Devicekey>";
        static string DeviceId = "<DeviceId>";

        private const int RED_LED_PIN = 6;
        private const int RED_BUTTON_PIN = 5;
        private const int GREEN_LED_PIN = 13;
        private const int GREEN_BUTTON_PIN = 26;
        private GpioPin redLedPin;
        private GpioPin redButtonPin;
        private GpioPinValue redLedPinValue = GpioPinValue.High;
        private GpioPin greenLedPin;
        private GpioPin greenButtonPin;
        private GpioPinValue greenLedPinValue = GpioPinValue.High;
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        public MainPage()
        {
            this.InitializeComponent();
            InitGPIO();
            DataContext = new MainViewModel();
            Run();
        }

        public async void Run()
        {
            try
            {
                var restRouteHandler = new RestRouteHandler();
                restRouteHandler.RegisterController<ParameterController>();

                var configuration = new HttpServerConfiguration()
                  .ListenOnPort(8800)
                  .RegisterRoute("api", restRouteHandler)
                  .EnableCors();

                var httpServer = new HttpServer(configuration);
                await httpServer.StartServerAsync();
            }
            catch (Exception ex)
            {

            }

            // now make sure the app won't stop after this (eg use a BackgroundTaskDeferral)
        }

        private static async void SendDeviceToCloudMessagesAsync(bool alarm)
        {
            deviceClient = DeviceClient.Create(iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, deviceKey), Microsoft.Azure.Devices.Client.TransportType.Http1);
            deviceClient.ProductInfo = "Surveillnace solution";

            var alertMessage = new
            {
                messageId = Guid.NewGuid(),
                alertTime = DateTime.Now,
                alarm
            };
            var messageString = JsonConvert.SerializeObject(alertMessage);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.ASCII.GetBytes(messageString));
            //message.Properties.Add("temperatureAlert", (currentTemperature > 30) ? "true" : "false");

            await deviceClient.SendEventAsync(message);
        }

        private async static void ReceiveFeedbackAsync()
        {
            serviceClient = ServiceClient.CreateFromConnectionString(iotHubUri);
            var feedbackReceiver = serviceClient.GetFeedbackReceiver();

            Console.WriteLine("\nReceiving c2d feedback from service");
            while (true)
            {
                var feedbackBatch = await feedbackReceiver.ReceiveAsync();
                if (feedbackBatch == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Received feedback: {0}", string.Join(", ", feedbackBatch.Records.Select(f => f.StatusCode)));
                Console.ResetColor();

                await feedbackReceiver.CompleteAsync(feedbackBatch);
            }
        }

        private void InitGPIO()
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            redButtonPin = gpio.OpenPin(RED_BUTTON_PIN);
            redLedPin = gpio.OpenPin(RED_LED_PIN);
            greenButtonPin = gpio.OpenPin(GREEN_BUTTON_PIN);
            greenLedPin = gpio.OpenPin(GREEN_LED_PIN);

            // Initialize LED to the OFF state by first writing a HIGH value
            // We write HIGH because the LED is wired in a active LOW configuration
            redLedPin.Write(GpioPinValue.High);
            redLedPin.SetDriveMode(GpioPinDriveMode.Output);
            greenLedPin.Write(GpioPinValue.High);
            greenLedPin.SetDriveMode(GpioPinDriveMode.Output);

            // Check if input pull-up resistors are supported
            if (redButtonPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                redButtonPin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                redButtonPin.SetDriveMode(GpioPinDriveMode.Input);

            if (greenButtonPin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                greenButtonPin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                greenButtonPin.SetDriveMode(GpioPinDriveMode.Input);

            // Set a debounce timeout to filter out switch bounce noise from a button press
            redButtonPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            greenButtonPin.DebounceTimeout = TimeSpan.FromMilliseconds(50);

            // Register for the ValueChanged event so our buttonPin_ValueChanged 
            // function is called when the button is pressed
            redButtonPin.ValueChanged += buttonPin_ValueChanged;
            greenButtonPin.ValueChanged += GreenButtonPin_ValueChanged;

            GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        private void GreenButtonPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            if (e.Edge == GpioPinEdge.FallingEdge)
            {
                greenLedPinValue = (greenLedPinValue == GpioPinValue.Low) ?
                    GpioPinValue.High : GpioPinValue.Low;
                greenLedPin.Write(greenLedPinValue);
                SendDeviceToCloudMessagesAsync(false);
            }
        }

        private void buttonPin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            // toggle the state of the LED every time the button is pressed
            if (e.Edge == GpioPinEdge.FallingEdge)
            {
                redLedPinValue = (redLedPinValue == GpioPinValue.Low) ?
                    GpioPinValue.High : GpioPinValue.Low;
                redLedPin.Write(redLedPinValue);
                SendDeviceToCloudMessagesAsync(true);
            }

            // need to invoke UI updates on the UI thread because this event
            // handler gets invoked on a separate thread.
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (e.Edge == GpioPinEdge.FallingEdge)
                {
                    ledEllipse.Fill = (redLedPinValue == GpioPinValue.Low) ?
                        redBrush : grayBrush;
                    GpioStatus.Text = "Button Pressed";
                }
                else
                {
                    GpioStatus.Text = "Button Released";
                }
            });
        }

        private void SendTestBtn_Click(object sender, RoutedEventArgs e)
        {
            SendDeviceToCloudMessagesAsync(true);
        }
    }

    public class DataReceived
    {
        public DateTime Time { get; set; }
        public string Code { get; set; }
    }

    [RestController(InstanceCreationType.Singleton)]
    public class ParameterController
    {
        // http://192.168.0.11:8800/api/somestring
        [UriFormat("/{guid}")]
        public IGetResponse GetWithSimpleParameters(string guid)
        {
            Debug.Write(guid);
            return new GetResponse(
              GetResponse.ResponseStatus.OK,
              new DataReceived() { Time = DateTime.Now, Code = guid });
        }
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        private string _resDateTime;
        public string ResDateTime
        {
            get
            {
                return _resDateTime;
            }
            set
            {
                _resDateTime = value;
                NotifyPropertyChanged("ResDateTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            _timer.Tick += TimerOnTick;
            _timer.Start();
        }

        private void TimerOnTick(object sender, object o)
        {
            ResDateTime = DateTime.Now.ToString("dd MMM yyyy HH:mm:ss");
        }
    }
}
