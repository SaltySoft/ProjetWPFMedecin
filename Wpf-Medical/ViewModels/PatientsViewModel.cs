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
        #region variables
        /// <summary>
        /// la sous-vue
        /// </summary>
        private UserControl _currentView;

        /// <summary>
        /// le patient
        /// </summary>
        private Patient _selectedPatient;

        /// <summary>
        /// commandes
        /// </summary>
        private ICommand _addPatientButtonCommand;
        private ICommand _removePatientButtonCommand;

        #endregion

        #region constructeur
        /// <summary>
        /// constructeur : construit la vue des patients
        /// </summary>
        public PatientsViewModel()
        {
            PatientsClient.Instance.RefreshPatients();
        }
        #endregion

        #region methodes
        /// <summary>
        /// charge vue detaillee d'un patient
        /// </summary>
        private void LoadDetailView()
        {
            var detailControl = new DetailPatientControl();
            var detailVM = new DetailPatientViewModel(_selectedPatient);
            detailControl.DataContext = detailVM;
            CurrentView = detailControl;
        }

        /// <summary>
        /// charge la vue d' ajout d' un patient
        /// </summary>
        private void LoadAddView()
        {
            var addControl = new AddPatientControl();
            var addVM = new AddPatientViewModel();
            addControl.DataContext = addVM;
            CurrentView = addControl;
        }

        /// <summary>
        /// supprime le patient selectionne
        /// </summary>
        private void RemovPatient()
        {
            if (_selectedPatient != null)
            {
                PatientsClient.Instance.RemovePatient(_selectedPatient.Id);
                SelectedPatient = PatientList.FirstOrDefault();
            }
        }
        #endregion

        #region getters / setters

        #region attribus

        /// <summary>
        /// la liste des patients
        /// </summary>
        public ObservableCollection<Patient> PatientList
        {
            get { return PatientsClient.Instance.GetPatients(); }
        }

        /// <summary>
        /// le patient selectionne
        /// </summary>
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

        /// <summary>
        /// la sous-vue actuelle (ajout / detail sur le patient)
        /// </summary>
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
        #endregion

        #region commandes

        /// <summary>
        /// bouton d' ajout d' un patient
        /// </summary>
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
        
        /// <summary>
        /// bouton de suppression d' un patient
        /// </summary>
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
        #endregion

        #endregion

    }
}
