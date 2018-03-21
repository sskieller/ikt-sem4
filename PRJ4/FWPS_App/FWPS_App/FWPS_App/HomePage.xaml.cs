using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FWPS_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPage
	{
	    private LogIn _login = new LogIn("https://fwps.azurewebsites.net/api/login/");

	    private string Username = "";
	    private string Password = "";
		public HomePage ()
		{
			InitializeComponent();
            OnLoginButton.Clicked += OnLoginButton_Clicked;
		}

        private async void OnLoginButton_Clicked(object sender, EventArgs e)
        {
            loadingWheelTM.IsRunning = true;

            Username = usernameTextBox.Text ?? "";
            Password = passwordTextBox.Text ?? "";

            Task<bool> login = _login.Login(Username, Password);

            bool shouldLogin = await login;

            if (shouldLogin)
            {
                loadingWheelTM.IsRunning = false;
                await Navigation.PushAsync(new MainPage());
            }
            else
                usernameTextBox.Text = "Du hvad kammerat";

            loadingWheelTM.IsRunning = false;
        }
    }
}