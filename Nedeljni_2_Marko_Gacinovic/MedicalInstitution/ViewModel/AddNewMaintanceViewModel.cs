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

        private bool isUpdateUser;
        public bool IsUpdateUser
        {
            get { return isUpdateUser; }
            set { isUpdateUser = value; }
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


        //constructor
        public AddNewMaintanceViewModel(AddNewMaintanceView addMaintanceOpen)
        {
            addMaintance = addMaintanceOpen;
            user = new tblUser();            
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

        /// <summary>
        /// method for adding new maintance
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    tblMaintance newMaintance = new tblMaintance();
                    tblUser newUser = new tblUser();

                    // inputs and validations...
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
                                        
                    newMaintance.UserID = user.UserId;
                    newMaintance.MaintanceID = maintance.MaintanceID;

                    // adding the maintance to the database
                    context.tblMaintances.Add(newMaintance);
                    context.tblUsers.Add(newUser);
                    context.SaveChanges();

                    // logging the action
                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "maintance", newUser.FullName);

                    IsUpdateMaintance = true;
                    IsUpdateUser = true;
                }
                MessageBox.Show("The maintance created succesfully.");
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
