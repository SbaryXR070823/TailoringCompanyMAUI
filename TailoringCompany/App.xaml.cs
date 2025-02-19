﻿namespace TailoringCompany
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}