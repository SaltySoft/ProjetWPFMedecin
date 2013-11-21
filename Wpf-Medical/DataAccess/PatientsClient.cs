using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Patient = Dbo.Patient;
using Observation = Dbo.Observation;

namespace Wpf_Medical.DataAccess
{
    public class PatientsClient
    {
        private ObservableCollection<Dbo.Patient> _patientList = null;

        private static PatientsClient instance = null;

        /// <summary>
        /// Thread-safe
        /// </summary>
        private static readonly object padlock = new object();


        public static PatientsClient Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new PatientsClient();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        private PatientsClient()
        {
        }


        /// <summary>
        /// Charge les patients crees par default definis par le web-service.
        /// </summary>
        public ObservableCollection<Patient> LoadPatients()
        {
            var clientPatient = new ServicePatient.ServicePatientClient();
            var patientList = new ObservableCollection<Patient>();
            try
            {
                Dbo.Patient[] patients = clientPatient.GetListPatient();
                patientList = new ObservableCollection<Patient> (patients);
            }
            catch (CommunicationException)
            {
                MessageBox.Show("Erreur d'acces aux web-services");
            }
            return patientList;
        }

        /// <summary>
        /// retourne la liste des patients
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<Patient> GetPatients()
        {
            if (_patientList == null)
            {
                _patientList = new ObservableCollection<Patient>();
                _patientList = LoadPatients();
            }
            return _patientList;
        }
   
        /// <summary>
        /// Ajoute le patient
        /// </summary>
        /// <param name="patient">Le patient a ajouter</param>
        public async void AddPatient(Patient patient)
        {
            var client = new ServicePatient.ServicePatientClient();
            try
            {
                Task<bool> didAddPatient = client.AddPatientAsync(patient);
                if (await didAddPatient)
                {
                    patient.Id = _patientList.OrderBy(x => x.Id).Last().Id + 1;
                    _patientList.Add(patient);
                }
            }
             catch (CommunicationException c)
             {
                 Console.WriteLine(c.StackTrace);
                 MessageBox.Show("Erreur d'acces aux web-services");
             }
        }

        /// <summary>
        /// Ajoute l' observation au patient
        /// </summary>
        /// <param name="obs">L'observation</param>
        /// <param name="p">Le patient</param>
        public async void AddObservationToPatient(Observation obs, Patient p)
        {
            var client = new ServiceObservation.ServiceObservationClient();
            try
            {
                Task<bool> didAddObservation = client.AddObservationAsync(p.Id, obs);
                if (await didAddObservation)
                {
                    p.Observations.Add(obs);
                }
            }
            catch (CommunicationException c)
            {
                Console.WriteLine(c.StackTrace);
                MessageBox.Show("Erreur d'acces aux web-services");
            }
        }

        /// <summary>
        /// Rafrachie la liste des patients
        /// </summary>
        public void RefreshPatients()
        {
            _patientList = LoadPatients();
        }

        /// <summary>
        /// souscription au service live
        /// </summary>
        public void SubscribeToPatient()
        {
            var client = new ServiceLive.IServiceLive();
            try
            {
                client.SubscribeAsync();
            }
            catch (CommunicationException c)
            {
                Console.WriteLine(c.StackTrace);
                MessageBox.Show("Erreur d'acces aux web-services");
            }
        }

        /// <summary>
        /// Verifie l' existence du patient.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public bool PatientExists(int id)
        {
            return _patientList.Any(p => p.Id == id);
        }

        /// <summary>
        /// Supprimer un patient
        /// </summary>
        /// <param name="id">L'id du patient a supprimer.</param>
        public async void RemovePatient(int id)
        {
            var client = new ServicePatient.ServicePatientClient();
            Task<bool> res = client.DeletePatientAsync(id);
            if (await res)
            {
                _patientList.Remove(_patientList.First(x => x.Id == id));   
            }
        }


    }
}
