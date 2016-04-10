using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VolMarker
{
    public class VolSurfaceViewModel : NotifyPropertyChanged
    {
        VolSurfaceModel _model;
        public VolSurfaceModel Model { get { return _model; } set { _model = value; OnPropertyChanged(); } }
        public VolSurfaceViewModel()
        {
            Model = new VolSurfaceModel();
        }
    }
}
