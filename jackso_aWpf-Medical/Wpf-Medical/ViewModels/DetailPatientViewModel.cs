using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Wpf_Medical.Views;
using System.Windows.Input;
using Wpf_Medical.DataAccess;
using System.Windows;
using System.ServiceModel;

namespace Wpf_Medical.ViewModels
{
    class DetailPatientViewModel : BaseViewModel, ServiceLive.IServiceLiveCallback
    {
        #region variables
        private readonly ServicePatient.Patient _selectedPatient;
        private ServicePatient.Observation _selectedObservation;

        private UserControl _currentView;
        private ServiceUser.User _userConnected;

        private bool _canEdit;

        private double _temperature;
        private double _timeHeart;

        private ICommand _addObservationCommand;

        private ObservableCollection<ServicePatient.Observation> _observationList;
        #endregion

        #region constructeur
        /// <summary>
        /// constructeur : prends le patient a afficher en argument
        /// </summary>
        /// <param name="p">Le patient a afficher</param>
        public DetailPatientViewModel(ServiceUser.User user,ServicePatient.Patient p)
        {
            _selectedPatient = p;
            _userConnected = user;
            CanEdit = true;
            if (_userConnected.Role == "Infirmière")
            {
                CanEdit = false;
            }
            TimeHeart = 0;
            Temparature = 0;
            //PatientsClient.Instance.SubscribeToPatient();
            if (p.Observations == null)
            {
                ObservationList = new ObservableCollection<ServicePatient.Observation>();
            }
            else
            {
                ObservationList = new ObservableCollection<ServicePatient.Observation>(p.Observations);
            }

            /*
            InstanceContext context = new InstanceContext(this);


            var client = new ServiceLive.ServiceLiveClient(context);
            try
            {
                client.Subscribe();
            }
            catch (CommunicationException c)
            {
                Console.WriteLine(c.StackTrace);
                MessageBox.Show("Erreur d'acces aux web-services");
            }
             * */
            
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
        public double Temparature
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
        public double TimeHeart
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
        public ServicePatient.Observation SelectedObservation
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
        public ObservableCollection<ServicePatient.Observation> ObservationList
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

        /// <summary>
        /// droit d'ajout /suppression
        /// </summary>
        public bool CanEdit
        {
            get { return _canEdit; }
            set
            {
                if (!(Equals(_canEdit, value)))
                {
                    _canEdit = value;
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




        public void PushDataHeart(double requestData)
        {
            TimeHeart = requestData;
        }

        public void PushDataTemp(double requestData)
        {
            Temparature = requestData;
        }
    }
}
