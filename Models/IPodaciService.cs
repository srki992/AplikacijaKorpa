using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija.Models
{
    public interface IPodaciService
    {
        List<Artikal> GetAll();
        ObservableCollection<StavkaRacuna> DodajStavkuURacun(ObservableCollection<StavkaRacuna> stavkaRacuna, StavkaRacuna stavkaKojaSeDodajeNaRacun);
        ObservableCollection<StavkaRacuna> ObrisiStavkuSaRacuna(ObservableCollection<StavkaRacuna> stavkaRacuna, StavkaRacuna stavkaKojaSeOduzimaSaRacuna);
   
    }
}
