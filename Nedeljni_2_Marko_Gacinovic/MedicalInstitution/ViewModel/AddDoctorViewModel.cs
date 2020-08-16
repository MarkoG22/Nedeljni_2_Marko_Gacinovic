using MedicalInstitution.Commands;
using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.ViewModel
{
    class AddDoctorViewModel : ViewModelBase
    {
        AddDoctorView addDoctor;

        #region Properties
        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private tblDoctor doctor;
        public tblDoctor Doctor
        {
            get { return doctor; }
            set { doctor = value; OnPropertyChanged("Doctor"); }
        }

        private tblShift shift;
        public tblShift Shift
        {
            get { return shift; }
            set { shift = value; OnPropertyChanged("Shift"); }
        }

        private vwManager manager;
        public vwManager Manager
        {
            get { return manager; }
            set { manager = value; }
        }

        private List<tblShift> shiftList;
        public List<tblShift> ShiftList
        {
            get { return shiftList; }
            set { shiftList = value; OnPropertyChanged("ShiftList"); }
        }

        private bool isUpdateUser;
        public bool IsUpdateUser
        {
            get { return isUpdateUser; }
            set { isUpdateUser = value; }
        }

        private bool isUpdateDoctor;
        public bool IsUpdateDoctor
        {
            get { return isUpdateDoctor; }
            set { isUpdateDoctor = value; }
        }
        #endregion

        // constructor
        public AddDoctorViewModel(AddDoctorView addDoctorOpen,vwManager viewManager)
        {
            addDoctor = addDoctorOpen;
            user = new tblUser();
            doctor = new tblDoctor();
            manager = viewManager;
            ShiftList = GetAllShift();
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
        /// method for adding the doctor
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    MessageBox.Show("If there is no managers, please create manager first. \nThank you.");

                    tblDoctor newDoctor = new tblDoctor();
                    tblUser newUser = new tblUser();

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

                    newDoctor.Department = doctor.Department;
                    newDoctor.UniqueNumber = doctor.UniqueNumber;
                    newDoctor.AccountNumber = doctor.AccountNumber;
                    newDoctor.Reception = doctor.Reception;
                    newDoctor.ShiftID = shift.ShiftID;

                    newDoctor.DoctorID = doctor.DoctorID;
                    newDoctor.UserID = user.UserId;                    

                    //if (manager.MaxDoctors > 0)
                    //{
                    //    newDoctor.ManagerID = manager.ManagerID;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry, the manager can not monitor this doctor because of maximum doctors monitoring number.");                        
                    //}

                    // adding the doctor to the database
                    context.tblUsers.Add(newUser);
                    context.tblDoctors.Add(newDoctor);
                    context.SaveChanges();
                    
                    // logging the action
                    FileActions.FileActions.Instance.Editing(FileActions.FileActions.path, FileActions.FileActions.actions, "doctor", newUser.FullName);

                    // variables for updating the lists
                    IsUpdateUser = true;
                    IsUpdateDoctor = true;
                }

                MessageBox.Show("Doctor created succesfully.");
                addDoctor.Close();
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
                addDoctor.Close();
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
        /// method for getting all shifts to the list
        /// </summary>
        /// <returns></returns>
        private List<tblShift> GetAllShift()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblShift> list = new List<tblShift>();
                    list = (from x in context.tblShifts select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
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
