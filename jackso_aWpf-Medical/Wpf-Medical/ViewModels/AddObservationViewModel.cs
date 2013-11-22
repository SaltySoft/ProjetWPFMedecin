using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using Wpf_Medical.DataAccess;
using System.IO;

namespace Wpf_Medical.ViewModels
{
    class AddObservationViewModel : BaseViewModel
    {
        #region variables
        /// <summary>
        /// view model parent pour mettre a jour la liste des observations
        /// </summary>
        private DetailPatientViewModel _detailPatientVM;

        private ServicePatient.Patient _patient;

        /// <summary>
        /// Donnees du patient
        /// </summary>
        private int _selectedWeight;
        private int _selectedBloodPressure;
        private string _selectedPrescription;
        private string _selectedPicture;
        private DateTime _date;
        private string _comment;
        private ObservableCollection<string> _pictures;
        private ObservableCollection<string> _prescriptions;


        private string _currentPrescription;
        private List<int> _weights;
        private List<int> _bloodPressures;

        /// <summary>
        /// Commandes
        /// </summary>
        private ICommand _addPescriptionCommand;
        private ICommand _addPictureCommand;
        private ICommand _deletePrescriptionCommand;
        private ICommand _deletePictureCommand;
        private ICommand _addObservationCommand;
        #endregion

        #region constructeur
        /// <summary>
        /// constructeur
        /// </summary>
        /// <param name="detailPatientVM">Le viewModel Parent</param>
        /// <param name="p">Le patient</param>
        public AddObservationViewModel(DetailPatientViewModel detailPatientVM, ServicePatient.Patient p)
        {
            _detailPatientVM = detailPatientVM;
            _patient = p;
            _prescriptions = new ObservableCollection<string>();
            _pictures = new ObservableCollection<string>();
            SelectedWeight = 60;
            _weights = new List<int>();
            _bloodPressures = new List<int>();
            _currentPrescription = "";
            Date = DateTime.Now.Date;

            for (int i = 1; i < 200; i++)
            {
                _weights.Add(i);
            }
            for (int i = 0; i < 30; i++)
            {
                _bloodPressures.Add(i);
            }
        }
        #endregion

        #region methodes
        /// <summary>
        /// Ajoute une observation (appel aux webservices)
        /// </summary>
        private async void AddObservation()
        {
            var obs = new ServiceObservation.Observation
            {
                BloodPressure = _selectedBloodPressure,
                Comment = _comment,
                Date = _date,
                Weight = _selectedWeight,
                Prescription = _prescriptions.ToArray()
            };
            
            int nbPictures = _pictures.Count;
            obs.Pictures = new Byte[nbPictures][];
            for (int j = 0; j < nbPictures; j++)
            {
                var sr = new StreamReader(_pictures.ElementAt(j));
                var read = new BinaryReader(sr.BaseStream);
                obs.Pictures[j] = read.ReadBytes((int)sr.BaseStream.Length);
            }

            ServicePatient.Patient patient = await PatientsClient.Instance.AddObservationToPatient(obs, _patient);
            _detailPatientVM.ObservationList = new ObservableCollection<ServicePatient.Observation>(patient.Observations);
        }

        /// <summary>
        /// Ajoute la prescription a la liste de prescriptions
        /// </summary>
        private void AddPrescriptionToArray()
        {
            _prescriptions.Add(_currentPrescription);
        }
        /// <summary>
        /// Supprimer la prescription de la liste de prescriptions
        /// </summary>
        private void DeletePrescriptioSelected()
        {
            _prescriptions.Remove(_selectedPrescription);
        }

        /// <summary>
        /// Supprimer l'image de la liste d'images
        /// </summary>
        private void DeletePictureSelected()
        {
            _pictures.Remove(_selectedPicture);
        }

        /// <summary>
        /// ajoute l'image renseignee a la liste d'images
        /// </summary>
        private void UpdateImage()
        {
            var op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                _pictures.Add(op.FileName);
            }
        }
        #endregion

