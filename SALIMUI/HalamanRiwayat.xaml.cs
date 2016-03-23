using System;
using System.Globalization;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SALIMUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HalamanRiwayat : Page
    {
        CultureInfo kulturIndonesia = new CultureInfo("id-ID");
        public HalamanRiwayat()
        {
            this.InitializeComponent();

            bacaFile(); // update UI hadist
            updateUIGrafikSalat("03/21/2016");
        }

        private async void ShareFacebookButton_Click(object sender, RoutedEventArgs e)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Hadist.txt"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                string[] satuHadist = streamReader.ReadToEnd().Split('\n');
                
                copyHadistToClipBoard(satuHadist[random.Next(0, (satuHadist.Length - 1))] + "-- via Salim(Salat In Time)");
                showDialogBox(kataPetunjuk("facebook"));
            }
            bukaBrowserKe(@"https://facebook.com");
        }

        private async void ShareTwitterButton_Click(object sender, RoutedEventArgs e)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Hadist.txt"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                string[] satuHadist = streamReader.ReadToEnd().Split('\n');

                copyHadistToClipBoard(satuHadist[random.Next(0, (satuHadist.Length - 1))] + "-- via Salim(Salat In Time)");
                showDialogBox(kataPetunjuk("twitter"));
            }
            bukaBrowserKe(@"https://twitter.com");
        }

        private async void ShareGoogleButton_Click(object sender, RoutedEventArgs e)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Hadist.txt"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                string[] satuHadist = streamReader.ReadToEnd().Split('\n');

                copyHadistToClipBoard(satuHadist[random.Next(0, (satuHadist.Length - 1))] + "-- via Salim(Salat In Time)");
                showDialogBox(kataPetunjuk("google+"));
            }
            bukaBrowserKe(@"https://plus.google.com");
        }
        // MeBuSen
        private async void bukaBrowserKe(string websiteNya)
        {

            // The URI to launch
            var uriAlamatWeb = new Uri(websiteNya);

            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(uriAlamatWeb);
        }
        // MeBuSen update ui grafik
        private async void updateUIGrafikSalat(String tanggal)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///DatabaseBerhasilSalat.txt"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                DateTime input = Convert.ToDateTime(tanggal);
                int delta = DayOfWeek.Monday - ((input.DayOfWeek == DayOfWeek.Sunday)?(DayOfWeek)7:input.DayOfWeek);
                DateTime monday = input.AddDays(delta);
                 
                string[] seluruhWaktuSalat = streamReader.ReadToEnd().Split('\n');
                int indeks = 0;
                foreach (var item in seluruhWaktuSalat)
                {
                    if (item.Split(' ')[0] == tanggal)
                        break;
                    indeks++;
                }
                try
                {
                    GrafikSalatTepatWaktu.tanggalAwal = monday.
                        ToString("dd MMMM yyyy", kulturIndonesia);
                    GrafikSalatTepatWaktu.tanggalAkhir = monday.AddDays(6).
                        ToString("dd MMMM yyyy", kulturIndonesia);
                }
                catch
                {

                }
                
                try
                {
                    GrafikSalatTepatWaktu.jumlahDiHariSenin = Int32.Parse(seluruhWaktuSalat[indeks++].Split(' ')[1]);
                }
                catch
                {
                    GrafikSalatTepatWaktu.jumlahDiHariSenin = 0;
                }
                try
                {
                    GrafikSalatTepatWaktu.jumlahDiHariSelasa = Int32.Parse(seluruhWaktuSalat[indeks++].Split(' ')[1]);
                }
                catch
                {
                    GrafikSalatTepatWaktu.jumlahDiHariSelasa = 0;
                }
                try
                {
                    GrafikSalatTepatWaktu.jumlahDiHariRabu = Int32.Parse(seluruhWaktuSalat[indeks++].Split(' ')[1]);
                }
                catch
                {
                    GrafikSalatTepatWaktu.jumlahDiHariRabu = 0;
                }
                try
                {
                    GrafikSalatTepatWaktu.jumlahDihariKamis = Int32.Parse(seluruhWaktuSalat[indeks++].Split(' ')[1]);
                }
                catch
                {
                    GrafikSalatTepatWaktu.jumlahDihariKamis = 0;
                }
                try
                {
                    GrafikSalatTepatWaktu.jumlahDiHariJumat = Int32.Parse(seluruhWaktuSalat[indeks++].Split(' ')[1]);
                }
                catch
                {
                    GrafikSalatTepatWaktu.jumlahDiHariJumat = 0;
                }
                try
                {
                    GrafikSalatTepatWaktu.jumlahDiHariSabtu = Int32.Parse(seluruhWaktuSalat[indeks++].Split(' ')[1]);
                }
                catch
                {
                    GrafikSalatTepatWaktu.jumlahDiHariSabtu = 0;
                }
                try
                {
                    GrafikSalatTepatWaktu.jumlahDiHariMinggu = Int32.Parse(seluruhWaktuSalat[indeks++].Split(' ')[1]);
                }
                catch
                {
                    GrafikSalatTepatWaktu.jumlahDiHariMinggu = 0;
                }
            }
        }
        // mengambil isi dari Hadist.txt
        private async void bacaFile()
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Hadist.txt"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                string[] satuHadist = streamReader.ReadToEnd().Split('\n');
                HadistNya.Text = satuHadist[random.Next(0, (satuHadist.Length - 1))];
            }
        }
        // MeBuSen
        private async void showDialogBox(string messageNya)
        {
            var dialog = new MessageDialog(messageNya);
            await dialog.ShowAsync();
        }
        // MeBuSen
        private string kataPetunjuk(string webnya)
        {
            return "Silahkan tekan tombol Ctrl+v pada kolom status di media sosial " + webnya + " Anda";
        }
        // MeBuSen
        private void copyHadistToClipBoard(String hadistYangInginDiCopy)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.RequestedOperation = DataPackageOperation.Copy;
            dataPackage.SetText(hadistYangInginDiCopy);

            Clipboard.SetContent(dataPackage);
        }
    }
}
