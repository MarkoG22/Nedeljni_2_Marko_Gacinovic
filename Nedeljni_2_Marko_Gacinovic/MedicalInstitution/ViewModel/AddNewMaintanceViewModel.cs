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
    class AddNewMaintanceViewModel : ViewModelBase
    {
        AddNewMaintanceView addMaintance;

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

        public AddNewMaintanceViewModel(AddNewMaintanceView addMaintanceOpen, tblUser userToPass)
        {
            user = userToPass;
            addMaintance = addMaintanceOpen;
            maintance = new tblMaintance();
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
                    tblMaintance newMaintance = new tblMaintance();

                    newMaintance.GrowPermision = maintance.GrowPermision;
                    newMaintance.InvalidDuty = maintance.InvalidDuty;

                    if (newMaintance.InvalidDuty == true)
                    {
                        newMaintance.AmbulanceDuty = false;
                    }
                    else
                    {
                        newMaintance.AmbulanceDuty = true;
                    }

                    

                    tblUser viaUser = (from x in context.tblUsers where x.UserId == user.UserId select x).First();
                    newMaintance.UserID = viaUser.UserId;
                    newMaintance.MaintanceID = maintance.MaintanceID;

                    context.tblMaintances.Add(newMaintance);
                    context.SaveChanges();

                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "maintance", viaUser.FullName);

                    IsUpdateMaintance = true;
                }
                addMaintance.Close();
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
                addMaintance.Close();
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
