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
    class EditDoctorViewModel : ViewModelBase
    {
        EditDoctorView editDoctor;

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

        public EditDoctorViewModel(EditDoctorView editDoctorOpen, tblDoctor doctorToPass)
        {
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

                    tblDoctor doctorToEdit = (from x in context.tblDoctors where x.DoctorID == id select x).First();

                    doctorToEdit.Department = doctor.Department;
                    doctorToEdit.UniqueNumber = doctor.UniqueNumber;
                    doctorToEdit.AccountNumber = doctor.AccountNumber;
                    doctorToEdit.Reception = doctor.Reception;
                    doctorToEdit.ShiftID = shift.ShiftID;

                    doctorToEdit.DoctorID = doctor.DoctorID;
                    doctorToEdit.UserID = doctor.UserID;

                    int userID = doctorToEdit.UserID;

                    tblUser viaUser = (from y in context.tblUsers where y.UserId == userID select y).First();

                    tblManager manager = (from x in context.tblManagers where x.UserID == viaUser.UserId select x).First();

                    if (manager.MaxDoctors > 0)
                    {
                        doctorToEdit.ManagerID = manager.ManagerID;
                    }
                    else
                    {
                        MessageBox.Show("Sorry, the manager can not monitor this doctor because of maximum doctors monitoring number.");
                    }

                    context.SaveChanges();

                    FileActions.FileActions.Instance.Editing(FileActions.FileActions.path, FileActions.FileActions.actions, "doctor", viaUser.FullName);

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
    }
}
