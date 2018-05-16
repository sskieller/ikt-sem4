using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FWPS_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private LogIn _login = new LogIn("https://fwps.azurewebsites.net/api/login/"); // Making a login Context to handle Login Requests

        private bool _loggingIn = false;

        public HomePage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            OnLoginButton.Clicked += OnLoginButton_Clicked;
        }

        private void OnLoginButton_Clicked(object sender, EventArgs e)
        {
            if (_loggingIn) return;
            loadingWheelTM.IsRunning = true;
            _loggingIn = true;

            Task.Run(() => LoginFunc()); // Making thread to handle login since it takes a whole
        }

        private void LoginFunc()
        {
            Thread.Sleep(1000); // CUZ OTHERWISE IT TOO FUCKIN' FAST

            string username = usernameTextBox.Text ?? "";
            string password = passwordTextBox.Text ?? "";

            if (_login.Login(username, password))
                Device.BeginInvokeOnMainThread(() =>
                {
                    _loggingIn = false;
                    loadingWheelTM.IsRunning = false;
                    Navigation.PushAsync(new MainPage() { Title = "Main Page" });
                    usernameTextBox.Text = "";
                    passwordTextBox.Text = "";
                });
            else
                Device.BeginInvokeOnMainThread(() =>
                {
                    DisplayAlert("Login", "Wrong username or password", "OK");
                    usernameTextBox.Text = "";
                    passwordTextBox.Text = "";
                    _loggingIn = false;
                    loadingWheelTM.IsRunning = false;
                });
        }
    }
}