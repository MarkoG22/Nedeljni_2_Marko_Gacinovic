using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for ClinicAdministrator.xaml
    /// </summary>
    public partial class ClinicAdministrator : Window
    {
        public ClinicAdministrator(tblUser user, tblManager manager, tblDoctor doctor)
        {
            InitializeComponent();
            this.DataContext = new ClinicAdministratorViewModel(this, user, manager, doctor);
        }
    }
}