        #region getter /setters

        #region attribus
        /// <summary>
        /// liste deroulante des poids
        /// </summary>
        public List<int> Weights
        {
            get { return _weights;}
        }

        /// <summary>
        /// liste deroulante des tensions
        /// </summary>
        public List<int> BloodPressures
        {
            get { return _bloodPressures;}
        }

        /// <summary>
        /// le poids selectionne
        /// </summary>
        public int SelectedWeight
        {
            get { return _selectedWeight; }
            set
            {
                if (value != _selectedWeight)
                {
                    _selectedWeight = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// la tension selectionnee
        /// </summary>
        public int SelectedBloodPressure
        {
            get { return _selectedBloodPressure; }
            set
            {
                if (value != _selectedBloodPressure)
                {
                    _selectedBloodPressure = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// La date de l' observation
        /// </summary>
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value != _date)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Le commentaire de l' onservation
        /// </summary>
        public String Comment
        {
            get { return _comment; }
            set
            {
                if (value != _comment)
                {
                    _comment = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// liste des prescriptions a envoyer au web-service
        /// </summary>
        public ObservableCollection<string> Prescriptions
        {
            get { return _prescriptions; }
        }

        /// <summary>
        /// liste des images a envoyer au web-service
        /// </summary>
        public ObservableCollection<string> Pictures
        {
            get { return _pictures; }
            set
            {
                if (value != _pictures)
                {
                    _pictures = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// prescription a ajouter a la liste de prescription
        /// </summary>
        public String CurrentPrescription
        {
            get { return _currentPrescription; }
            set
            {
                if (value != _currentPrescription)
                {
                    _currentPrescription = value;
                    OnPropertyChanged();
                }
            }
        }


        /// <summary>
        /// prescription selectionnee
        /// </summary>
        public String SelectedPrescription
        {
            get { return _selectedPrescription;  }
            set
            {
                if (value != _selectedPrescription)
                {
                    _selectedPrescription = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// image selectionnee
        /// </summary>
        public String SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                if (value != _selectedPicture)
                {
                    _selectedPicture = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region commands
        /// <summary>
        /// pour le bouton d'ajout de prescriptions
        /// </summary>
        public ICommand AddPrescriptionCommand
        {
            get
            {
                if (_addPescriptionCommand == null)
                {
                    _addPescriptionCommand = new RelayCommand(
                        param => AddPrescriptionToArray(),
                        param => true
                    );
                }
                return _addPescriptionCommand;
            }
        }

        /// <summary>
        /// pour le bouton d' ajout d' image
        /// </summary>
        public ICommand AddPictureCommand
        {
            get
            {
                if (_addPictureCommand == null)
                {
                    _addPictureCommand = new RelayCommand(
                        param => UpdateImage(),
                        param => true
                    );
                }
                return _addPictureCommand;
            }
        }

        /// <summary>
        /// pour le bouton de suppression de prescription
        /// </summary>
        public ICommand DeletePrescriptionCommand
        {
            get
            {
                if (_deletePrescriptionCommand == null)
                {
                    _deletePrescriptionCommand = new RelayCommand(
                        param => DeletePrescriptioSelected(),
                        param => true
                    );
                }
                return _deletePrescriptionCommand;
            }
        }

        /// <summary>
        /// pour le bouton de suppresion d' image
        /// </summary>
        public ICommand DeletePictureCommand
        {
            get
            {
                if (_deletePictureCommand == null)
                {
                    _deletePictureCommand = new RelayCommand(
                        param => DeletePictureSelected(),
                        param => true
                    );
                }
                return _deletePictureCommand;
            }
        }

        /// <summary>
        /// pour le bouton d' ajout d' observation
        /// </summary>
        public ICommand AddObservationCommand
        {
            get
            {
                if (_addObservationCommand == null)
                {
                    _addObservationCommand = new RelayCommand(
                        param => AddObservation(),
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
