
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entry = Microcharts.Entry;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using Microcharts;
using Newtonsoft.Json;

namespace FWPS_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    /////////////////////////////////////////////////
    /// Content Page for MorningSun statistics
    /////////////////////////////////////////////////
    public partial class ShowLightStatisticsPage : ContentPage
    {

        /////////////////////////////////////////////////
        /// Initialisations 
        /////////////////////////////////////////////////
        public ShowLightStatisticsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MakePlot();
            ReturnBtn.Clicked += ReturnBtn_Clicked;
        }

        /////////////////////////////////////////////////
        /// Make plot for statistics displaying 8 
        /// coloums, 7 for recent days and 1 for total
        /////////////////////////////////////////////////
        public void MakePlot()
        {
            // Last seven days in specific format. Is to be used in labels in chart.
            DateTime[] last7Days = Enumerable.Range(0, 7).Select(i => DateTime.Now.Date.AddDays(-i)).ToArray();
            string day1 = last7Days[6].ToString("yyyy-MM-dd");
            string day2 = last7Days[5].ToString("yyyy-MM-dd");
            string day3 = last7Days[4].ToString("yyyy-MM-dd");
            string day4 = last7Days[3].ToString("yyyy-MM-dd");
            string day5 = last7Days[2].ToString("yyyy-MM-dd");
            string day6 = last7Days[1].ToString("yyyy-MM-dd");
            string day7 = last7Days[0].ToString("yyyy-MM-dd");

            // Get objects from recent seven days
            string obj = HTTPRequestHandler.CreateGetRequest("http://fwps.azurewebsites.net/api/light/getupdate").ToString();

            var lightObject = JsonConvert.DeserializeObject<List<LightPage.LightObject>>(obj);

            int day1Count = lightObject.Count(spo => spo.CreatedDate.Date == last7Days[6].Date && spo.IsOn == true);
            int day2Count = lightObject.Count(spo => spo.CreatedDate.Date == last7Days[5].Date && spo.IsOn == true);
            int day3Count = lightObject.Count(spo => spo.CreatedDate.Date == last7Days[4].Date && spo.IsOn == true);
            int day4Count = lightObject.Count(spo => spo.CreatedDate.Date == last7Days[3].Date && spo.IsOn == true);
            int day5Count = lightObject.Count(spo => spo.CreatedDate.Date == last7Days[2].Date && spo.IsOn == true);
            int day6Count = lightObject.Count(spo => spo.CreatedDate.Date == last7Days[1].Date && spo.IsOn == true);
            int day7Count = lightObject.Count(spo => spo.CreatedDate.Date == last7Days[0].Date && spo.IsOn == true);

            int total = day1Count + day2Count + day3Count + day4Count + day5Count + day6Count + day7Count;

            var entries = new[]
            {
            new Entry(day1Count)
            {
                Label = day1,
                ValueLabel = day1Count.ToString(),
                Color = SKColor.Parse("#b455b6")
            },
            new Entry(day2Count)
            {
                Label = day2,
                ValueLabel = day2Count.ToString(),
                Color = SKColor.Parse("#b455b6")
            },
            new Entry(day3Count)
            {
                Label = day3,
                ValueLabel = day3Count.ToString(),
                Color = SKColor.Parse("#b455b6")
            },
            new Entry(day4Count)
            {
                Label = day4,
                ValueLabel = day4Count.ToString(),
                Color = SKColor.Parse("#b455b6")

            },
            new Entry(day5Count)
            {
                Label = day5,
                ValueLabel = day5Count.ToString(),
                Color = SKColor.Parse("#b455b6")
            },
            new Entry(day6Count)
            {
                Label = day6,
                ValueLabel = day6Count.ToString(),
                Color = SKColor.Parse("#b455b6")
            },
            new Entry(day7Count)
            {
                Label = day7,
                ValueLabel = day7Count.ToString(),
                Color = SKColor.Parse("#b455b6")
            },
            new Entry(total)
            {
                Label = "Total",
                ValueLabel = total.ToString(),
                Color = SKColor.Parse("#3498db")}
            };


            var chart = new BarChart() { Entries = entries };

            chartView.Chart = chart;
        }

        /////////////////////////////////////////////////
        /// Button event handler that redirects to
        /// MorningSun page
        /////////////////////////////////////////////////
        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}