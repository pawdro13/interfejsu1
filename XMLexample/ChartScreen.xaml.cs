using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using XMLexample.Common;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace XMLexample
{   
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ChartScreen : Page
    {
        List<RekordWykres> dataChart = new List<RekordWykres>();

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        private Waluta currentCurrency;
        public ChartScreen()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            currentCurrency = e.Parameter as Waluta;
            pageTitle.Text = "Historia kursu " + currentCurrency.KodWaluty;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private float ProccedWithXML(String xml_url)
        {
            XDocument loadedXML = XDocument.Load(xml_url);
            var data = from query in loadedXML.Descendants("pozycja")
                       select new RekordWykres
                       {
                           ExchangeRate = (float)query.Element("kurs_sredni")
                       };
           // return (float)data.ExchangeRate;
            foreach (String w in data)
            {
                return w;
            }
        }


        String responseBody;
        String[] splitted;
        private async Task GetDates()
        {
            var varDateStart = dateStart.Date;
            var varDateFinish = dateFinish.Date;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new System.Uri("http://www.nbp.pl/kursy/xml/dir.txt"));
            response.EnsureSuccessStatusCode();
            responseBody = await response.Content.ReadAsStringAsync();

            splitted = responseBody.Split('\n');
            for (int s = 0; s < splitted.Length; s++)   //the last one is empty
                splitted[s] = splitted[s].Trim('\r');

            for (int s = 0; s < splitted.Length - 1; s++)   //the last one is empty
            //for (int s = 0; s < 10; s++)   //the last one is empty
            {
                DateTime tmp = new DateTime(2000+System.Convert.ToInt32(splitted[s].Substring(5, 2)),System.Convert.ToInt32(splitted[s].Substring(7, 2)),System.Convert.ToInt32(splitted[s].Substring(9, 2)));

                if (!splitted[s].Substring(0, 1).Equals("a"))
                    continue;
                if (!(tmp.Date>varDateStart.Date && tmp.Date < varDateFinish.Date))
                    continue;
                string xml_url = @"http://www.nbp.pl/kursy/xml/" + splitted[s] + @".xml";

                RekordWykres tmpRekordWykres = new RekordWykres(tmp, ProccedWithXML(xml_url));
                dataChart.Add(tmpRekordWykres);
                //ProccedWithXML4(xml_url);
                //listBox_daty.Items.Add(ProccedWithXML4Date(xml_url));
                //listBox_daty.Items.Insert(0, "20" + splitted[s].Substring(5, 2) + "-" + splitted[s].Substring(7, 2) + "-" + splitted[s].Substring(9, 2));

            }

        }

        List<XMLexample.RekordWykres> records = new List<XMLexample.RekordWykres>();
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var varDateStart = dateStart.Date;
            var varDateFinish = dateFinish.Date;
            TimeSpan cnt;
            int curCnt;
            cnt = varDateFinish - varDateStart;
            progressOfLoad.Maximum = cnt.Days;
            await GetDates();

            await Task.Run(() =>
            {
               for(curCnt=0;curCnt<cnt.Days;curCnt++)
               {
                   
               }              
            });
            await Task.Run(() =>
            {


            });
           

        }
        

        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var varDateStart = dateStart.Date;
            var varDateFinish = dateFinish.Date;

            if (!(varDateFinish > varDateStart))
            {
                errorConsole.Text = "Data początkowa powinna być \nstarsza od końcowej";            
            }
            else
            {
                errorConsole.Text = "";
            }
        }

        private void dateStart_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            var varDateStart = dateStart.Date;
            var varDateFinish = dateFinish.Date;
            if (!(varDateFinish > varDateStart))
            {
                errorConsole.Text = "Data początkowa powinna być \nstarsza od końcowej";
            }
            else
            {
                errorConsole.Text = "";
            }

        }


        



    }
}
