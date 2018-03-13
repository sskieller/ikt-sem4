using System;
using System.Collections.Generic;
using System.Linq;
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

	    public string Username = "";
	    public string Password = "";
		public HomePage ()
		{
			InitializeComponent ();
            OnLoginButton.Clicked += OnLoginButton_Clicked;
		}

        private void OnLoginButton_Clicked(object sender, EventArgs e)
        {
            Username = usernameTextBox.Text ?? "";
            Password = passwordTextBox.Text ?? "";

            if (_login.Login(Username, Password))
                Navigation.PushAsync(new MainPage());
            else
                usernameTextBox.Text = "Du hvad kammerat";
        }
    }
}