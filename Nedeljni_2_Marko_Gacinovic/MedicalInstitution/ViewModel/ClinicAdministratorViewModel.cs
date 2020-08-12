using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalInstitution.ViewModel
{
    class ClinicAdministratorViewModel : ViewModelBase
    {
        ClinicAdministrator clinicAdministrator;        

        // properties
        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private tblHospital hospital;
        public tblHospital Hospital
        {
            get { return hospital; }
            set { hospital = value; OnPropertyChanged("Hospital"); }
        }        

        public ClinicAdministratorViewModel(ClinicAdministrator adminOpen, tblUser userToPass)
        {
            user = userToPass;
            clinicAdministrator = adminOpen;
        }      

    }
}
