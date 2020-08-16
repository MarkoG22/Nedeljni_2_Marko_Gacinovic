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
    class EditUserViewModel : ViewModelBase
    {
        EditUserView editUser; // patient

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

        private tblPatient patient;
        public tblPatient Patient
        {
            get { return patient; }
            set { patient = value; OnPropertyChanged("Patient"); }
        }

        // constructor
        public EditUserViewModel(EditUserView editUserOpen, tblUser userToPass, tblPatient patientToPass)
        {
            editUser = editUserOpen;
            user = userToPass;
            patient = patientToPass;
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

        /// <summary>
        /// method for editing the patient
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    int id = user.UserId;

                    tblUser newUser = (from x in context.tblUsers where x.UserId == id select x).First();
                    tblPatient newPatient = (from y in context.tblPatients where y.UserID == id select y).First();

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

                    user.UserId = newUser.UserId;

                    newPatient.CardNumber = patient.CardNumber;
                    newPatient.DateExpire = DateTime.Now.AddYears(2);
                    newPatient.PatientID = patient.PatientID;
                    newPatient.UserID = newUser.UserId;

                    context.tblPatients.Add(newPatient);
                    context.SaveChanges();

                    FileActions.FileActions.Instance.Editing(FileActions.FileActions.path, FileActions.FileActions.actions, "patient", newUser.FullName);

                    IsUpdateUser = true;
                }
                editUser.Close();
                MessageBox.Show("The patient edited succesfully.");
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs, please check your inputs or try again.");
            }
        }

        /// <summary>
        /// method for Save button disabled
        /// </summary>
        /// <returns></returns>
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
                editUser.Close();
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
