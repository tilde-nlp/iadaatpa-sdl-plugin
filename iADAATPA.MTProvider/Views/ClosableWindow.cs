using iADAATPA.MTProvider.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace iADAATPA.MTProvider.Views
{
    public class ClosableWindow : Window
    {
        private ClosableViewModel viewModel;
        public ClosableWindow(): base() { }
        public ClosableWindow(ClosableViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = viewModel;
            viewModel.ClosingRequest += viewModel_ClosingRequest;
        }

        void viewModel_ClosingRequest(object sender, EventArgs e) => this.Close();
    }
}
