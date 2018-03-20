using iADAATPA.MTProvider.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace iADAATPA.MTProvider.ViewModels
{
    public abstract class ClosableViewModel : INotifyPropertyChanged
    {
        public event EventHandler ClosingRequest;

        protected bool? _dialogResult;

        protected void OnClosingRequest() => this.ClosingRequest?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// Provides a DialogResult for the Window that binds to the ClosableViewModel
        /// </summary>
        /// <remarks>
        /// Note that after closing the DialogResult of a Window is either true or false.
        /// A null value is changed to false.
        /// </remarks>
        public virtual bool? DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; Notify(); }
        }

        public void Notify() => this.Notify(PropertyChanged);
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
