﻿using XMLexample.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml.Linq;
using System.Net.Http;
using System.Threading.Tasks;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace XMLexample
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int i;
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


        public MainPage()
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        String responseBody;
        String[] splitted;
        private async Task GetDates()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new System.Uri("http://www.nbp.pl/kursy/xml/dir.txt"));
            response.EnsureSuccessStatusCode();
            responseBody = await response.Content.ReadAsStringAsync();
            
            splitted = responseBody.Split('\n');
            for (int s = 0; s < splitted.Length; s++)   //the last one is empty
                splitted[s] = splitted[s].Trim('\r');

            for (int s = 0; s < splitted.Length-1; s++)   //the last one is empty
            //for (int s = 0; s < 10; s++)   //the last one is empty
            {
                if (!splitted[s].Substring(0, 1).Equals("a"))
                    continue;
                string xml_url = @"http://www.nbp.pl/kursy/xml/" + splitted[s] + @".xml";
                //ProccedWithXML4(xml_url);
                //listBox_daty.Items.Add(ProccedWithXML4Date(xml_url));
                listBox_daty.Items.Insert(0,"20" + splitted[s].Substring(5, 2) + "-" + splitted[s].Substring(7, 2) + "-" + splitted[s].Substring(9,2));

            }
            
        }

        private async void pageRoot_Loaded(object sender, RoutedEventArgs e)
        {           
            // Restore values stored in app data.
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Containers.ContainsKey("exchangeRates"))
            {
                for (int i = 0; ; i++)
                {
                    if (i<35)
                    {
                        XMLexample.Waluta temp = new Waluta();
                        Windows.Storage.ApplicationDataCompositeValue tempComposite =
                            (Windows.Storage.ApplicationDataCompositeValue)localSettings.Containers["exchangeRates"].Values[i.ToString()];
                        if (tempComposite != null)
                        {
                            if (tempComposite["KodWaluty"]!=null)
                            temp.KodWaluty= (string)tempComposite["KodWaluty"];
                            if (tempComposite["KursSredni"] != null)
                            temp.KursSredni = (string)tempComposite["KursSredni"];
                            if (tempComposite["NazwaKraju"] != null)
                            temp.NazwaKraju = (string)tempComposite["NazwaKraju"];
                            if (tempComposite["Przelicznik"] != null)
                            temp.Przelicznik = (string)tempComposite["Przelicznik"];
                            if (tempComposite["NazwaWaluty"] != null)
                            temp.NazwaWaluty = (string)tempComposite["NazwaWaluty"];
                            listBox_waluty.Items.Add(temp);
                        }
                    }
                    else{
                        break;
                    }
                }
            }
            else
            {
                myTextBlock.Text = "Brak danych do wyświetlenia";
            }
        }
       
        private string ProccedWithXML4Date(String xml_url)
        {
            XDocument loadedXML = XDocument.Load(xml_url);
            return (string)loadedXML.Descendants("tabela_kursow").ElementAt(0).Element("data_publikacji");
        }

        private void ProccedWithXML(String xml_url)
        {
            XDocument loadedXML = XDocument.Load(xml_url);
            myTextBlock.Text = "Data publikacji: " + (string)loadedXML.Descendants("tabela_kursow").ElementAt(0).Element("data_publikacji");
            var data = from query in loadedXML.Descendants("pozycja")
                       select new Waluta
                       {
                           KursSredni = (string)query.Element("kurs_sredni"),
                           NazwaWaluty = ((string)query.Element("nazwa_waluty")) == null ? (string)query.Element("nazwa_kraju") : (string)query.Element("nazwa_waluty"),
                           KodWaluty = (string)query.Element("kod_waluty")
                       };
            listBox_waluty.ItemsSource = data;
        }



        private void listBox_daty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tmpS = (string)listBox_daty.SelectedItem;            
            tmpS = tmpS.Substring(2,2)+tmpS.Substring(5,2)+tmpS.Substring(8,2);
            foreach (string ss in splitted)
            {
                if (!ss.Substring(0, 1).Equals("a"))
                    continue;
                if (ss.Substring(5,6).Equals(tmpS)) //a002z020103
                {
                    tmpS = ss;
                    break;
                }
            }
            ProccedWithXML( @"http://www.nbp.pl/kursy/xml/" + tmpS + @".xml");
        }

        private void pageTitle_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
            
        }

        private void CloseApplicationButton1_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void UpdateDataButton_Click(object sender, RoutedEventArgs e)
        {
            await GetDates();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (!localSettings.Containers.ContainsKey("exchangeRates"))
            {
                localSettings.CreateContainer("exchangeRates", Windows.Storage.ApplicationDataCreateDisposition.Always);
            }
           
            
            for (int i = 0; i < listBox_waluty.Items.Count(); i++)
            {
                
                XMLexample.Waluta temp = (XMLexample.Waluta)listBox_waluty.Items[i];
                Windows.Storage.ApplicationDataCompositeValue tempComp = new Windows.Storage.ApplicationDataCompositeValue();
                if (temp.KodWaluty!=null)
                tempComp["KodWaluty"] = temp.KodWaluty;
                if (temp.KursSredni != null) 
                tempComp["KursSredni"] = temp.KursSredni;
                if (temp.NazwaKraju != null) 
                tempComp["NazwaKraju"] = temp.NazwaKraju.ToString();
                if (temp.Przelicznik != null) 
                tempComp["Przelicznik"] = temp.Przelicznik.ToString();
                if (temp.NazwaWaluty != null)
                tempComp["NazwaWaluty"] = temp.NazwaWaluty.ToString();
                localSettings.Containers["exchangeRates"].Values[i.ToString()]=tempComp;
            }
           

        }

        private void listBox_waluty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(ChartScreen), e.AddedItems[0]);
                
            }
        }



    }
}
