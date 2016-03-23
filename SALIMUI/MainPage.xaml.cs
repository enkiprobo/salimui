using System;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SALIMUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer pengaturanLokal = ApplicationData.Current.LocalSettings;
        DateTime sekarang = DateTime.Now;
        PrayTime objPrayTime = new PrayTime();
        String[] waktuSalatHariIni;

        public MainPage()
        {
            this.InitializeComponent();

            SetTitleBar();
            // set preffered min window size 
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Windows.Foundation.Size(450, 500));
            // set to halamanutama
            HalamanYangAktif.Navigate(typeof(HalamanUtama));
            MenuListBox.SelectionChanged += MenuListBox_SelectionChanged;
            // waktu salat hari ini
            waktuSalatHariIni = objPrayTime.getDatePrayerTimes(sekarang.Year,
                sekarang.Month, sekarang.Day, -7.98, 112.62, 7);

            createPengaturan();
            updateUIPengaturan();


            hapusNotifikasi(); // nanti hapus ini
            buatDanUpdatePengigatWaktuSalat();
            
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
        // MeBuSen Event handler
        private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MenuListBox.SelectedIndex == 0)
            {
                HalamanYangAktif.Navigate(typeof(HalamanUtama));
            } else if(MenuListBox.SelectedIndex == 1){
                HalamanYangAktif.Navigate(typeof(HalamanRiwayat));
            }
            else if(MenuListBox.SelectedIndex == 2)
            {
                Pengaturan.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }
        // MeBuSen Event handler 
        private void TombolKonfirmasi_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Pengaturan.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
           
            // konfirmasi pengaturan
            updatePengaturan();
            buatDanUpdatePengigatWaktuSalat();
            // kembali ke halaman sebelumnya
            MenuListBox.SelectedIndex = (Type.Equals(HalamanYangAktif.CurrentSourcePageType, typeof(HalamanUtama))) ? 0 : 1;
        }
        // MeBuSen Event handler
        private void HamburgerButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            MenuDiKiri.IsPaneOpen = !MenuDiKiri.IsPaneOpen;
        }
        // MeBuSen membuat pengaturan awalsaat aplikasi diinstall
        private void createPengaturan()
        {
           if (!pengaturanLokal.Values.ContainsKey("pemberitahuan")) {
                // set setting suara untuk awal penginstalan aplikasi
                pengaturanLokal.Values["suara"] = true;
                // set setting pemberithuan untuk awal penginstalan aplikasi
                ApplicationDataCompositeValue pemberitahuan = new ApplicationDataCompositeValue();
                pemberitahuan["subuh"] = true;
                pemberitahuan["dzuhur"] = true;
                pemberitahuan["ashr"] = true;
                pemberitahuan["maghrib"] = true;
                pemberitahuan["isya"] = true;
                pengaturanLokal.Values["pemberitahuan"] = pemberitahuan;
                // set setting durasi waktu salat untuk awal penginstalan aplikasi
                pengaturanLokal.Values["durasiWaktu"] = 10;                
           }
        }
        // MeBuSen mengupdate display pengaturan
        private void updateUIPengaturan()
        {
            // suara
            PengaturanSuara.IsOn = (bool) pengaturanLokal.Values["suara"];
            // pemberitahuan
            ApplicationDataCompositeValue pemberitahuan = (ApplicationDataCompositeValue)pengaturanLokal.
                Values["pemberitahuan"];
            PemberitahuanShubuh.IsOn = (bool) pemberitahuan["subuh"];
            PemberitahuanDzuhur.IsOn = (bool) pemberitahuan["dzuhur"];
            PemberitahuanAshr.IsOn = (bool) pemberitahuan["ashr"];
            PemberitahuanMaghrib.IsOn = (bool) pemberitahuan["maghrib"];
            PemberitahuanIsya.IsOn = (bool) pemberitahuan["isya"];
            // durasi waktu salat
            durasiWaktuSalat.Value = (int) pengaturanLokal.Values["durasiWaktu"];
        }
        // MeBuSen mengupdate pengaturan
        private void updatePengaturan()
        {
            // set setting suara untuk awal penginstalan aplikasi
            pengaturanLokal.Values["suara"] = PengaturanSuara.IsOn;
            // set setting pemberithuan untuk awal penginstalan aplikasi
            ApplicationDataCompositeValue pemberitahuan = (ApplicationDataCompositeValue)pengaturanLokal.
                Values["pemberitahuan"];
            pemberitahuan["subuh"] = PemberitahuanShubuh.IsOn;
            pemberitahuan["dzuhur"] = PemberitahuanDzuhur.IsOn;
            pemberitahuan["ashr"] = PemberitahuanAshr.IsOn;
            pemberitahuan["maghrib"] = PemberitahuanMaghrib.IsOn;
            pemberitahuan["isya"] = PemberitahuanIsya.IsOn;
            pengaturanLokal.Values["pemberitahuan"] = pemberitahuan;
            // set setting durasi waktu salat untuk awal penginstalan aplikasi
            pengaturanLokal.Values["durasiWaktu"] = (int) durasiWaktuSalat.Value;
        }       

        // MeBuSen Event
        private Windows.Data.Xml.Dom.XmlDocument membuatXMLNotifikasi(string namaWaktuSalatNya, 
            bool suara)
        {
            string TOAST = $@"
                        <toast launch=""countdown"">
                          <visual>
                            <binding template=""ToastGeneric"">
                              <text>Salim(Salat In Time)</text>
                              <text>Sudah waktunya salat {namaWaktuSalatNya}. Yuk, Salat dulu</text>
                            </binding>
                          </visual>
                          <actions>
                            <action content = ""Salat"" arguments=""mulaiCountDown""/>
                            <action content = ""Nanti"" arguments=""keluar""/>
                          </actions>
                        </toast>";
            
            Windows.Data.Xml.Dom.XmlDocument xml = new Windows.Data.Xml.Dom.XmlDocument();
            xml.LoadXml(TOAST);

            return xml;
        }
        // MeBuSen
        private void buatNotifikasiTerjadwal(String namaWaktuSalatNya, String waktuSalatnya, bool suara)
        {
            string idNotifikasi = "Ntfsk" + namaWaktuSalatNya;
            if (!apakahNotifikasiSudahTerdaftar(idNotifikasi))
            {
                DateTime when = buatWaktuBaru(DateTime.Now, waktuSalatnya);

                Windows.Data.Xml.Dom.XmlDocument xml = membuatXMLNotifikasi(namaWaktuSalatNya, 
                    suara);

                ScheduledToastNotification toast = new ScheduledToastNotification(xml, when);

                toast.Id = idNotifikasi;
                

                ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
            }
        }
        // MeBuSen
        private bool apakahNotifikasiSudahTerdaftar(string namaNotifikasi)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();
            foreach (var item in scheduled)
            {
                if (item.Id == namaNotifikasi)
                {
                    return true;
                }
            }
            return false;
        }
        // MeBuSen
        private void hapusNotifikasi(string idNotifikasi)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();
            foreach (var item in scheduled)
            {
                if (item.Id == idNotifikasi)
                {
                    notifier.RemoveFromSchedule(item);
                    break;
                }
            }
        }
        // MeBuSen
        private void hapusNotifikasi()
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();
            foreach (var item in scheduled)
            {
               notifier.RemoveFromSchedule(item);         
            }
        }
        // MeBuSen
        private DateTime buatWaktuBaru(DateTime tanggal, string waktu)
        {
            return Convert.ToDateTime(tanggal.Month + "/" + tanggal.Day + "/" + tanggal.Year + " " + waktu);
        }
        // MeBuSen
        private void buatDanUpdatePengigatWaktuSalat()
        {
            ApplicationDataCompositeValue pemberitahuan = (ApplicationDataCompositeValue)pengaturanLokal.
                Values["pemberitahuan"];
            string ntfskSubuh = "Ntfsksubuh", ntfskDzuhur = "Ntfskdzuhur", ntfskAshr = "Ntfskashr",
                ntfskMaghrib = "Ntfskmaghrib", ntfskIsya = "Ntfskisya";

            // buat notifikasi subuh
            if(DateTime.Compare(DateTime.Now, buatWaktuBaru(DateTime.Now, waktuSalatHariIni[0])) < 0)
            {
                if ((bool)pemberitahuan["subuh"])
                {
                    buatNotifikasiTerjadwal("subuh", waktuSalatHariIni[0], (bool)pengaturanLokal.Values["suara"]);
                }
                else
                {
                    hapusNotifikasi(ntfskSubuh);
                }
            }

            // buat notifikasi dzuhur
            if (DateTime.Compare(DateTime.Now, buatWaktuBaru(DateTime.Now, waktuSalatHariIni[2])) < 0)
            {
                if ((bool)pemberitahuan["dzuhur"])
                {
                    if (!apakahNotifikasiSudahTerdaftar(ntfskDzuhur))
                    {
                        buatNotifikasiTerjadwal("dzuhur", waktuSalatHariIni[2], (bool)pengaturanLokal.Values["suara"]);
                    }
                }
                else
                {
                    if (apakahNotifikasiSudahTerdaftar(ntfskDzuhur))
                    {
                        hapusNotifikasi(ntfskDzuhur);
                    }
                }
            }

            // buat notifikasi ashr
            if (DateTime.Compare(DateTime.Now, buatWaktuBaru(DateTime.Now, waktuSalatHariIni[3])) < 0)
            {
                if ((bool)pemberitahuan["ashr"])
                {
                    if (!apakahNotifikasiSudahTerdaftar(ntfskAshr))
                    {
                        buatNotifikasiTerjadwal("ashr", waktuSalatHariIni[3], (bool)pengaturanLokal.Values["suara"]);
                    }
                }
                else
                {
                    if (apakahNotifikasiSudahTerdaftar(ntfskAshr))
                    {
                        hapusNotifikasi(ntfskAshr);
                    }
                }
            }
             
            // buat notifikasi maghrib
            if (DateTime.Compare(DateTime.Now, buatWaktuBaru(DateTime.Now, waktuSalatHariIni[5])) < 0)
            {
                if ((bool)pemberitahuan["maghrib"])
                {
                    if (!apakahNotifikasiSudahTerdaftar(ntfskMaghrib))
                    {
                        buatNotifikasiTerjadwal("maghrib", waktuSalatHariIni[5], (bool)pengaturanLokal.Values["suara"]);
                    }
                }
                else
                {
                    if (apakahNotifikasiSudahTerdaftar(ntfskMaghrib))
                    {
                        hapusNotifikasi(ntfskMaghrib);
                    }
                }
            }
             
            // buat notifikasi isya
            if (DateTime.Compare(DateTime.Now, buatWaktuBaru(DateTime.Now, waktuSalatHariIni[6])) < 0)
            {
                if ((bool)pemberitahuan["isya"])
                {
                    if (!apakahNotifikasiSudahTerdaftar(ntfskIsya))
                    {
                        buatNotifikasiTerjadwal("isya", waktuSalatHariIni[6], (bool)pengaturanLokal.Values["suara"]);
                    }
                }
                else
                {
                    if (apakahNotifikasiSudahTerdaftar(ntfskIsya))
                    {
                        hapusNotifikasi(ntfskIsya);
                    }
                }
            }
        }
    }
}
