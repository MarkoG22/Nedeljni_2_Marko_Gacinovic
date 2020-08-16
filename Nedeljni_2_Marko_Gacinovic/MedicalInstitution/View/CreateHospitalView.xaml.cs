using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for CreateHospitalView.xaml
    /// </summary>
    public partial class CreateHospitalView : Window
    {
        public CreateHospitalView(tblUser user)
        {
            InitializeComponent();
            this.DataContext = new CreateHospitalViewModel(this, user);
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
