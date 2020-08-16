using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for AddDoctorView.xaml
    /// </summary>
    public partial class AddDoctorView : Window
    {
        public AddDoctorView(vwManager manager)
        {
            InitializeComponent();
            this.DataContext = new AddDoctorViewModel(this, manager);
        }

        /// <summary>
        /// method for textbox numbers validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// method for texbox letters validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LettersValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
