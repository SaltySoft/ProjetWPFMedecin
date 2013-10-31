using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_Medical.Helpers
{
    /// <summary>
    /// permet de fermer une fenêtre par binding MVVM ///
    /// </summary>
    public static class WindowCloseBehaviour
    {
        #region methodes

        /// <summary>
        /// Ferme la fenetre
        /// </summary>
        /// <param name="target">Cible</param>
        /// <param name="value">Valeur</param>
        public static void SetClose(DependencyObject target, bool value)
        {
            target.SetValue(CloseProperty, value);
        }

        public static readonly DependencyProperty CloseProperty = DependencyProperty.RegisterAttached("Close",
            typeof (bool), typeof (WindowCloseBehaviour),
            new UIPropertyMetadata(false, OnClose));

        /// <summary>
        /// Evenement close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnClose(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool && ((bool)e.NewValue))
            {
                Window window = GetWindow(sender);
                if (window != null)
                    window.Close();
            }
        }

        /// <summary>
        /// retourne la fenetre 
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private static Window GetWindow(DependencyObject sender)
        {
            Window window = null;

            if (sender is Window)
                window = (Window)sender;
            if (window == null)
                window = Window.GetWindow(sender);

            return window;
        }

        #endregion
    }
}
