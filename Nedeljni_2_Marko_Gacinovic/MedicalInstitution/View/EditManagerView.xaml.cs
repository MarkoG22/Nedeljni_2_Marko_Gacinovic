using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;


namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for EditManagerView.xaml
    /// </summary>
    public partial class EditManagerView : Window
    {
        public EditManagerView(tblManager manager)
        {
            InitializeComponent();
            this.DataContext = new EditManagerViewModel(this, manager);
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void LettersValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
