using MedicalInstitution.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;


namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for MasterView.xaml
    /// </summary>
    public partial class MasterView : Window
    {
        public MasterView()
        {
            InitializeComponent();
            this.DataContext = new MasterViewModel(this);
        }

        private void LettersValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
