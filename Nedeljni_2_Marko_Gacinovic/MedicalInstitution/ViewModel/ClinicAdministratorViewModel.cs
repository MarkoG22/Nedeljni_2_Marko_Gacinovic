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

        #region Properties
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

        private Queue<tblMaintance> maintanceList;
        public Queue<tblMaintance> MaintanceList
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

        private vwManager vwManager;
        public vwManager VwManager
        {
            get { return vwManager; }
            set { vwManager = value; }
        }


        private List<tblManager> managerList;
        public List<tblManager> ManagerList
        {
            get { return managerList; }
            set { managerList = value; OnPropertyChanged("ManagerList"); }
        }       
        #endregion

        // constructor
        public ClinicAdministratorViewModel(ClinicAdministrator adminOpen, tblUser userToPass, tblManager managerToPass, tblDoctor doctorToPass, tblPatient patientToPass)
        {            
            clinicAdministrator = adminOpen;
            user = userToPass;
            manager = managerToPass;
            doctor = doctorToPass;
            patient = patientToPass;

            UserList = GetAllUser();
            HospitalList = GetAllHospital();
            DoctorList = GetAllDoctor();
            PatientList = GetAllPatient();
            MaintanceList = GetAllMaintance();
            ManagerList = GetAllManager();
        }

        #region Patient
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
                    PatientList = GetAllPatient();
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
                    // checking the action
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the user?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        tblUser userToDelete = (from y in context.tblUsers where y.UserId == patient.UserID select y).First();
                        tblPatient patientToDelete = (from x in context.tblPatients where x.PatientID == patient.PatientID select x).First();

                        context.tblPatients.Remove(patientToDelete);
                        context.tblUsers.Remove(userToDelete);
                        context.SaveChanges();

                        PatientList = GetAllPatient();
                        UserList = GetAllUser();

                        FileActions.FileActions.Instance.Deleting(FileActions.FileActions.path, FileActions.FileActions.actions, "user", userToDelete.FullName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, the patient can not be deleted.");
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
                EditUserView editUser = new EditUserView(user, patient);
                editUser.ShowDialog();
                if ((editUser.DataContext as EditUserViewModel).IsUpdateUser == true)
                {
                    PatientList = GetAllPatient();
                    UserList = GetAllUser().ToList();                    
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        #endregion

        #region Hospital
        // command for editing the user
        private ICommand editHospital;
        public ICommand EditHospital
        {
            get
            {
                if (editHospital == null)
                {
                    editHospital = new RelayCommand(param => EditHospitalExecute(), param => CanEditHospitalExecute());
                }
                return editHospital;
            }
        }

        private bool CanEditHospitalExecute()
        {
            return true;
        }

        /// <summary>
        /// method for opening the view for the editing user
        /// </summary>
        private void EditHospitalExecute()
        {
            try
            {
                EditHospitalView editHospital = new EditHospitalView(hospital);
                editHospital.ShowDialog();
                if ((editHospital.DataContext as EditHospitalViewModel).IsUpdateHospital == true)
                {
                    HospitalList = GetAllHospital().ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }
        #endregion

        #region Maintance
        private ICommand addNewMaintance;
        public ICommand AddNewMaintance
        {
            get
            {
                if (addNewMaintance == null)
                {
                    addNewMaintance = new RelayCommand(param => AddNewMaintanceExecute(), param => CanAddNewMaintanceExecute());
                }
                return addNewMaintance;
            }
        }

        private bool CanAddNewMaintanceExecute()
        {
            return true;
        }

        private void AddNewMaintanceExecute()
        {
            try
            {
                AddNewMaintanceView addMaintance = new AddNewMaintanceView();
                addMaintance.ShowDialog();
                // updating the project list view
                if ((addMaintance.DataContext as AddNewMaintanceViewModel).IsUpdateMaintance == true)
                {
                    MaintanceList = GetAllMaintance();
                    UserList = GetAllUser();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private ICommand deleteMaintance;
        public ICommand DeleteMaintance
        {
            get
            {
                if (deleteMaintance == null)
                {
                    deleteMaintance = new RelayCommand(param => DeleteMaintanceExecute(), param => CanDeleteMaintanceExecute());
                }
                return deleteMaintance;
            }

        }

        private bool CanDeleteMaintanceExecute()
        {
            return true;
        }

        private void DeleteMaintanceExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    int id = maintance.MaintanceID;

                    // checking the action
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the maintance?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        tblMaintance maintanceToDelete = (from x in context.tblMaintances where x.MaintanceID == id select x).First();
                        tblUser userToDelete = (from y in context.tblUsers where y.UserId == maintance.UserID select y).First();

                        
                        context.tblMaintances.Remove(maintanceToDelete);
                        context.tblUsers.Remove(userToDelete);
                        context.SaveChanges();

                        MaintanceList = GetAllMaintance();
                        userList = GetAllUser();

                        FileActions.FileActions.Instance.Deleting(FileActions.FileActions.path, FileActions.FileActions.actions, "maintance", userToDelete.FullName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, the maintance can not be deleted.");
            }
        }

        // command for editing the user
        private ICommand editMaintance;
        public ICommand EditMaintance
        {
            get
            {
                if (editMaintance == null)
                {
                    editMaintance = new RelayCommand(param => EditMaintanceExecute(), param => CanEditMaintanceExecute());
                }
                return editMaintance;
            }
        }

        private bool CanEditMaintanceExecute()
        {
            return true;
        }

        /// <summary>
        /// method for opening the view for the editing user
        /// </summary>
        private void EditMaintanceExecute()
        {
            try
            {
                EditMaintanceView editMaintance = new EditMaintanceView(maintance);
                editMaintance.ShowDialog();
                if ((editMaintance.DataContext as EditMaintanceViewModel).IsUpdateMaintance == true)
                {
                    MaintanceList = GetAllMaintance();
                    userList = GetAllUser();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }
        #endregion

        #region Manager
        private ICommand addNewManager;
        public ICommand AddNewManager
        {
            get
            {
                if (addNewManager == null)
                {
                    addNewManager = new RelayCommand(param => AddNewManagerExecute(), param => CanAddNewManagerExecute());
                }
                return addNewManager;
            }
        }

        private bool CanAddNewManagerExecute()
        {
            return true;
        }

        private void AddNewManagerExecute()
        {
            try
            {
                AddManagerView addManager = new AddManagerView();
                addManager.ShowDialog();
                // updating the project list view
                if ((addManager.DataContext as AddNewManagerViewModel).IsUpdateManager == true)
                {
                    ManagerList = GetAllManager().ToList();
                    UserList = GetAllUser();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private ICommand deleteManager;
        public ICommand DeleteManager
        {
            get
            {
                if (deleteManager == null)
                {
                    deleteManager = new RelayCommand(param => DeleteManagerExecute(), param => CanDeleteManagerExecute());
                }
                return deleteManager;
            }

        }

        private bool CanDeleteManagerExecute()
        {
            return true;
        }

        private void DeleteManagerExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {                    
                    // checking the action
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the manager?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        tblUser userToDelete = (from y in context.tblUsers where y.UserId == manager.UserID select y).First();
                        tblManager managerToDelete = (from x in context.tblManagers where x.ManagerID == manager.ManagerID select x).First();

                        context.tblManagers.Remove(managerToDelete);
                        context.tblUsers.Remove(userToDelete);
                        context.SaveChanges();

                        ManagerList = GetAllManager();
                        UserList = GetAllUser();

                        FileActions.FileActions.Instance.Deleting(FileActions.FileActions.path, FileActions.FileActions.actions, "manager", userToDelete.FullName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, the manager can not be deleted.");
            }
        }

        // command for editing the user
        private ICommand editManager;
        public ICommand EditManager
        {
            get
            {
                if (editManager == null)
                {
                    editManager = new RelayCommand(param => EditManagerExecute(), param => CanEditManagerExecute());
                }
                return editManager;
            }
        }

        private bool CanEditManagerExecute()
        {
            return true;
        }

        /// <summary>
        /// method for opening the view for the editing user
        /// </summary>
        private void EditManagerExecute()
        {
            try
            {
                EditManagerView editManager = new EditManagerView(manager);
                editManager.ShowDialog();
                if ((editManager.DataContext as EditManagerViewModel).IsUpdateManager == true)
                {
                    ManagerList = GetAllManager();
                    UserList = GetAllUser();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }
        #endregion

        #region Doctor
        private ICommand addNewDoctor;
        public ICommand AddNewDoctor
        {
            get
            {
                if (addNewDoctor == null)
                {
                    addNewDoctor = new RelayCommand(param => AddNewDoctorExecute(), param => CanAddNewDoctorExecute());
                }
                return addNewDoctor;
            }
        }

        private bool CanAddNewDoctorExecute()
        {
            return true;
        }

        private void AddNewDoctorExecute()
        {
            try
            {
                AddDoctorView addDoctor = new AddDoctorView(vwManager);
                addDoctor.ShowDialog();
                // updating the project list view
                if ((addDoctor.DataContext as AddDoctorViewModel).IsUpdateDoctor == true)
                {
                    DoctorList = GetAllDoctor().ToList();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

        private ICommand deleteDoctor;
        public ICommand DeleteDoctor
        {
            get
            {
                if (deleteDoctor == null)
                {
                    deleteDoctor = new RelayCommand(param => DeleteDoctorExecute(), param => CanDeleteDoctorExecute());
                }
                return deleteDoctor;
            }

        }

        private bool CanDeleteDoctorExecute()
        {
            return true;
        }

        private void DeleteDoctorExecute()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {       
                    // checking the action
                    MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete the doctor?", "Delete Confirmation", MessageBoxButton.YesNo);

                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        tblUser userToDelete = (from y in context.tblUsers where y.UserId == doctor.UserID select y).First();
                        tblDoctor doctorToDelete = (from x in context.tblDoctors where x.DoctorID == doctor.DoctorID select x).First();
                                                
                        context.tblDoctors.Remove(doctorToDelete);
                        context.tblUsers.Remove(userToDelete);
                        context.SaveChanges();

                        DoctorList = GetAllDoctor();
                        UserList = GetAllUser();

                        FileActions.FileActions.Instance.Deleting(FileActions.FileActions.path, FileActions.FileActions.actions, "doctor", userToDelete.FullName);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Sorry, the doctor can not be deleted.");
            }
        }

        // command for editing the user
        private ICommand editDoctor;
        public ICommand EditDoctor
        {
            get
            {
                if (editDoctor == null)
                {
                    editDoctor = new RelayCommand(param => EditDoctorExecute(), param => CanEditDoctorExecute());
                }
                return editDoctor;
            }
        }

        private bool CanEditDoctorExecute()
        {
            return true;
        }

        /// <summary>
        /// method for opening the view for the editing user
        /// </summary>
        private void EditDoctorExecute()
        {
            try
            {
                EditDoctorView editDoctor = new EditDoctorView(doctor, user);
                editDoctor.ShowDialog();
                if ((editDoctor.DataContext as EditDoctorViewModel).IsUpdateDoctor == true)
                {
                    DoctorList = GetAllDoctor().ToList();
                    UserList = GetAllUser();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }
        #endregion

        #region Methods
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

        private Queue<tblMaintance> GetAllMaintance()
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    List<tblMaintance> list = new List<tblMaintance>();
                    Queue<tblMaintance> q = new Queue<tblMaintance>();
                    list = (from x in context.tblMaintances select x).ToList();                    

                    for (int i = 0; i < list.Count; i++)
                    {
                        if (i>2)
                        {
                            q.Dequeue();                            
                        }
                        q.Enqueue(list[i]);
                    }
                    
                    return q;
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
        #endregion
    }
}
