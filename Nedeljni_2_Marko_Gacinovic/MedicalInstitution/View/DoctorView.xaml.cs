using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System.Windows;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for DoctorView.xaml
    /// </summary>
    public partial class DoctorView : Window
    {
        public DoctorView(tblDoctor doctor)
        {
            InitializeComponent();
            this.DataContext = new DoctorViewModel(this, doctor);
        }
    }
}
