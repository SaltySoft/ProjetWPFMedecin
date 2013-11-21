using Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf_Medical.Views;

namespace Wpf_Medical.ViewModels
{
    class DetailObservationViewModel : BaseViewModel
    {
        #region variables
        private readonly Observation _observation;
        private UserControl _currentView;

        private ICommand _picturesCommand;
        private ICommand _prescriptionsCommand;

        #endregion

        #region constructeur
        /// <summary>
        /// constructeur : prends l' observation a afficher en argument
        /// </summary>
        /// <param name="obs">l'observation a afficher</param>
        public DetailObservationViewModel (Observation obs)
        {
            _observation = obs;
            PushPicturesView();
        }
        #endregion

        #region methodes

        /// <summary>
        /// Charge la sous-vue des images
        /// </summary>
        private void PushPicturesView()
        {
            var picturesView = new Views.ImageListViewControl();
            var picturesVM = new ImageListViewModel(_observation.Pictures);
            picturesView.DataContext = picturesVM;
            CurrentView = picturesView;
        }

        /// <summary>
        /// charge la sous-vue des prescriptions
        /// </summary>
        private void PushPrescrciptionsView()
        {
            var prescriptionsView = new Views.PrescriptionListViewControl();
            var prescriptionsVM = new PrescriptionListViewModel(_observation.Prescription);
            prescriptionsView.DataContext = prescriptionsVM;
            CurrentView = prescriptionsView;
        }

        #endregion

        #region getters/setters
        /// <summary>
        /// l'observation affichee
        /// </summary>
        public Observation Observation
        {
            get { return _observation; }
        }

        /// <summary>
        /// la sous-vue chargee
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

        #region commands
        /// <summary>
        /// bouton des prescriptions
        /// </summary>
        public ICommand PrescriptionsCommand
        {
            get
            {
                if (_prescriptionsCommand == null)
                {
                    _prescriptionsCommand = new RelayCommand(
                        param => PushPrescrciptionsView(),
                        param => true
                    );
                }
                return _prescriptionsCommand;
            }
        }

        /// <summary>
        /// bouton des images
        /// </summary>
        public ICommand PicturesCommand
        {
            get
            {
                if (_picturesCommand == null)
                {
                    _picturesCommand = new RelayCommand(
                        param => PushPicturesView(),
                        param => true
                    );
                }
                return _picturesCommand;
            }
        }
        #endregion
    }
}
