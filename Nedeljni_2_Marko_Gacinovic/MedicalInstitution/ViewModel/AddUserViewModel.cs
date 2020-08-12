using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalInstitution.ViewModel
{
    class AddUserViewModel : ViewModelBase
    {
        AddUserView addUser;

        private bool isUpdateUser;
        public bool IsUpdateUser
        {
            get { return isUpdateUser; }
            set { isUpdateUser = value; }
        }


        public AddUserViewModel(AddUserView addUserOpen)
        {
            addUser = addUserOpen;
        }
    }
}
