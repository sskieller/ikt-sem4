
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
    public partial class ShowStatisticsPage : ContentPage
    {

        public ShowStatisticsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            MakePlot();
            ReturnBtn.Clicked += ReturnBtn_Clicked;
        }

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
            string obj = HTTPRequestHandler.CreateGetRequest("http://fwps.azurewebsites.net/api/snapbox/getupdate").ToString();

            var snapboxObject = JsonConvert.DeserializeObject<List<SnapboxPage.SnapboxObject>>(obj);
            
            int day1Count = snapboxObject.Count(spo => spo.CreatedDate.Date == last7Days[6].Date && spo.MailReceived == false);
            int day2Count = snapboxObject.Count(spo => spo.CreatedDate.Date == last7Days[5].Date && spo.MailReceived == false);
            int day3Count = snapboxObject.Count(spo => spo.CreatedDate.Date == last7Days[4].Date && spo.MailReceived == false);
            int day4Count = snapboxObject.Count(spo => spo.CreatedDate.Date == last7Days[3].Date && spo.MailReceived == false);
            int day5Count = snapboxObject.Count(spo => spo.CreatedDate.Date == last7Days[2].Date && spo.MailReceived == false);
            int day6Count = snapboxObject.Count(spo => spo.CreatedDate.Date == last7Days[1].Date && spo.MailReceived == false);
            int day7Count = snapboxObject.Count(spo => spo.CreatedDate.Date == last7Days[0].Date && spo.MailReceived == false);

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

        private void ReturnBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}