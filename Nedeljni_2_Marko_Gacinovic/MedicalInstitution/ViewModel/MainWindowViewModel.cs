using MedicalInstitution.Commands;
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
            try
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
                    if (i==0)
                    {
                        username = list[1];
                    }
                    else if (i == 1)
                    {
                        password = list[1];
                    }
                }
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong inputs, please check the file credentials.");
                return false;
            }
        }
    }
}
