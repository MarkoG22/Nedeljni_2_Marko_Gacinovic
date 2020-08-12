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


        // constructor
        public MainWindowViewModel(MainWindow mainOpen)
        {            
            main = mainOpen;
        }

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
            else if (IsAdmin(username,UserPassword))
            {
                ClinicAdministrator admin = new ClinicAdministrator();
                if (FirstLogin(username,userPassword))
                {
                    CreateHospitalView hospital = new CreateHospitalView(user);
                    hospital.ShowDialog();
                }
                
                admin.ShowDialog();
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
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception" + ex.Message.ToString());
                return false;
            }
        }
    }
}
