﻿using MedicalInstitution.Commands;
using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.ViewModel
{
    class CreateHospitalViewModel : ViewModelBase
    {
        CreateHospitalView createHospital;

        // properties
        private tblHospital hospital;
        public tblHospital Hospital
        {
            get { return hospital; }
            set { hospital = value; OnPropertyChanged("Hospital"); }
        }

        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        // constructor
        public CreateHospitalViewModel(CreateHospitalView createHospitalOpen, tblUser userToPass)
        {
            user = userToPass;
            createHospital = createHospitalOpen;
            hospital = new tblHospital();
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
            if (String.IsNullOrEmpty(hospital.HospitalName) || String.IsNullOrEmpty(hospital.Adress)
                || String.IsNullOrEmpty(hospital.Owns))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// method for adding new hospital
        /// </summary>
        private void SaveExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    tblHospital newHospital = new tblHospital();

                    newHospital.HospitalName = hospital.HospitalName;
                    newHospital.Adress = hospital.Adress;
                    newHospital.StartDate = DateTime.Now.Date;
                    newHospital.Owns = hospital.Owns;
                    newHospital.Flors = hospital.Flors;
                    newHospital.Levels = hospital.Levels;
                    newHospital.Balcony = hospital.Balcony;
                    newHospital.Yard = hospital.Yard;
                    newHospital.AmbulancePoint = hospital.AmbulancePoint;
                    newHospital.InvalidPoint = hospital.InvalidPoint;
                    newHospital.HospitalID = hospital.HospitalID;

                    tblUser viaUser = (from x in context.tblUsers where x.UserId == user.UserId select x).First();

                    // variable that hospital is created once
                    viaUser.LoggedIn = true;

                    // saving data
                    context.tblHospitals.Add(newHospital);
                    context.SaveChanges();

                    // logging the action
                    FileActions.FileActions.Instance.Adding(FileActions.FileActions.path, FileActions.FileActions.actions, "hospital", viaUser.FullName);
                }
                createHospital.Close();
                MessageBox.Show("The hospital created succesfully.");
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
                createHospital.Close();
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
