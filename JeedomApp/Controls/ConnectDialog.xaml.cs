using Jeedom;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using Windows.Foundation.Metadata;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//TODO: Gérer l'adresse sur le dns jeedom

namespace JeedomApp.Controls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConnectDialog : UserControl
    {

        public ConnectDialog()
        {
            this.InitializeComponent();
        }

        private void Demo_Click(object sender, RoutedEventArgs e)
        {
            // Charger des données de demo pour l'application

            // Masque le dialogue de connection
            ConnectDialog.HideConnectDialog();
        }

        private async void ShowError(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
            return;
        }

        private async void bConnect_Click(object sender, RoutedEventArgs e)
        {
            // Lance le rapatriement des données de Jeedom
            var taskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());

            await taskFactory.StartNew(async () =>
            {
                // Connection à Jeedom
                var error = await RequestViewModel.Instance.ConnectJeedomByLogin();
                if (error != null)
                { ShowError(error.message); }

                // Création du mobile dans le plugin
                error = await RequestViewModel.Instance.CreateEqLogicMobile();
                if (error != null)
                { ShowError(error.message); }

                error = await RequestViewModel.Instance.SearchConfigByKey("jeedom::url", "core");
                if (error != null)
                { ShowError(error.message); }

                RequestViewModel.config.HostExt = RequestViewModel.Instance.configByKey;
                await taskFactory.StartNew(async () =>
                {
                    await RequestViewModel.Instance.FirstLaunch();
                });

                // Masque le dialogue de connection
                ConnectDialog.HideConnectDialog();
            });
        }

        /// <summary>
        /// Affiche le dialogue de Connection
        /// </summary>
        public static void ShowConnectDialog()
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                var view = modal.ModalContent as ConnectDialog;
                if (view == null)
                    modal.ModalContent = view = new ConnectDialog();
                modal.IsModal = true;
                //view.Logo.Begin();
            });
        }

        /// <summary>
        /// Masque le dialogue de Connection
        /// </summary>
        public static void HideConnectDialog()
        {
            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                var modal = Window.Current.Content as ModalDialog;
                if (modal != null)
                    modal.IsModal = false;
            });
        }
       
        BarcodeScanner scanner = null;
        ClaimedBarcodeScanner claimedScanner = null;
        private async Task<bool> CreateDefaultScannerObject()
        {
            if (scanner == null)
            {
                var DeviceSelector = BarcodeScanner.GetDeviceSelector();
                DeviceInformationCollection deviceCollection = await DeviceInformation.FindAllAsync();
                if (deviceCollection != null && deviceCollection.Count > 0)
                {
                    foreach (var device in deviceCollection) { 
                        scanner = await BarcodeScanner.FromIdAsync(device.Id);
                        if (scanner != null)
                            break;
                    }
                    if (scanner == null)
                    {
                        return false;
                    }
                }
                else
                {
                     return false;
                }
            }

            return true;
        }
        private async Task<bool> ClaimScanner()
        {
            if (claimedScanner == null)
            {
                // claim the barcode scanner
                claimedScanner = await scanner.ClaimScannerAsync();

                // enable the claimed barcode scanner
                if (claimedScanner == null)
                {
                     return false;
                }
            }
            return true;
        }
        private async void QrCodeInfo_Click(object sender, RoutedEventArgs e)
        {
          
            // create the barcode scanner. 
            if (await CreateDefaultScannerObject())
            {
                // after successful creation, claim the scanner for exclusive use and enable it so that data reveived events are received.
                if (await ClaimScanner())
                {

                    // It is always a good idea to have a release device requested event handler. If this event is not handled, there are chances of another app can 
                    // claim ownsership of the barcode scanner.
                    claimedScanner.ReleaseDeviceRequested += claimedScanner_ReleaseDeviceRequested;

                    // after successfully claiming, attach the datareceived event handler.
                    claimedScanner.DataReceived += claimedScanner_DataReceived;

                    // Ask the API to decode the data by default. By setting this, API will decode the raw data from the barcode scanner and 
                    // send the ScanDataLabel and ScanDataType in the DataReceived event
                    claimedScanner.IsDecodeDataEnabled = true;

                    // enable the scanner.
                    // Note: If the scanner is not enabled (i.e. EnableAsync not called), attaching the event handler will not be any useful because the API will not fire the event 
                    // if the claimedScanner has not beed Enabled
                    await claimedScanner.EnableAsync();

                    // reset the button state
                    //ScenarioEndScanButton.IsEnabled = true;
                   // ScenarioStartScanButton.IsEnabled = false;

                     }
            }
        }
        async void claimedScanner_ReleaseDeviceRequested(object sender, ClaimedBarcodeScanner e)
        {
            // always retain the device
            e.RetainDevice();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
               });
        }

        string GetDataString(IBuffer data)
        {
            StringBuilder result = new StringBuilder();

            if (data == null)
            {
                result.Append("No data");
            }
            else
            {
                // Just to show that we have the raw data, we'll print the value of the bytes.
                // Arbitrarily limit the number of bytes printed to 20 so the UI isn't overloaded.
                const uint MAX_BYTES_TO_PRINT = 20;
                uint bytesToPrint = Math.Min(data.Length, MAX_BYTES_TO_PRINT);

                DataReader reader = DataReader.FromBuffer(data);
                byte[] dataBytes = new byte[bytesToPrint];
                reader.ReadBytes(dataBytes);

                for (uint byteIndex = 0; byteIndex < bytesToPrint; ++byteIndex)
                {
                    result.AppendFormat("{0:X2} ", dataBytes[byteIndex]);
                }

                if (bytesToPrint < data.Length)
                {
                    result.Append("...");
                }
            }

            return result.ToString();
        }

        string GetDataLabelString(IBuffer data, uint scanDataType)
        {
            string result = null;
            // Only certain data types contain encoded text.
            //   To keep this simple, we'll just decode a few of them.
            if (data == null)
            {
                result = "No data";
            }
            else
            {
                switch (BarcodeSymbologies.GetName(scanDataType))
                {
                    case "Upca":
                    case "UpcaAdd2":
                    case "UpcaAdd5":
                    case "Upce":
                    case "UpceAdd2":
                    case "UpceAdd5":
                    case "Ean8":
                    case "TfStd":
                        // The UPC, EAN8, and 2 of 5 families encode the digits 0..9
                        // which are then sent to the app in a UTF8 string (like "01234")

                        // This is not an exhaustive list of symbologies that can be converted to a string

                        DataReader reader = DataReader.FromBuffer(data);
                        result = reader.ReadString(data.Length);
                        break;
                    default:
                        // Some other symbologies (typically 2-D symbologies) contain binary data that
                        //  should not be converted to text.
                        result = string.Format("Decoded data unavailable. Raw label data: {0}", GetDataString(data));
                        break;
                }
            }

            return result;
        }
        async void claimedScanner_DataReceived(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args)
        {
            // need to update the UI data on the dispatcher thread.
            // update the UI with the data received from the scan.
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // read the data from the buffer and convert to string.
               // ScenarioOutputScanDataLabel.Text = GetDataLabelString(args.Report.ScanDataLabel, args.Report.ScanDataType);

             //   ScenarioOutputScanData.Text = GetDataString(args.Report.ScanData);

              //  ScenarioOutputScanDataType.Text = BarcodeSymbologies.GetName(args.Report.ScanDataType);
            });
        }
        
        private void ResetTheScenarioState()
        {
            if (claimedScanner != null)
            {
                // Detach the event handlers
                claimedScanner.DataReceived -= claimedScanner_DataReceived;
                claimedScanner.ReleaseDeviceRequested -= claimedScanner_ReleaseDeviceRequested;
                // Release the Barcode Scanner and set to null
                claimedScanner.Dispose();
                claimedScanner = null;
            }

            scanner = null;
            
        }
       /* private void ScenarioEndScanButton_Click(object sender, RoutedEventArgs e)
        {
            // reset the scenario state
            this.ResetTheScenarioState();
        }*/
    }
}
