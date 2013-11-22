
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;


namespace Wpf_Medical.ViewModels
{
    class BaseViewModel : INotifyPropertyChanged
    {
        #region variables

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region constructeur

        protected  BaseViewModel() {}

        #endregion

        #region methodes

        /// <summary>
        ///     Declenche l'evenment lorsque le nom de la propriete change
        /// </summary>
        /// <param name="propertyName"> Le nom de la propriete </param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///  Verifie que l'element binde existe bien.
        /// </summary>
        /// <param name="propertyName"></param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                    Debug.Fail(msg);
            }
        }
        #endregion

    }
}
