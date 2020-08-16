using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for EditUserView.xaml
    /// </summary>
    public partial class EditUserView : Window
    {
        public EditUserView(tblUser user, tblPatient patient)
        {
            InitializeComponent();
            this.DataContext = new EditUserViewModel(this, user, patient);
        }

        private void LettersValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
