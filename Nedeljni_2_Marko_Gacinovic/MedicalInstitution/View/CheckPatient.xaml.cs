using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for CheckPatient.xaml
    /// </summary>
    public partial class CheckPatient : Window
    {
        static BackgroundWorker bw = new BackgroundWorker();
        static Random rnd = new Random();
        int num { get; set; }

        public CheckPatient()
        {
            InitializeComponent();
            bw.DoWork += bw_DoWork;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
        }      
        
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            // variables for calculating progress bar percentage
            int sum = 0;
            int percentage;

            for (int i = 1; i <= 5; i++)
            {
                Thread.Sleep(1000);
                percentage = 100 / 5;
                sum = sum + percentage;
                bw.ReportProgress(sum);

                if (bw.CancellationPending)
                {
                    // Set Cancel property of DoWorkEventArgs object to true
                    e.Cancel = true;
                    // Reset progress percentage to ZERO and return
                    bw.ReportProgress(0);
                    return;
                }                
            }

            num = rnd.Next(0,2);

            if (num==0)
            {
                MessageBox.Show("The patient is virus positive.");
            }
            else
            {
                MessageBox.Show("The patient is virus negative.");
            }

            e.Result = sum;
        }

        /// <summary>
        /// method for displaying progress bar work
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            Percentage.Content = e.ProgressPercentage.ToString() + "%";
        }

        /// <summary>
        /// method for displaying result message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Message.Content = "Processing cancelled";
            }
            else if (e.Error != null)
            {
                Message.Content = e.Error.Message;
            }
            else
            {
                Message.Content = e.Result.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!bw.IsBusy)
            {

                bw.RunWorkerAsync();
            }
            else
            {
                Message.Content = "Checking is busy, please wait.";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (bw.IsBusy)
            {
                bw.CancelAsync();
            }
        }
    }
}
