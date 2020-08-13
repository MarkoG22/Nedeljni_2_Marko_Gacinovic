using MedicalInstitution.Commands;
using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.ViewModel
{
    class EditMaintanceViewModel : ViewModelBase
    {
        EditMaintanceView editMaintance;

        // properties
        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private tblMaintance maintance;
        public tblMaintance Maintance
        {
            get { return maintance; }
            set { maintance = value; OnPropertyChanged("Maintance"); }
        }

        private bool isUpdateMaintance;
        public bool IsUpdateMaintance
        {
            get { return isUpdateMaintance; }
            set { isUpdateMaintance = value; }
        }

        public EditMaintanceViewModel(EditMaintanceView editMaintanceOpen, tblMaintance maintanceToPass)
        {
            editMaintance = editMaintanceOpen;
            maintance = maintanceToPass;
        }

        // commands
        private ICommand save;
        public ICommand Save
        {
            get
            {
                if (save == null)
                {
                    save = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return save;
            }
        }

        private bool CanSaveExecute()
        {
            return true;
        }

        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    int id = maintance.MaintanceID;

                    tblMaintance maintanceToEdit = (from x in context.tblMaintances where x.MaintanceID == id select x).First();

                    maintanceToEdit.GrowPermision = maintance.GrowPermision;
                    maintanceToEdit.InvalidDuty = maintance.InvalidDuty;

                    if (maintanceToEdit.InvalidDuty == true)
                    {
                        maintanceToEdit.AmbulanceDuty = false;
                    }
                    else
                    {
                        maintanceToEdit.AmbulanceDuty = true;
                    }

                    tblUser viaUser = (from x in context.tblUsers where x.UserId == maintance.UserID select x).First();
                    maintanceToEdit.UserID = viaUser.UserId;
                    maintanceToEdit.MaintanceID = maintance.MaintanceID;
                    
                    context.SaveChanges();

                    IsUpdateMaintance = true;
                }
                editMaintance.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs, please check your inputs or try again.");
            }
        }

        // command for closing the window
        private ICommand close;
        public ICommand Close
        {
            get
            {
                if (close == null)
                {
                    close = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return close;
            }
        }

        /// <summary>
        /// method for closing the window
        /// </summary>
        private void CloseExecute()
        {
            try
            {
                editMaintance.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private bool CanCloseExecute()
        {
            return true;
        }
    }
}
