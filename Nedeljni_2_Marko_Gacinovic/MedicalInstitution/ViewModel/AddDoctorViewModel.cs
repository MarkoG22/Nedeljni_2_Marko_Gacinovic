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
    class AddDoctorViewModel : ViewModelBase
    {
        AddDoctorView addDoctor;

        // properties
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

        private List<tblShift> shiftList;
        public List<tblShift> ShiftList
        {
            get { return shiftList; }
            set { shiftList = value; OnPropertyChanged("ShiftList"); }
        }

        private bool isUpdateDoctor;
        public bool IsUpdateDoctor
        {
            get { return isUpdateDoctor; }
            set { isUpdateDoctor = value; }
        }

        public AddDoctorViewModel(AddDoctorView addDoctorOpen, tblUser userToPass)
        {
            addDoctor = addDoctorOpen;
            user = userToPass;
            doctor = new tblDoctor();
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
                    tblDoctor newDoctor = new tblDoctor();

                    newDoctor.Department = doctor.Department;
                    newDoctor.UniqueNumber = doctor.UniqueNumber;
                    newDoctor.AccountNumber = doctor.AccountNumber;
                    newDoctor.Reception = doctor.Reception;
                    newDoctor.ShiftID = shift.ShiftID;

                    newDoctor.DoctorID = doctor.DoctorID;
                    newDoctor.UserID = user.UserId;
                    // ManagerID !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                    context.tblDoctors.Add(newDoctor);
                    context.SaveChanges();

                    IsUpdateDoctor = true;
                }
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
