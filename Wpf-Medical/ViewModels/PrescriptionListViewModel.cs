using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Medical.ViewModels
{
    class PrescriptionListViewModel : BaseViewModel
    {
        #region variables

        private string[] _prescriptions;

        #endregion

        #region constructeur

        /// <summary>
        /// construit la liste des prescriptions
        /// </summary>
        /// <param name="prescriptions"></param>
        public PrescriptionListViewModel(string[] prescriptions)
        {
            _prescriptions = prescriptions;
        }

        #endregion

        #region getters / setters

        /// <summary>
        /// liste de prescriptions
        /// </summary>
        public String[] Prescriptions
        {
            get {  return _prescriptions; }
        }

        #endregion

    }
}
