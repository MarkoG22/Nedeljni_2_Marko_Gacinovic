﻿using MedicalInstitution.Models;
using MedicalInstitution.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MedicalInstitution.View
{
    /// <summary>
    /// Interaction logic for CreateHospitalView.xaml
    /// </summary>
    public partial class CreateHospitalView : Window
    {
        public CreateHospitalView(tblUser user)
        {
            InitializeComponent();
            this.DataContext = new CreateHospitalViewModel(this, user);
        }

        private void NumbersTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
