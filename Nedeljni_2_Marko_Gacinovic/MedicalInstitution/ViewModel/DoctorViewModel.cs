using MedicalInstitution.Commands;
using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace MedicalInstitution.ViewModel
{
    class DoctorViewModel : ViewModelBase
    {
        DoctorView doctorView;

        private tblDoctor doctor;
        public tblDoctor Doctor
        {
            get { return doctor; }
            set { doctor = value; }
        }

        private tblPatient patient;
        public tblPatient Patient
        {
            get { return patient; }
            set { patient = value; OnPropertyChanged("Patient"); }
        }

        private List<tblPatient> patientList;
        public List<tblPatient> PatientList
        {
            get { return patientList; }
            set { patientList = value; OnPropertyChanged("PatientList"); }
        }

        public DoctorViewModel(DoctorView doctorOpen, tblDoctor doctorToPass)
        {
            doctorView = doctorOpen;
            doctor = doctorToPass;
            PatientList = GetAllPatient();
        }

        // commands
        private ICommand check;
        public ICommand Check
        {
            get
            {
                if (check == null)
                {
                    check = new RelayCommand(param => AddNewCheckExecute(), param => CanAddNewCheckExecute());
                }
                return check;
            }
        }

        private bool CanAddNewCheckExecute()
        {
            return true;
        }

        /// <summary>
        /// method for opening the window for checking patients
        /// </summary>
        private void AddNewCheckExecute()
        {
            try
            {
                CheckPatient checkPatient = new CheckPatient();
                checkPatient.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// method for getting all patients to the list
        /// </summary>
        /// <returns></returns>
        private List<tblPatient> GetAllPatient()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblPatient> list = new List<tblPatient>();
                    list = (from x in context.tblPatients select x).ToList();
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
