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

        // constructor
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

        /// <summary>
        /// method for editing the maintance
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    
                    tblUser newUser = (from x in context.tblUsers where x.UserId == maintance.UserID select x).First();
                    tblMaintance maintanceToEdit = (from x in context.tblMaintances where x.MaintanceID == maintance.MaintanceID select x).First();

                    // inputs and validations
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
                    
                    maintanceToEdit.UserID = user.UserId;
                    maintanceToEdit.MaintanceID = maintance.MaintanceID;
                    
                    // saving the data
                    context.SaveChanges();

                    // logging the action
                    FileActions.FileActions.Instance.Editing(FileActions.FileActions.path, FileActions.FileActions.actions, "maintance", user.FullName);

                    IsUpdateMaintance = true;
                }
                editMaintance.Close();
                MessageBox.Show("The maintance edited succesfully.");
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
