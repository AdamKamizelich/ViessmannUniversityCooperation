﻿namespace UniversityIot.UI.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.LoadApplication(new Core.App());
        }
    }
}