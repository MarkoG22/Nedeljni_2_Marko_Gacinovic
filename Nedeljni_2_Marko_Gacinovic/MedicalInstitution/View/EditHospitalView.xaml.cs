using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for EditHospitalView.xaml
    /// </summary>
    public partial class EditHospitalView : Window
    {
        public EditHospitalView(tblHospital hospital)
        {
            InitializeComponent();
            this.DataContext = new EditHospitalViewModel(this, hospital);
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
