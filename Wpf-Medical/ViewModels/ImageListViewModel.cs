using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Medical.ViewModels
{
    class ImageListViewModel : BaseViewModel
    {
        #region variables

        private byte[][] _pictures;

        #endregion

        #region constructeur

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="pictures">liste des images a afficher</param>
        public ImageListViewModel(byte[][] pictures)
        {
            _pictures = pictures;
        }
        #endregion

        #region getters / setters
        /// <summary>
        /// liste des images
        /// </summary>
        public byte[][] Pictures
        {
            get { return _pictures; }
        }
        #endregion
    }
}
