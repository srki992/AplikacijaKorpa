using Aplikacija.Commands;
using Aplikacija.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged, IMainViewModel
    {
        IPodaciService podaciService;
        public MainViewModel(IPodaciService podaciService)
        {
            this.podaciService = podaciService;
            _DodajUKorpuCommand = new RelayCommand(DodajUKorpu);
            _OduzmiIzKorpeCommand = new RelayCommand(OduzmiIzKorpe);
            _ZavrsiKupovinuCommand = new RelayCommand(ZavrsiKupovinu);
            _IzracunajCenuCommand = new RelayCommand(IzracunajCenuZavrsniRacun);
            LoadData();
        }



        #region Metode
        private void IzracunajCenuZavrsniRacun()
        {
            var PuterPunaCenaNaRacunu = (from lista in ListaStavkiUKorpi
                                         where lista.ime == "Puter"
                                         select lista.kolicina * lista.cena).Sum();
            var HlebPunaCenaNaRacunu = (from lista in ListaStavkiUKorpi
                                        where lista.ime == "Hleb"
                                        select lista.kolicina * lista.cena).Sum();
            var MlekoPunaCenaNaRacunu = (from lista in ListaStavkiUKorpi
                                         where lista.ime == "Mleko"
                                         select lista.kolicina * lista.cena).Sum();
            txtIznosPunaCena = PuterPunaCenaNaRacunu + HlebPunaCenaNaRacunu + MlekoPunaCenaNaRacunu;
            txtKonacnaCenaIspis = PuterPunaCenaNaRacunu + HlebPunaCenaNaRacunu + MlekoPunaCenaNaRacunu;
            SumaSumarum(ListaStavkiUKorpi);
        }
        private void ZavrsiKupovinu()
        {
            Message = "Kupovina je zavrsena,mozete ponovo da dodajete u novu korpu artikle.Hvala na kupovini.";
            ListaStavkiUKorpi.Clear();
            txtIznosPunaCena = 0.00;
            txtPopustiIspis = 0.00;
            txtKonacnaCenaIspis = 0.00;
            txtKolicinaUnos = 0;
        }
        private void OduzmiIzKorpe()
        {
            if (SelektovaniRedUKorpi != null)
            {
                ListaStavkiUKorpi = podaciService.ObrisiStavkuSaRacuna(ListaStavkiUKorpi, SelektovaniRedUKorpi);

                //ListaStavkiUKorpi.Remove(SelektovaniRedUKorpi);
            }
        }
        private void DodajUKorpu()
        {
            var daliPostojiArtikalVecUStavkama = (from lista in ListaStavkiUKorpi
                                                  where lista.ime == SelektovaniArtikal.ime
                                                  select lista.kolicina).Sum();
            if (SelektovaniArtikal != null && txtKolicinaUnos > 0)
            {
                if (daliPostojiArtikalVecUStavkama == 0)
                {
                    ListaStavkiUKorpi = podaciService.DodajStavkuURacun(ListaStavkiUKorpi, new StavkaRacuna { ime = SelektovaniArtikal.ime, kolicina = txtKolicinaUnos, cena = SelektovaniArtikal.cena, valuta = "$" });
                    //ListaStavkiUKorpi.Add(new StavkaRacuna { ime = SelektovaniArtikal.ime, kolicina = txtKolicinaUnos, cena = SelektovaniArtikal.cena, valuta = "$" });
                }
                else
                {
                    Message = "Ne mozete dodavati isti artikal vise puta u racun";
                }
            }
            else
            {
                Message = "Niste selektovali artikal ili niste uneli kolicinu vecu od 0,proverite pa pokusajte ponovo.";
            }
        }

        private void SumaSumarum(ObservableCollection<StavkaRacuna> listaStavkiUKorpi)
        {
            //2 putera i jedan hleb za 50% manje
            //3 mleka ,jedno gratis
            var prviUslovZaPutereKolikoPutaIhImaNaRacunu = (from lista in listaStavkiUKorpi
                                                            where lista.ime == "Puter"
                                                            select lista.kolicina).Sum();
            var drugiUslovZaHlebKolikoPutaIhImaNaRacunu = (from lista in listaStavkiUKorpi
                                                           where lista.ime == "Hleb"
                                                           select lista.kolicina).Sum();

            var drugiUslovZaMlekoKolikoPutaIhImaNaRacunu = (from lista in listaStavkiUKorpi
                                                            where lista.ime == "Mleko"
                                                            select lista.kolicina).Sum();

            RacunajPuterAkciju(prviUslovZaPutereKolikoPutaIhImaNaRacunu, drugiUslovZaHlebKolikoPutaIhImaNaRacunu); ;

            RacunajMlekoAkciju(drugiUslovZaMlekoKolikoPutaIhImaNaRacunu);


        }

        private void RacunajMlekoAkciju(int drugiUslovZaMlekoKolikoPutaIhImaNaRacunu)
        {


            if (drugiUslovZaMlekoKolikoPutaIhImaNaRacunu > 0 && drugiUslovZaMlekoKolikoPutaIhImaNaRacunu <= 2)
            {
                //nista da se ne radi sa ovim
            }
            else if (drugiUslovZaMlekoKolikoPutaIhImaNaRacunu > 0 && drugiUslovZaMlekoKolikoPutaIhImaNaRacunu % 3 == 0)
            {
                int kolikoPutaSeRacunaImaPo3Mleka = drugiUslovZaMlekoKolikoPutaIhImaNaRacunu / 3;//koliko puta se primenjuje akcija

                if (kolikoPutaSeRacunaImaPo3Mleka == 1)
                {
                    for (int i = 0; i < kolikoPutaSeRacunaImaPo3Mleka; i++)
                    {
                        ListaStavkiUKorpi.Add(new StavkaRacuna { ime = "MlekoGratis", kolicina = 1, cena = 1.15, valuta = "$" });
                    }

                    txtIznosPunaCena += 1.15 * kolikoPutaSeRacunaImaPo3Mleka;
                    txtPopustiIspis += 1.15 * kolikoPutaSeRacunaImaPo3Mleka;
                    txtKonacnaCenaIspis = txtIznosPunaCena - (1.15 * kolikoPutaSeRacunaImaPo3Mleka);
                }
                else
                {
                    txtPopustiIspis += 1.15 * kolikoPutaSeRacunaImaPo3Mleka;
                    txtKonacnaCenaIspis = txtIznosPunaCena - (1.15 * kolikoPutaSeRacunaImaPo3Mleka);
                }
            }
            else if (drugiUslovZaMlekoKolikoPutaIhImaNaRacunu > 0 && drugiUslovZaMlekoKolikoPutaIhImaNaRacunu % 3 > 0)
            {
                int kolikoPutaSeRacunaImaPo3Mleka = drugiUslovZaMlekoKolikoPutaIhImaNaRacunu / 3;//koliko puta se primenjuje akcija
                txtPopustiIspis += 1.15 * kolikoPutaSeRacunaImaPo3Mleka;
                txtKonacnaCenaIspis -= 1.15 * kolikoPutaSeRacunaImaPo3Mleka;
            }
        }

        private void RacunajPuterAkciju(int prviUslovZaPutereKolikoPutaIhImaNaRacunu, int drugiUslovZaHlebKolikoPutaIhImaNaRacunu)
        {
            if (prviUslovZaPutereKolikoPutaIhImaNaRacunu > 0 && prviUslovZaPutereKolikoPutaIhImaNaRacunu % 2 == 0)//uslov za akciju za putere
            {
                int kolikoPutaSeRacunaImaPo2Putera = prviUslovZaPutereKolikoPutaIhImaNaRacunu / 2;//koliko puta se primenjuje akcija

                if (drugiUslovZaHlebKolikoPutaIhImaNaRacunu > 0)//za hleb logika
                {
                    if (drugiUslovZaHlebKolikoPutaIhImaNaRacunu == 1)
                    {
                        txtPopustiIspis += 0.5;
                        txtKonacnaCenaIspis -= 0.5;
                    }
                    else
                    {//ako hleb postoji vise od jednog artikla u stavkama racuna da se onoliko puta koliko postoji
                     //uslova za akciju koju racunam u kolikoPutaSeRacunaImaPo2Putera da se toliko puta primeni na hlebove
                        txtPopustiIspis += 0.5 * kolikoPutaSeRacunaImaPo2Putera;
                        txtKonacnaCenaIspis -= 0.5 * kolikoPutaSeRacunaImaPo2Putera;
                    }
                }

            }
        }

        private void LoadData()
        {
            ListaArtikala = new ObservableCollection<Artikal>(podaciService.GetAll());
            ListaStavkiUKorpi = new ObservableCollection<StavkaRacuna>();
        }

        #endregion

        #region Properti
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }

        private ObservableCollection<StavkaRacuna> _ListaStavkiUKorpi;

        public ObservableCollection<StavkaRacuna> ListaStavkiUKorpi
        {
            get
            {
                return _ListaStavkiUKorpi;
            }
            set
            {
                _ListaStavkiUKorpi = value;
                OnPropertyChanged("ListaStavkiUKorpi");
            }
        }

        private ObservableCollection<Artikal> _ListaArtikala;

        public ObservableCollection<Artikal> ListaArtikala
        {
            get
            {
                return _ListaArtikala;
            }
            set
            {
                _ListaArtikala = value;
                OnPropertyChanged("ListaArtikala");
            }
        }


        private StavkaRacuna _SelektovaniRedUKorpi;

        public StavkaRacuna SelektovaniRedUKorpi
        {
            get { return _SelektovaniRedUKorpi; }
            set
            {
                _SelektovaniRedUKorpi = value;
                OnPropertyChanged("SelektovaniRedUKorpi");
            }
        }

        private Artikal _selektovaniArtikal;

        public Artikal SelektovaniArtikal
        {
            get { return _selektovaniArtikal; }
            set
            {
                _selektovaniArtikal = value;
                OnPropertyChanged("SelektovaniArtikal");
            }
        }



        private int _txtKolicinaUnos = 0;

        public int txtKolicinaUnos
        {
            get { return _txtKolicinaUnos; }
            set { _txtKolicinaUnos = value; OnPropertyChanged("txtKolicinaUnos"); }
        }



        private RelayCommand _DodajUKorpuCommand;

        public RelayCommand DodajUKorpuCommand
        {
            get { return _DodajUKorpuCommand; }
        }

        private RelayCommand _OduzmiIzKorpeCommand;

        public RelayCommand OduzmiIzKorpeCommand
        {
            get { return _OduzmiIzKorpeCommand; }
        }
        private RelayCommand _ZavrsiKupovinuCommand;

        public RelayCommand ZavrsiKupovinuCommand
        {
            get { return _ZavrsiKupovinuCommand; }
        }


        private RelayCommand _IzracunajCenuCommand;

        public RelayCommand IzracunajCenuCommand
        {
            get { return _IzracunajCenuCommand; }
        }

        private double _txtIznosPunaCena = 0.00;

        public double txtIznosPunaCena
        {
            get { return _txtIznosPunaCena; }
            set
            {
                _txtIznosPunaCena = value;
                OnPropertyChanged("txtIznosPunaCena");
            }
        }
        private double _txtPopustiIspis = 0.00;

        public double txtPopustiIspis
        {
            get { return _txtPopustiIspis; }
            set
            {
                _txtPopustiIspis = value;
                OnPropertyChanged("txtPopustiIspis");
            }
        }


        private double _txtKonacnaCenaIspis = 0.00;

        public double txtKonacnaCenaIspis
        {
            get { return _txtKonacnaCenaIspis; }
            set
            {
                _txtKonacnaCenaIspis = value;
                OnPropertyChanged("txtKonacnaCenaIspis");
            }
        }
        #endregion

        #region INotifyPropertyChanged_Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
