using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dbo;
using System.Collections.ObjectModel;

namespace Wpf_Medical.ViewModels
{
    class DetailPatientViewModel : BaseViewModel
    {
        private readonly Patient _selectedPatient;
        private byte[] _connectedPicture;
        private  ObservableCollection<Observation> _observationList;

        public DetailPatientViewModel(Patient p)
        {
            _selectedPatient = p;
            _observationList = new ObservableCollection<Observation> (p.Observations);
        }

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
        }

        public ObservableCollection<Observation> ObservationList
        {
            get { return _observationList; }
        }

    }
}
