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
        private API.IClient _client;

        public AuthViewModel(API.IClient client)
        {
            _client = client;

            _goCommand = new DelegateCommand(() =>
            {
                bool isValidToken = _client.ValidateToken(AuthToken).Result;
                if (isValidToken)
                {
                    DialogResult = true;
                    OnClosingRequest();
                    return;
                }
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
    }
}
