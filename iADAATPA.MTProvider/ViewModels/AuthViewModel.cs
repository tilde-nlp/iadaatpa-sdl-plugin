using iADAATPA.MTProvider.Extensions;
using iADAATPA.MTProvider.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace iADAATPA.MTProvider.ViewModels
{
    public class AuthViewModel : ClosableViewModel
    {
        private string _authToken;
        private ICommand _goCommand;
        public AuthViewModel()
        {
            _goCommand = new DelegateCommand(() =>
            {
                DialogResult = true;
                OnClosingRequest();
            });
        }
        public string AuthToken {
            get => _authToken;
            set
            {
                if(_authToken != value)
                {
                    _authToken = value;
                    this.Notify();
                }
            }
        }

        public ICommand GoCommand => _goCommand;

        public event EventHandler GoExecuted;
    }
}
