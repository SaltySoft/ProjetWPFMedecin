using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Medical.DataAccess
{
    public class PatientsClient
    {
        private ObservableCollection<Dbo.Patient> _patientListPatients = null;

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
    }
}
