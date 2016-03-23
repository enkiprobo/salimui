using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SALIMUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HalamanUtama : Page
    {
        CultureInfo kulturIndonesia = new CultureInfo("id-ID");
        DateTime sekarang = DateTime.Now;
        Windows.UI.Xaml.DispatcherTimer jamMenitDetik = new Windows.UI.Xaml.DispatcherTimer();

        PrayTime objPrayTime = new PrayTime();
        String[] waktuSalatHariIni;

        public HalamanUtama()
        {
            this.InitializeComponent();

            jamMenitDetik.Tick += JamMenitDetik_Tick;
            jamMenitDetik.Interval = TimeSpan.FromMinutes(1);
            jamMenitDetik.Start();

            //menentukan penghitungan waktu salat
            waktuSalatHariIni = objPrayTime.getDatePrayerTimes(sekarang.Year,
                sekarang.Month, sekarang.Day, -7.98, 112.62, 7);

            updateWaktuSholat();
            UpdateUITanggalDiHeader();
            UpdateUIBlockWaktuSalat(sekarang, waktuSalatHariIni);
            bacaFile(); // Update UI hadist
        }

        // MeBuSen Event
        private void JamMenitDetik_Tick(object sender, object e)
        {
            sekarang = DateTime.Now;
            JamHariIni.Text = sekarang.ToString("HH:mm");
            updateWaktuSholat();
        }

        // MeBuSen
        void updateWaktuSholat()
        {
            var tempSubuh = buatWaktuBaru(sekarang, waktuSalatHariIni[0]);
            var tempZuhur = buatWaktuBaru(sekarang, waktuSalatHariIni[2]);
            var tempAshar = buatWaktuBaru(sekarang, waktuSalatHariIni[3]);
            var tempMaghrib = buatWaktuBaru(sekarang, waktuSalatHariIni[5]);
            var tempIsha = buatWaktuBaru(sekarang, waktuSalatHariIni[6]);

            int membandingkanWaktudenganSubuh = sekarang.CompareTo(tempSubuh);
            int membandingkanWaktudenganZuhur = sekarang.CompareTo(tempZuhur);
            int membandingkanWaktudenganAshar = sekarang.CompareTo(tempAshar);
            int membandingkanWaktudenganMaghrib = sekarang.CompareTo(tempMaghrib);
            int membandingkanWaktudenganIsha = sekarang.CompareTo(tempIsha);

            if (membandingkanWaktudenganSubuh < 0 || membandingkanWaktudenganIsha > 0)
            {
                NamaSalat.Text = "Subuh";
            }
            else if (membandingkanWaktudenganMaghrib > 0)
            {
                NamaSalat.Text = "Isya";
            }
            else if (membandingkanWaktudenganAshar > 0)
            {
                NamaSalat.Text = "Maghrib";
            }
            else if (membandingkanWaktudenganZuhur > 0)
            {
                NamaSalat.Text = "Ashr";
            }
            else if (membandingkanWaktudenganSubuh > 0)
            {
                NamaSalat.Text = "Zuhur";
            }
        }

        // MeBuSen Event
        private void KalenderPerbulan_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            DateTime tanggalTerpilih = sender.SelectedDates.Select(p => p.Date).FirstOrDefault();
            int timeZoneMachine = TimeZoneInfo.Local.GetUtcOffset(new DateTime((int)tanggalTerpilih.Year, 
                tanggalTerpilih.Month, tanggalTerpilih.Day)).Hours;
            String[] waktuSalatTanggalTerpilih = objPrayTime.getDatePrayerTimes(tanggalTerpilih.Year,
                tanggalTerpilih.Month, tanggalTerpilih.Day, -7.98, 112.62, timeZoneMachine); 
            // latitude(lintang) dan longitude(garis bujur)-nya menggunakan daerah malang (-7.98, 112.62)

            UpdateUIBlockWaktuSalat(tanggalTerpilih, waktuSalatTanggalTerpilih);
        }
        // MeBuSen
        private void UpdateUIBlockWaktuSalat(DateTime tanggal, String[] waktuSalat)
        {
            WaktuSalatUntukHariIni.Text = tanggal.ToString("dd MMMM yyyy", kulturIndonesia);
            WaktuSubuh.Text = waktuSalat[0];
            WaktuDzuhur.Text = waktuSalat[2];
            WaktuAshr.Text = waktuSalat[3];
            WaktuMaghrib.Text = waktuSalat[5];
            WaktuIsya.Text = waktuSalat[6];
        }
        // MeBuSen
        private void UpdateUITanggalDiHeader()
        {
            HariIni.Text = sekarang.ToString("dd MMMM yyyy", kulturIndonesia);
            JamHariIni.Text = sekarang.ToString("HH:mm");
        }
        private async void bacaFile()
        {

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Hadist.txt"));
            using (var inputStream = await file.OpenReadAsync())
            using (var classicStream = inputStream.AsStreamForRead())
            using (var streamReader = new StreamReader(classicStream))
            {
                Random random = new Random((int)DateTime.Now.Ticks);
                string[] satuHadist = streamReader.ReadToEnd().Split('\n');
                HadistNya.Text =  satuHadist[random.Next(0,(satuHadist.Length-1))];
            }
        }
        // MeBuSen convert waktu
        private DateTime buatWaktuBaru(DateTime tanggal, string waktu)
        {
            return Convert.ToDateTime(tanggal.Month + "/" + tanggal.Day + "/" + tanggal.Year + " " + waktu);
        }
    }
}
