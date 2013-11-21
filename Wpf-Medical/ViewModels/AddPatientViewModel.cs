using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Dbo;

namespace Wpf_Medical.ViewModels
{
    class AddPatientViewModel : BaseViewModel
    {
        #region variables
        private string _firstName;
        private string _lastName;
        private DateTime _birthDay;

        private ICommand _addPatientCommand;

        #endregion

        #region constructeur
        /// <summary>
        /// constructeur
        /// </summary>
        public AddPatientViewModel()
        {
            _birthDay = DateTime.Now.Date;
        }
        #endregion

        #region methodes
        /// <summary>
        /// Ajoute un patient (appel aux webservices)
        /// </summary>
        private void AddPatient()
        {
            Patient p = new Patient()
            {
                Firstname = _firstName,
                Name = _lastName,
                Birthday = _birthDay,
                Observations = new List<Observation> ()
            };
            DataAccess.PatientsClient.Instance.AddPatient(p);
        }
        #endregion

        #region getters / setters

        #region attribus
        /// <summary>
        /// Le prenom du patient
        /// </summary>
        public String FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }


        public String LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }


        public DateTime BirthDay
        {
            get { return _birthDay; }
            set
            {
                if (_birthDay != value)
                {
                    _birthDay = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region commandes
        public ICommand AddPatientCommand
        {
            get
            {
                return _addPatientCommand ?? (_addPatientCommand = new RelayCommand(
                    param => AddPatient(),
                    param => true
                    ));
            }
        }
        #endregion

        #endregion
    }
}
