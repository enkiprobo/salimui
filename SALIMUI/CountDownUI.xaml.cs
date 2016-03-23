using System;
using System.IO;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SALIMUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CountDownUI : Page
    {
        ApplicationDataContainer pengaturanLokal = ApplicationData.Current.LocalSettings;
        DispatcherTimer countdown = new DispatcherTimer();
        DateTime waktuNya;
        public CountDownUI()
        {
            this.InitializeComponent();
            int menit = (int) pengaturanLokal.Values["durasiWaktu"];
            waktuNya = new DateTime(2011,2,3,0,menit,0);
            SetTitleBar();
            WaktuTersisa.Text = waktuNya.ToString("mm:ss");
            countdown.Tick += Countdown_Tick;
            countdown.Interval = TimeSpan.FromSeconds(1);
            countdown.Start();

            ButtonSelesai.Click += ButtonSelesai_Click;
            ButtonBerhenti.Click += ButtonBerhenti_Click;
        }
        private void ButtonBerhenti_Click(object sender, RoutedEventArgs e)
        {
            countdown.Stop();
            ButtonBerhenti.Visibility = Visibility.Collapsed;
            ButtonSelesai.Visibility = Visibility.Visible;
            TulisanDiHeader.Text = "Anda gagal";

        }

        private void ButtonSelesai_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame = new Frame();
            Window.Current.Content = rootFrame;
            updateDatabaseBerhasilSalat(DateTime.Now);
            rootFrame.Navigate(typeof(MainPage));
        }

        private void Countdown_Tick(object sender, object e)
        {
            waktuNya = waktuNya.AddSeconds(-1);
            WaktuTersisa.Text = waktuNya.ToString("mm:ss");
            if (waktuNya.ToString("mm:ss") == "00:00")
            {
                countdown.Stop();
                ButtonBerhenti.Visibility = Visibility.Collapsed;
                ButtonSelesai.Visibility = Visibility.Visible;
                TulisanDiHeader.Text = "Alhamdulillah";
            }

        }
        // MeBuSen mengubah tampilan title bar
        public void SetTitleBar()
        {
            CoreApplicationViewTitleBar TitleBarInti = CoreApplication.GetCurrentView().TitleBar;
            TitleBarInti.ExtendViewIntoTitleBar = true;
            ApplicationViewTitleBar titleBarSaatIni = ApplicationView.GetForCurrentView().TitleBar;
            titleBarSaatIni.ButtonBackgroundColor = Colors.Transparent;
            titleBarSaatIni.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        public async void updateDatabaseBerhasilSalat(DateTime tanggal)
        {
            string sementara;
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///DatabaseBerhasilSalat.txt"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                sementara = streamReader.ReadToEnd();
            }

            bool ketemu = false;
            string[] seluruhWaktuSalat = sementara.Split('\n');
            int indeks = 0;
            foreach (var item in seluruhWaktuSalat)
            {
                if (item.Split(' ')[0] == tanggal.ToString("MM/dd/yyyy"))
                {
                    ketemu = true;
                    break;
                }
                indeks++;
            }

            if (ketemu)
            {
                seluruhWaktuSalat[indeks].Split(' ')[1] = (Int32.Parse(seluruhWaktuSalat[indeks].Split(' ')[1])+1).
                    ToString();
                string aku = "";
                foreach (var item in seluruhWaktuSalat)
                {
                    aku += item + "\n";
                }
                sementara = "";
                for (int i = 0; i < aku.Length-1; i++)
                {
                    sementara += aku[i];
                }
            }
            else
            {
                sementara += "\n" + tanggal.ToString("MM/dd/yyyy")+" "+"1";
            }
            // File.Create(file.Path).clo
            // File.WriteAllText(file.Path, sementara);
        }
    }
}
