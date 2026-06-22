using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VUV_osiguranje.PoliceOsiguranja;

namespace VUV_osiguranje.Stete
{
    internal class Steta
    {
        private Polica _polica;
        private DateTime _datum;
        private string _opis;
        private double _iznos;
        private StatusStete _status;
       

        public Steta(Polica polica, DateTime datum, string opis, double iznos, StatusStete status)
        {
            _polica = polica;
            _datum = datum;
            _opis = opis;
            _iznos = iznos;
            _status = status;
        }

        public Polica polica
        {
            get { return _polica; }
            set { _polica = value; }
        }

        public DateTime datum
        {
            get { return _datum; }
            set { _datum = value; }
        }

        public string opis
        {
            get { return _opis; }
            set { _opis = value; }
        }

        public double iznos
        {
            get { return _iznos; }
            set { _iznos = value; }
        }

        public StatusStete status
        {
            get { return _status; }

            set
            {
                if(_status == StatusStete.Odobrena || _status == StatusStete.Odbijena)
                {
                    throw new InvalidOperationException("Status se ne može mijenjati nakon odobrenja ili odbijanja.");
                }

                _status = value;
            }
        }

        public void Odobri()
        {
            if(_status != StatusStete.Prijavljena)
            {
                throw new InvalidOperationException();
            }

            status = StatusStete.Odobrena;
        }

        public void Odbij()
        {
            if(_status != StatusStete.Prijavljena)
            {
                throw new InvalidOperationException();
            }

            status = StatusStete.Odbijena;
        }
    }
}
