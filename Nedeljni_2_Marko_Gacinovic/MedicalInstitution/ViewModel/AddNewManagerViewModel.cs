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
    class AddNewManagerViewModel : ViewModelBase
    {
        AddManagerView addManager;

        // properties
        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private tblManager manager;
        public tblManager Manager
        {
            get { return manager; }
            set { manager = value; OnPropertyChanged("Manager"); }
        }

        private bool isUpdateManager;
        public bool IsUpdateManager
        {
            get { return isUpdateManager; }
            set { isUpdateManager = value; }
        }

        public AddNewManagerViewModel(AddManagerView addManagerOpen, tblUser userToPass)
        {
            addManager = addManagerOpen;
            user = userToPass;
            manager = new tblManager();
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

        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    tblManager newManager = new tblManager();

                    newManager.HospitalLevel = manager.HospitalLevel;
                    newManager.MaxDoctors = manager.MaxDoctors;
                    newManager.MinRooms = manager.MinRooms;
                    newManager.Erors = manager.Erors;

                    newManager.ManagerID = manager.ManagerID;

                    tblUser viaUser = (from x in context.tblUsers where x.UserId == user.UserId select x).First();
                    newManager.UserID = viaUser.UserId;

                    context.tblManagers.Add(newManager);
                    context.SaveChanges();

                    IsUpdateManager = true;
                }
                addManager.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs, please check your inputs or try again.");
            }
        }

        private bool CanSaveExecute()
        {
            return true;
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
                addManager.Close();
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
