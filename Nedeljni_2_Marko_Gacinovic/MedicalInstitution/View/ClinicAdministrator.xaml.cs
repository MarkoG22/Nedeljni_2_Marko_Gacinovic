using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System.Windows;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for ClinicAdministrator.xaml
    /// </summary>
    public partial class ClinicAdministrator : Window
    {
        public ClinicAdministrator(tblUser user, tblManager manager, tblDoctor doctor, tblPatient patient)
        {
            InitializeComponent();
            this.DataContext = new ClinicAdministratorViewModel(this, user, manager, doctor, patient);
        }
    }
}
