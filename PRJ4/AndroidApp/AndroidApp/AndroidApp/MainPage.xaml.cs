using System;
using Xamarin.Forms;

namespace FWPS
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            LoginButton.Clicked += LoginButton_Clicked; ;
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page_Climate());
        }
    }
}
