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
    class EditManagerViewModel : ViewModelBase
    {
        EditManagerView editManager;

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

        public EditManagerViewModel(EditManagerView editManagerOpen, tblManager managerToPass)
        {
            manager = managerToPass;
            editManager = editManagerOpen;
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
                    int id = manager.ManagerID;

                    tblManager managerToEdit = (from x in context.tblManagers where x.ManagerID == id select x).First();

                    if (manager.Erors == null)
                    {
                        managerToEdit.Erors = 0;
                    }
                    else
                    {
                        managerToEdit.Erors = manager.Erors;
                    }

                    if (manager.HospitalLevel == null)
                    {
                        managerToEdit.HospitalLevel = 0;
                    }
                    else
                    {
                        managerToEdit.HospitalLevel = manager.HospitalLevel;
                    }

                    if (managerToEdit.Erors <= 5)
                    {
                        managerToEdit.MaxDoctors = manager.MaxDoctors;
                        managerToEdit.MinRooms = manager.MinRooms;
                    }
                    else
                    {
                        MessageBox.Show("Manager with 5 or more errors can not monitor any doctor or room.");
                        managerToEdit.MaxDoctors = 0;
                        managerToEdit.MinRooms = 0;
                    }

                    managerToEdit.ManagerID = manager.ManagerID;
                    managerToEdit.UserID = manager.UserID;

                    context.SaveChanges();

                    int userID = manager.UserID;

                    tblUser user = (from y in context.tblUsers where y.UserId == userID select y).First();

                    FileActions.FileActions.Instance.Editing(FileActions.FileActions.path, FileActions.FileActions.actions, "manager", user.FullName);

                    IsUpdateManager = true;
                }
                editManager.Close();
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
                editManager.Close();
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
