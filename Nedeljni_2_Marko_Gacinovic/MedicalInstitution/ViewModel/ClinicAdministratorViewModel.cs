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

        private List<tblManager> managerList;
        public List<tblManager> ManagerList
        {
            get { return managerList; }
            set { managerList = value; OnPropertyChanged("ManagerList"); }
        }

        // constructor
        public ClinicAdministratorViewModel(ClinicAdministrator adminOpen, tblUser userToPass)
        {            
            clinicAdministrator = adminOpen;
            user = userToPass;

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
                AddNewMaintanceView addMaintance = new AddNewMaintanceView(user);
                addMaintance.ShowDialog();
                // updating the project list view
                if ((addMaintance.DataContext as AddNewMaintanceViewModel).IsUpdateMaintance == true)
                {
                    MaintanceList = GetAllMaintance();
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

                        context.tblMaintances.Remove(maintanceToDelete);
                        context.SaveChanges();

                        MaintanceList = GetAllMaintance();
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
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
            }
        }

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
                AddManagerView addManager = new AddManagerView(user);
                addManager.ShowDialog();
                // updating the project list view
                if ((addManager.DataContext as AddNewManagerViewModel).IsUpdateManager == true)
                {
                    ManagerList = GetAllManager().ToList();
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
    }
}
