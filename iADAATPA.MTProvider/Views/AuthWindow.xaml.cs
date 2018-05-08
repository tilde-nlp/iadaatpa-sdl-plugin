using iADAATPA.MTProvider.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace iADAATPA.MTProvider.Views
{
    /// <summary>
    /// Interaction logic for AuthView.xaml
    /// </summary>
    public partial class AuthWindow : ClosableWindow
    {
        public AuthWindow(AuthViewModel vm) : base(vm)
        {
            InitializeComponent();
            vm.ShowMessage += Vm_ShowMessage;
        }

        private void Vm_ShowMessage(object sender, string e)
            => MessageBox.Show(e);
    }
}
