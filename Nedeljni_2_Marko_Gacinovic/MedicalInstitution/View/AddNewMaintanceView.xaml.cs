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
    /// Interaction logic for AddNewMaintanceView.xaml
    /// </summary>
    public partial class AddNewMaintanceView : Window
    {
        public AddNewMaintanceView()
        {
            InitializeComponent();
            this.DataContext = new AddNewMaintanceViewModel(this);
        }

        private void LettersValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^a-zA-Z ]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
