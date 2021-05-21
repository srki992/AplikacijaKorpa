using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija.Models
{
    public class PodaciService : IPodaciService
    {
        List<Artikal> listaArtikala;
        List<StavkaRacuna> listaStavkeRacuna;
        public PodaciService()
        {
            listaArtikala = new List<Artikal>();
            listaStavkeRacuna = new List<StavkaRacuna>();
        }


        public List<Artikal> GetAll()
        {
            listaArtikala.Add(new Artikal { ime = "Mleko", cena = 1.15, valuta = "$" });
            listaArtikala.Add(new Artikal { ime = "Hleb", cena = 1.00, valuta = "$" });
            listaArtikala.Add(new Artikal { ime = "Puter", cena = 0.80, valuta = "$" });

            return listaArtikala;
        }

        public ObservableCollection<StavkaRacuna> DodajStavkuURacun(ObservableCollection<StavkaRacuna> stavkaRacuna, StavkaRacuna stavkaKojaSeDodajeNaRacun)//dva param postojeca lista,i red koji se dodajeoduzima
        {
            stavkaRacuna.Add(stavkaKojaSeDodajeNaRacun);
            return stavkaRacuna;
        }

        public ObservableCollection<StavkaRacuna> ObrisiStavkuSaRacuna(ObservableCollection<StavkaRacuna> stavkaRacuna, StavkaRacuna stavkaKojaSeOduzimaSaRacuna)//dva param postojeca lista,i red koji se dodajeoduzima
        {
            stavkaRacuna.Remove(stavkaKojaSeOduzimaSaRacuna);
            return stavkaRacuna;
        }



    }
}
