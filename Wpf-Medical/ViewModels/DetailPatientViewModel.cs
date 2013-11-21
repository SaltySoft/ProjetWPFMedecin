using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using Dbo;
using System.Collections.ObjectModel;
using Wpf_Medical.Views;
using System.Windows.Input;

namespace Wpf_Medical.ViewModels
{
    class DetailPatientViewModel : BaseViewModel
    {
        #region variables
        private readonly Patient _selectedPatient;
        private Observation _selectedObservation;

        private UserControl _currentView;

        private int _temperature;
        private int _timeHeart;

        private ICommand _addObservationCommand;

        private ObservableCollection<Observation> _observationList;
        #endregion

        #region constructeur
        /// <summary>
        /// constructeur : prends le patient a afficher en argument
        /// </summary>
        /// <param name="p">Le patient a afficher</param>
        public DetailPatientViewModel(Patient p)
        {
            _selectedPatient = p;
            _timeHeart = 0;
            _temperature = 0;
            _observationList = new ObservableCollection<Observation> (p.Observations);
        }
        #endregion


        #region methodes
        /// <summary>
        /// Charge la vue d'ajout des observation
        /// </summary>
        private void LoadAddObservationView()
        {
            var addObserverView = new AddObservationControl();
            var addObserverVM = new AddObservationViewModel(this,_selectedPatient);
            addObserverView.DataContext = addObserverVM;
            CurrentView = addObserverView;
        }

        /// <summary>
        /// charge la vue de detail d'une observation
        /// </summary>
        private void LoadDetailObservationView()
        {
            var detailObserverView = new DetailObservationControl();
            var detailObserverVM = new DetailObservationViewModel(_selectedObservation);
            detailObserverView.DataContext = detailObserverVM;
            CurrentView = detailObserverView;
        }
        #endregion

        #region getters / setters

        #region attribus

        /// <summary>
        /// La temperature du patient
        /// </summary>
        public int Temparature
        {
            get { return _temperature; }
            set
            {
                if (value != _temperature)
                {
                    _temperature = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Le rythme cardiaque du patient
        /// </summary>
        public int TimeHeart
        {
            get { return _timeHeart; }
            set
            {
                if (value != _timeHeart)
                {
                    _timeHeart = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// la derniere observation selectionnee
        /// </summary>
        public Observation SelectedObservation
        {
            get { return _selectedObservation; }
            set
            {
                if (value != _selectedObservation)
                {
                    _selectedObservation = value;
                    OnPropertyChanged();
                    LoadDetailObservationView();
                }
            }
        }

        /// <summary>
        /// la liste des observations
        /// </summary>
        public ObservableCollection<Observation> ObservationList
        {
            get { return _observationList; }
            set
            {
                if (value != _observationList)
                {
                    _observationList = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// la sous-vue actuelle
        /// </summary>
        public UserControl CurrentView
        {
            get { return _currentView; }
            set
            {
                if (!Equals(_currentView, value))
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region commandes
        /// <summary>
       /// bouton pour ajouter une observation
       /// </summary>
        public ICommand AddObservationCommand
        {
            get
            {
                if (_addObservationCommand == null)
                {
                    _addObservationCommand = new RelayCommand(
                        param => LoadAddObservationView(),
                        param => true
                    );
                }
                return _addObservationCommand;
            }
        }
        #endregion

        #endregion



    }
}
