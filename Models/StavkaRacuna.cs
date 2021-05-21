﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikacija.Models
{
    public class StavkaRacuna : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _ime;
        private double _cena;
        private int _kolicina;
        private string _valuta;
        public string ime
        {
            get { return _ime; }
            set { _ime = value; OnPropertyChanged("ime"); }
        }
        public double cena
        {
            get { return _cena; }
            set { _cena = value; OnPropertyChanged("cena"); }
        }
        public int kolicina
        {
            get { return _kolicina; }
            set { _kolicina = value; OnPropertyChanged("kolicina"); }
        }
        public string valuta
        {
            get { return _valuta; }
            set { _valuta = value; OnPropertyChanged("valuta"); }
        }
    }
}
