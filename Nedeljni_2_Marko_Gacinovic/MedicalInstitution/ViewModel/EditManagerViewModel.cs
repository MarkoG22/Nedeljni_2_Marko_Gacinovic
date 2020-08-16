using MedicalInstitution.Commands;
using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.ViewModel
{
    class EditManagerViewModel : ViewModelBase
    {
        EditManagerView editManager;

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

        // constructor
        public EditManagerViewModel(EditManagerView editManagerOpen, tblManager managerToPass)
        {     
            editManager = editManagerOpen;
            manager = managerToPass;
            
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

        /// <summary>
        /// method for editing the manager
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    int id = manager.UserID;
                    tblUser newUser = (from x in context.tblUsers where x.UserId == id select x).First();
                    tblManager managerToEdit = (from x in context.tblManagers where x.UserID == id select x).First(); 

                    newUser.FullName = user.FullName;
                    newUser.IdCard = user.IdCard;

                    string sex = user.Gender.ToUpper();

                    // gender validation
                    if ((sex == "M" || sex == "Z" || sex == "X" || sex == "N"))
                    {
                        newUser.Gender = sex;
                    }
                    else
                    {
                        MessageBox.Show("Wrong Gender input, please enter M, Z, X or N.");
                    }

                    newUser.Birthdate = user.Birthdate;
                    newUser.Citizenship = user.Citizenship;
                    newUser.Manager = false;
                    newUser.Username = user.Username;
                    

                    if (PasswordValidation(user.Pasword))
                    {
                        newUser.Pasword = user.Pasword;
                    }
                    else
                    {
                        MessageBox.Show("Wrong password. Password must have at least 8 characters.\n(1 upper char, 1 lower char, 1 number and 1 special char)\nPlease try again.");
                    }


                    newUser.UserId = user.UserId;


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
                    managerToEdit.UserID = newUser.UserId;

                    context.SaveChanges();   

                    FileActions.FileActions.Instance.Editing(FileActions.FileActions.path, FileActions.FileActions.actions, "manager", user.FullName);

                    IsUpdateManager = true;
                    IsUpdateUser = true;
                }
                editManager.Close();
                MessageBox.Show("The manager edited succesfully.");
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

        /// <summary>
        /// method for the password validation
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool PasswordValidation(string password)
        {
            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,15}$");

            bool isValidated = regex.IsMatch(password);

            if (isValidated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
