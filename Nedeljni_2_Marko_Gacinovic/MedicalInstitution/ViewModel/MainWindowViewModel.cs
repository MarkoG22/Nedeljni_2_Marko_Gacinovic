using MedicalInstitution.Commands;
using MedicalInstitution.Models;
using MedicalInstitution.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MedicalInstitution.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        MainWindow main;

        #region Properties
        // properties for username and password
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }

        private string userPassword;
        public string UserPassword
        {
            get { return userPassword; }
            set
            {
                userPassword = value;
                OnPropertyChanged("UserPassword");
            }
        }

        private tblUser user;
        public tblUser User
        {
            get { return user; }
            set { user = value; }
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


        private tblDoctor doctor;
        public tblDoctor Doctor
        {
            get { return doctor; }
            set { doctor = value; OnPropertyChanged("Doctor"); }
        }

        private tblPatient patient;
        public tblPatient Patient
        {
            get { return patient; }
            set { patient = value; OnPropertyChanged("Patient"); }
        }
        #endregion

        // constructor
        public MainWindowViewModel(MainWindow mainOpen)
        {            
            main = mainOpen;
        }

        #region Commands
        // command for the login button
        private ICommand logIn;
        public ICommand LogIn
        {
            get
            {
                if (logIn == null)
                {
                    logIn = new RelayCommand(param => SaveExecute(), param => CanSaveExecute());
                }
                return logIn;
            }
        }

        private bool CanSaveExecute()
        {
            return true;
        }

        /// <summary>
        /// method for checking username and password and opening the windows
        /// </summary>
        private void SaveExecute()
        {  
            if (IsMaster(username, userPassword))
            {
                MasterView master = new MasterView();
                master.ShowDialog();
            }
            else if (IsManager(username, userPassword))
            {
                AddDoctorView addManager = new AddDoctorView(vwManager);
                addManager.ShowDialog();
            }
            else if (IsDoctor(username, userPassword))
            {
                DoctorView doctorView = new DoctorView(doctor);
                doctorView.ShowDialog();
            }
            else if (IsAdmin(username,UserPassword))
            {
                try
                {
                    using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                    {
                        user = (from x in context.tblUsers where x.Username == username && x.Pasword == userPassword select x).First();
                        
                        if (user.LoggedIn != true)
                        {
                            CreateHospitalView hospital = new CreateHospitalView(user);
                            hospital.ShowDialog();
                            user.LoggedIn = true;
                        }
                        ClinicAdministrator admin = new ClinicAdministrator(user, manager, doctor, patient);
                        admin.ShowDialog();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                }
            }            
            else
            {
                MessageBox.Show("Wrong username or password, please try again.");
            }
        }

        // command for closing the window
        private ICommand logout;
        public ICommand Logout
        {
            get
            {
                if (logout == null)
                {
                    logout = new RelayCommand(param => CloseExecute(), param => CanCloseExecute());
                }
                return logout;
            }
        }

        /// <summary>
        /// method for closing the window
        /// </summary>
        private void CloseExecute()
        {
            try
            {
                main.Close();
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
        #endregion

        #region Methods
        private bool IsMaster(string username, string password)
        {
            string[] lines = File.ReadAllLines(@"../../ClinicAccess.txt");
            List<string> list = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    continue;
                }
                list = lines[i].Split(':').ToList();
                if (username == list[1] || password == list[1])
                {
                    return true;                    
                }                
            }
            return false;
        }

        private bool IsAdmin(string username, string password)
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    tblUser user = (from x in context.tblUsers where x.Username == username && x.Pasword == password select x).First();
                    bool isManager = user.Manager;
                    return isManager;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return false;
            }
        }

        private bool FirstLogin(string username, string password)
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    tblUser user = (from x in context.tblUsers where x.Username == username && x.Pasword == password select x).First();
                    if (user.LoggedIn == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return false;
            }
        }

        private bool IsManager(string username, string password)
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    tblUser user = (from x in context.tblUsers where x.Username == username && x.Pasword == password select x).First();
                    tblManager manager = (from y in context.tblManagers where y.UserID == user.UserId select y).First();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return false;
            }
        }

        private bool IsDoctor(string username, string password)
        {
            try
            {
                using (MedicalInstitutionEntities context = new MedicalInstitutionEntities())
                {
                    tblUser user = (from x in context.tblUsers where x.Username == username && x.Pasword == password select x).First();
                    tblDoctor doctor = (from y in context.tblDoctors where y.UserID == user.UserId select y).First();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return false;
            }
        }
        #endregion
    }
}
