using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for EditMaintanceView.xaml
    /// </summary>
    public partial class EditMaintanceView : Window
    {
        public EditMaintanceView(tblMaintance maintance)
        {
            InitializeComponent();
            this.DataContext = new EditMaintanceViewModel(this, maintance);
        }

        private void LettersValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
