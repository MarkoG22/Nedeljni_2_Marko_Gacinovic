using MedicalInstitution.Commands;
using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public AddNewManagerViewModel(AddManagerView addManagerOpen)
        {
            addManager = addManagerOpen;
            user = new tblUser();
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
                    tblUser newUser = new tblUser();

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
                    string pass = user.Pasword;

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
                        newManager.Erors = 0;
                    }
                    else
                    {
                        newManager.Erors = manager.Erors;
                    }

                    if (manager.HospitalLevel == null)
                    {
                        newManager.HospitalLevel = 0;
                    }
                    else
                    {
                        newManager.HospitalLevel = manager.HospitalLevel;
                    }

                    if (newManager.Erors <=5)
                    {
                        newManager.MaxDoctors = manager.MaxDoctors;
                        newManager.MinRooms = manager.MinRooms;
                    }
                    else
                    {
                        MessageBox.Show("Manager with 5 or more errors can not monitor any doctor or room.");
                        newManager.MaxDoctors = 0;
                        newManager.MinRooms = 0;
                    }                  
                    
                    newManager.ManagerID = manager.ManagerID;
                    
                    newManager.UserID = user.UserId;

                    context.tblUsers.Add(newUser);
                    context.tblManagers.Add(newManager);
                    context.SaveChanges();

                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "manager", user.FullName);

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
            if (String.IsNullOrEmpty(user.FullName) || String.IsNullOrEmpty(user.IdCard)
                || String.IsNullOrEmpty(user.Gender) || String.IsNullOrEmpty(user.Citizenship)
                || String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.Pasword)
                || !user.Citizenship.All(Char.IsLetter))
            {
                return false;
            }
            else
            {
                return true;
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
