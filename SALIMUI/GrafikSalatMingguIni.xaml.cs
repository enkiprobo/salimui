using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SALIMUI
{
    public sealed partial class GrafikSalatMingguIni : UserControl
    {
        public GrafikSalatMingguIni()
        {
            this.InitializeComponent();
        }
        // tanggal awal
        public string tanggalAwal
        {
            get { return (string)GetValue(tanggalAwalProperty); }
            set { SetValue(tanggalAwalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for tanggalAwal.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty tanggalAwalProperty =
            DependencyProperty.Register("tanggalAwal", typeof(string), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        // tanggal akhir
        public string tanggalAkhir
        {
            get { return (string)GetValue(tanggalAkhirProperty); }
            set { SetValue(tanggalAkhirProperty, value); }
        }

        // Using a DependencyProperty as the backing store for tanggalAkhir.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty tanggalAkhirProperty =
            DependencyProperty.Register("tanggalAkhir", typeof(string), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        // jumlah di hari senin
        public int jumlahDiHariSenin
        {
            get { return (int)GetValue(jumlahDiHariSeninProperty); }
            set
            {
                SetValue(jumlahDiHariSeninProperty, pilihanToHeight(value));
            }
        }

        // Using a DependencyProperty as the backing store for jumlahDiHariSenin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty jumlahDiHariSeninProperty =
            DependencyProperty.Register("jumlahDiHariSenin", typeof(int), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        // jumlah di hari selasa 
        public int jumlahDiHariSelasa
        {
            get { return (int)GetValue(jumlahDiHariSelasaProperty); }
            set
            {
                SetValue(jumlahDiHariSelasaProperty, pilihanToHeight(value));
            }
        }

        // Using a DependencyProperty as the backing store for jumlahDiHariSelasa.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty jumlahDiHariSelasaProperty =
            DependencyProperty.Register("jumlahDiHariSelasa", typeof(int), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        // jumlah di hari rabu
        public int jumlahDiHariRabu
        {
            get { return (int)GetValue(jumlahDiHariRabuProperty); }
            set
            {
                SetValue(jumlahDiHariRabuProperty, pilihanToHeight(value));
            }
        }

        // Using a DependencyProperty as the backing store for jumlahDiHariRabu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty jumlahDiHariRabuProperty =
            DependencyProperty.Register("jumlahDiHariRabu", typeof(int), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        // jumlah di hari kamis
        public int jumlahDihariKamis
        {
            get { return (int)GetValue(jumlahDihariKamisProperty); }
            set
            {
                SetValue(jumlahDihariKamisProperty, pilihanToHeight(value));
            }
        }

        // Using a DependencyProperty as the backing store for jumlahDihariKamis.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty jumlahDihariKamisProperty =
            DependencyProperty.Register("jumlahDihariKamis", typeof(int), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        // jumlah di hari jumat
        public int jumlahDiHariJumat
        {
            get { return (int)GetValue(jumlahDiHariJumatProperty); }
            set
            {
                SetValue(jumlahDiHariJumatProperty, pilihanToHeight(value));
            }
        }

        // Using a DependencyProperty as the backing store for jumlahDiHariJumat.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty jumlahDiHariJumatProperty =
            DependencyProperty.Register("jumlahDiHariJumat", typeof(int), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------


        // jumlah di hari sabtu
        public int jumlahDiHariSabtu
        {
            get { return (int)GetValue(jumlahDiHariSabtuProperty); }
            set
            {
                SetValue(jumlahDiHariSabtuProperty, pilihanToHeight(value));
            }
        }

        // Using a DependencyProperty as the backing store for jumlahDiHariSabtu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty jumlahDiHariSabtuProperty =
            DependencyProperty.Register("jumlahDiHariSabtu", typeof(int), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        // jumlah di hari minggu
        public int jumlahDiHariMinggu
        {
            get { return (int)GetValue(jumlahDiHariMingguProperty); }
            set
            {
                SetValue(jumlahDiHariMingguProperty, pilihanToHeight(value));
            }
        }

        // Using a DependencyProperty as the backing store for jumlahDiHariMinggu.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty jumlahDiHariMingguProperty =
            DependencyProperty.Register("jumlahDiHariMinggu", typeof(int), typeof(GrafikSalatMingguIni), null);
        //------------------------------------------------------------------------------------------------------------------------------

        private int pilihanToHeight(int pilihan)
        {
            int tinggiNya;

            switch (pilihan)
            {
                case 1:
                    tinggiNya = 25;
                    break;
                case 2:
                    tinggiNya = 52;
                    break;
                case 3:
                    tinggiNya = 82;
                    break;
                case 4:
                    tinggiNya = 110;
                    break;
                case 5:
                    tinggiNya = 143;
                    break;
                default:
                    tinggiNya = 0;
                    break;
            }
            return tinggiNya;
        }
    }
}
