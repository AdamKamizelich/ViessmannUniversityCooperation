﻿using UniversityIot.UI.Core.Services;
using UniversityIot.UI.Core.ViewModels;
using Xamarin.Forms;

namespace UniversityIot.UI.Core.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            var credentialsService = new CredentialsService();

            BindingContext = new LoginViewModel(new UserManagementService(), credentialsService);
        }
    }
}