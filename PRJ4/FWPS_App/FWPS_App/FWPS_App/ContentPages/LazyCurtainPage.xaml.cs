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

    /////////////////////////////////////////////////
    /// Content Page for LazyCurtain 
    /// TODO: implement the system
    /////////////////////////////////////////////////
    public partial class LazyCurtainPage : ContentPage
	{
        /////////////////////////////////////////////////
        /// Initialisations 
        /////////////////////////////////////////////////
        public LazyCurtainPage ()
		{
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            ReturnBtn.Clicked += ReturnBtn_Clicked;
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// Mainpage which shows content for 
        /// mainpage
        /////////////////////////////////////////////////
        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}