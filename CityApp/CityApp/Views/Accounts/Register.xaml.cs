﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using CityApp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace CityApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        private RegisterViewModel _vm;
        private bool _correctCredentials;
        public bool CorrectCredentials
        {
            get { return _correctCredentials; }
            set { _correctCredentials = value;
                Bindings.Update();
            }
        }
        private string _errorText;

        public string ErrorText
        {
            get { return _errorText; }
            set { _errorText = value;
                Bindings.Update();
            }
        }


        public Register()
        {
            this.InitializeComponent();
            _vm = new RegisterViewModel();
            this.DataContext = _vm;
            CorrectCredentials = false;
            ErrorText = "Please fill in all fields.";
        }

        private void BirthDate_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if (BirthDate.Date > DateTimeOffset.Now)
            {
                CorrectCredentials = false;
            }
            else {
                CorrectCredentials = true;
            }
        }
        private void Credentials_Changed(object sender, TextChangedEventArgs e) {
            if (String.IsNullOrEmpty(Name.Text) || String.IsNullOrEmpty(FirstName.Text) || String.IsNullOrEmpty(Email.Text) || String.IsNullOrEmpty(Password.Text) || String.IsNullOrEmpty(Username.Text))
            {
                ErrorText = "Please fill in all fields.";
                CorrectCredentials = false;
            }
            else {
                ErrorText = "";
                CorrectCredentials = true;
            }
        }
    }
}
