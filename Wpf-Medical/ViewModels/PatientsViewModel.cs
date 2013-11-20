using Dbo;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using Wpf_Medical.Views;
using Wpf_Medical.DataAccess;
using System.Windows.Input;
using System.Linq;

namespace Wpf_Medical.ViewModels
{
    class PatientsViewModel : BaseViewModel
    {
        private UserControl _currentView;

        private ICommand _addPatientButtonCommand;
        private ICommand _removePatientButtonCommand;


        private Patient _selectedPatient;


        public PatientsViewModel()
        {
            PatientsClient.Instance.RefreshPatients();
        }

        private void LoadDetailView()
        {
            var detailControl = new DetailPatientControl();
            var detailVM = new DetailPatientViewModel(_selectedPatient);
            detailControl.DataContext = detailVM;
            CurrentView = detailControl;
        }

        private void LoadAddView()
        {
            var addControl = new AddPatientUserControl();
            var addVM = new AddPatientViewModel();
            addControl.DataContext = addVM;
            CurrentView = addControl;
        }

        private void RemovPatient()
        {
            if (_selectedPatient != null)
            {
                PatientsClient.Instance.RemovePatient(_selectedPatient.Id);
                SelectedPatient = PatientList.FirstOrDefault();
            }
        }

        public ObservableCollection<Patient> PatientList
        {
            get { return PatientsClient.Instance.GetPatients(); }
        }

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                if (_selectedPatient != value)
                {
                    _selectedPatient = value;
                    OnPropertyChanged();
                    LoadDetailView();
                }
            }
        }


        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (!(Equals(_currentView, value)))
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddPatientButtonCommand
        {
            get
            {
                return _addPatientButtonCommand ?? (_addPatientButtonCommand = new RelayCommand(
                    param => LoadAddView(),
                    param => true
                    ));
            }
        }

        public ICommand RemovePatientButtonCommand
        {
            get
            {
                return _removePatientButtonCommand ?? (_removePatientButtonCommand = new RelayCommand(
                    param => RemovPatient(),
                    param => true
                    ));
            }
        }



    }
}
