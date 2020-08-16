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
    class EditDoctorViewModel : ViewModelBase
    {
        EditDoctorView editDoctor;

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

        private bool isUpdateDoctor;
        public bool IsUpdateDoctor
        {
            get { return isUpdateDoctor; }
            set { isUpdateDoctor = value; }
        }

        private tblShift shift;
        public tblShift Shift
        {
            get { return shift; }
            set { shift = value; OnPropertyChanged("Shift"); }
        }

        private List<tblShift> shiftList;
        public List<tblShift> ShiftList
        {
            get { return shiftList; }
            set { shiftList = value; OnPropertyChanged("ShiftList"); }
        }

        public EditDoctorViewModel(EditDoctorView editDoctorOpen, tblDoctor doctorToPass, tblUser userToPass)
        {
            user = userToPass;
            doctor = doctorToPass;
            editDoctor = editDoctorOpen;
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
            return true;
        }

        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    int id = doctor.DoctorID;
                    int userID = user.UserId;

                    tblUser newUser = (from x in context.tblUsers where x.UserId == id select x).First();
                    tblDoctor doctorToEdit = (from y in context.tblDoctors where y.DoctorID == id select y).First();

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

                    doctorToEdit.Department = doctor.Department;
                    doctorToEdit.UniqueNumber = doctor.UniqueNumber;
                    doctorToEdit.AccountNumber = doctor.AccountNumber;
                    doctorToEdit.Reception = doctor.Reception;
                    doctorToEdit.ShiftID = shift.ShiftID;

                    doctorToEdit.DoctorID = doctor.DoctorID;
                    doctorToEdit.UserID = doctor.UserID;                    

                    tblManager manager = (from x in context.tblManagers where x.UserID == user.UserId select x).First();

                    if (manager.MaxDoctors > 0)
                    {
                        doctorToEdit.ManagerID = manager.ManagerID;
                    }
                    else
                    {
                        MessageBox.Show("Sorry, the manager can not monitor this doctor because of maximum doctors monitoring number.");
                    }

                    context.SaveChanges();

                    FileActions.FileActions.Instance.Editing(FileActions.FileActions.path, FileActions.FileActions.actions, "doctor", newUser.FullName);

                    isUpdateDoctor = true;
                }
                editDoctor.Close();
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
                editDoctor.Close();
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
