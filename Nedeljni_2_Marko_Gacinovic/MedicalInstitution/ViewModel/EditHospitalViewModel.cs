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
    class EditHospitalViewModel : ViewModelBase
    {
        EditHospitalView editHospitalView;

        private tblHospital hospital;
        public tblHospital Hospital
        {
            get { return hospital; }
            set { hospital = value; OnPropertyChanged("Hospital"); }
        }

        private bool isUpdateHospital;
        public bool IsUpdateHospital
        {
            get { return isUpdateHospital; }
            set { isUpdateHospital = value; }
        }

        public EditHospitalViewModel(EditHospitalView editHospitalOpen, tblHospital hospitalToPass)
        {
            editHospitalView = editHospitalOpen;
            hospital = hospitalToPass;
        }

        // command for editing the user
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
            set { save = value; }
        }

        private bool CanSaveExecute()
        {
            if (string.IsNullOrEmpty(hospital.Owns))
            {
                return false;
            }
            return true;
        }

        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    int id = hospital.HospitalID;

                    tblHospital hospitalToEdit = (from x in context.tblHospitals where x.HospitalID == id select x).First();

                    hospitalToEdit.Owns = hospital.Owns;
                    hospitalToEdit.InvalidPoint = hospital.InvalidPoint;
                    hospitalToEdit.AmbulancePoint = hospital.AmbulancePoint;
                    hospital.HospitalID = hospitalToEdit.HospitalID;

                    context.SaveChanges();

                    isUpdateHospital = true;
                }
                editHospitalView.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong input, please try again.");
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
                editHospitalView.Close();
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
    }
}
