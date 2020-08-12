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
    class ClinicAdministratorViewModel : ViewModelBase
    {
        ClinicAdministrator clinicAdministrator;        

        // properties
        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }

        private List<tblUser> userList;
        public List<tblUser> UserList
        {
            get { return userList; }
            set { userList = value; OnPropertyChanged("UserList"); }
        }


        private tblHospital hospital;
        public tblHospital Hospital
        {
            get { return hospital; }
            set { hospital = value; OnPropertyChanged("Hospital"); }
        }

        private List<tblHospital> hospitalList;
        public List<tblHospital> HospitalList
        {
            get { return hospitalList; }
            set { hospitalList = value; OnPropertyChanged("HospitalList"); }
        }


        private tblDoctor doctor;
        public tblDoctor Doctor
        {
            get { return doctor; }
            set { doctor = value; OnPropertyChanged("Doctor"); }
        }

        private List<tblDoctor> doctorList;
        public List<tblDoctor> DoctorList
        {
            get { return doctorList; }
            set { doctorList = value; OnPropertyChanged("DoctorList"); }
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


        private tblMaintance maintance;
        public tblMaintance Maintance
        {
            get { return maintance; }
            set { maintance = value; OnPropertyChanged("Maintance"); }
        }

        private List<tblMaintance> maintanceList;
        public List<tblMaintance> MaintanceList
        {
            get { return maintanceList; }
            set { maintanceList = value; OnPropertyChanged("MaintanceList"); }
        }


        private tblManager manager;
        public tblManager Manager
        {
            get { return manager; }
            set { manager = value; OnPropertyChanged("Manager"); }
        }

        private List<tblManager> managerList;
        public List<tblManager> ManagerList
        {
            get { return managerList; }
            set { managerList = value; OnPropertyChanged("ManagerList"); }
        }

        // constructor
        public ClinicAdministratorViewModel(ClinicAdministrator adminOpen)
        {            
            clinicAdministrator = adminOpen;

            UserList = GetAllUser();
            HospitalList = GetAllHospital();
            DoctorList = GetAllDoctor();
            PatientList = GetAllPatient();
            MaintanceList = GetAllMaintance();
            ManagerList = GetAllManager();
        }

        // commands
        private ICommand addNewUser;
        public ICommand AddNewUser
        {
            get
            {
                if (addNewUser == null)
                {
                    addNewUser = new RelayCommand(param => AddNewUserExecute(), param => CanAddNewUserExecute());
                }
                return addNewUser;
            }
        }

        private bool CanAddNewUserExecute()
        {
            return true;
        }

        private void AddNewUserExecute()
        {
            try
            {
                AddUserView addUser = new AddUserView();
                addUser.ShowDialog();
                // updating the project list view
                if ((addUser.DataContext as AddUserViewModel).IsUpdateUser == true)
                {
                    UserList = GetAllUser().ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private ICommand deleteUser;
        public ICommand DeleteUser
        {
            get
            {
                if (deleteUser == null)
                {
                    deleteUser = new RelayCommand(param => DeleteUserExecute(), param => CanDeleteUserExecute());
                }
                return deleteUser;
            }

        }

        private bool CanDeleteUserExecute()
        {
            return true;
        }

        private void DeleteUserExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    int id = user.UserId;

                    // checking the action
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the user?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        tblUser userToDelete = (from x in context.tblUsers where x.UserId == id select x).First();

                        context.tblUsers.Remove(userToDelete);
                        context.SaveChanges();

                        UserList = GetAllUser();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, the user can not be deleted.");
            }
        }

        // command for editing the user
        private ICommand editUser;
        public ICommand EditUser
        {
            get
            {
                if (editUser == null)
                {
                    editUser = new RelayCommand(param => EditUserExecute(), param => CanEditUserExecute());
                }
                return editUser;
            }
        }

        private bool CanEditUserExecute()
        {
            return true;
        }

        /// <summary>
        /// method for opening the view for the editing user
        /// </summary>
        private void EditUserExecute()
        {
            try
            {
                EditUserView editUser = new EditUserView(user);
                editUser.ShowDialog();
                if ((editUser.DataContext as EditUserViewModel).IsUpdateUser == true)
                {
                    UserList = GetAllUser().ToList();                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }




        private List<tblUser> GetAllUser()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblUser> list = new List<tblUser>();
                    list = (from x in context.tblUsers select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        private List<tblHospital> GetAllHospital()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblHospital> list = new List<tblHospital>();
                    list = (from x in context.tblHospitals select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        private List<tblDoctor> GetAllDoctor()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblDoctor> list = new List<tblDoctor>();
                    list = (from x in context.tblDoctors select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

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

        private List<tblMaintance> GetAllMaintance()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblMaintance> list = new List<tblMaintance>();
                    list = (from x in context.tblMaintances select x).ToList();
                    return list;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return null;
            }
        }

        private List<tblManager> GetAllManager()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblManager> list = new List<tblManager>();
                    list = (from x in context.tblManagers select x).ToList();
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
