using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalInstitution.ViewModel
{
    class EditUserViewModel : ViewModelBase
    {
        EditUserView editUser;

        // properties
        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private bool isUpdateUser;
        public bool IsUpdateUser
        {
            get { return isUpdateUser; }
            set { isUpdateUser = value; }
        }

        public EditUserViewModel(EditUserView editUserOpen, tblUser userToPass)
        {
            editUser = editUserOpen;
            user = userToPass;
        }
    }
}
