using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija.Models
{
    public class Racun : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _ukupnasuma;

        public double ukupnasuma
        {
            get { return _ukupnasuma; }
            set { _ukupnasuma = value; OnPropertyChanged("ukupnasuma"); }
        }

        public double RacunajPopust()
        {
            return 0;
        }


    }
}
