using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VolMarker
{
    public class VolSurfaceModel : NotifyPropertyChanged
    {
        public VolSurfaceModel()
        {
            Underlier = "SPX";
            VolSurface = new Dictionary<string, double>();
            VolSurface["1m"] = 20;
            VolSurface["3m"] = 21;
            VolSurface["6m"] = 23;
        }

        private string _underlier;
        public string Underlier {
            get { return _underlier; }
            set { _underlier = value; OnPropertyChanged(); }
        }
        private Dictionary<string, double> _volSurface;
        public Dictionary<string,double> VolSurface
        {
            get { return _volSurface; }
            set { _volSurface = value; OnPropertyChanged(); }
        }

    }
}
