using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input.Inking;
using XMLexample.Common;
using Windows.Graphics.Imaging;
using Windows.UI;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace XMLexample
{   
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ChartScreen : Page
    {
        List<RekordWykres> dataChart = new List<RekordWykres>();
        //do wykresu
        InkManager _inkManager = new Windows.UI.Input.Inking.InkManager();
        int leftDrawingMargin = 40;
        int rightDrawingMargin = 20;
        int topDrawingMargin = 20;
        int bottomDrawingMargin = 40;

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

        private string formatDate(DateTime _time)
        {
            string temp;
            temp = _time.Date.Day.ToString();
            temp = temp +"."+ _time.Date.Month.ToString();
            temp = temp + "." + _time.Date.Year.ToString();
            return temp;
        }
        private string ProccedWithXML(String xml_url)
        {            
            //ładuje dokument xml
            XDocument loadedXML = XDocument.Load(xml_url);
            //textbox info
            //myTextBlock.Text = "Data publikacji: " + (string)loadedXML.Descendants("tabela_kursow").ElementAt(0).Element("data_publikacji");
            string SYMBOL_WALUTY = "USD";
            //W liście bedzie tylko 1 obiekt string który będzie posiadał wartość waluty z danego dnia
            var data = from query in loadedXML.Descendants("pozycja")
                       where (string)query.Element("kod_waluty") == SYMBOL_WALUTY
                       select (string)query.Element("kurs_sredni");
            foreach (String w in data)
            {
                return w;
            }
            return "ERROR";
        }


        String responseBody;
        String[] splitted;
        
        private async Task GetDates()
        {
            var varDateStart = dateStart.Date;
            var varDateFinish = dateFinish.Date;
            dataChart.Clear();
            progressOfLoad.ClearValue(ProgressBar.ValueProperty);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(new System.Uri("http://www.nbp.pl/kursy/xml/dir.txt"));
            CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            response.EnsureSuccessStatusCode();
            responseBody = await response.Content.ReadAsStringAsync();

            splitted = responseBody.Split('\n');
            for (int s = 0; s < splitted.Length; s++)   //the last one is empty
                splitted[s] = splitted[s].Trim('\r');
            TimeSpan maxDay = dateFinish.Date - dateStart.Date;
            int cntMaxDay = maxDay.Days;
            progressOfLoad.Maximum = maxDay.Days;
            for (int s = splitted.Length - 2; s>=0; s--)   //the last one is empty
            {

                if (cntMaxDay != 0)
                { 
                DateTime tmp = new DateTime(2000 + System.Convert.ToInt32(splitted[s].Substring(5, 2)), System.Convert.ToInt32(splitted[s].Substring(7, 2)), System.Convert.ToInt32(splitted[s].Substring(9, 2)));
                if (tmp.Date < varDateStart.Date)
                {
                    DateTime now = DateTime.Now;
                    progressOfLoad.Value = progressOfLoad.Maximum;
                    errorConsole.Text = errorConsole.Text + "\n" + now.ToString() + ": Pobieranie danych zakończone ";
                    break;
                }
                if (!splitted[s].Substring(0, 1).Equals("a"))
                    continue;
                if (!(tmp.Date >= varDateStart.Date && tmp.Date <= varDateFinish.Date))
                    continue;
                string xml_url = @"http://www.nbp.pl/kursy/xml/" + splitted[s] + @".xml";
                RekordWykres tmpRekordWykres = new RekordWykres(tmp, ProccedWithXML(xml_url));
                dataChart.Add(tmpRekordWykres);
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    DateTime now = DateTime.Now;
                    progressOfLoad.Value += 1;
                    errorConsole.Text = errorConsole.Text + "\n" + now.ToString() + ": Pobrany rekord z dnia  " + formatDate(tmp);

                });
                cntMaxDay -= 1;
                }
                else
                {
                    DateTime now = DateTime.Now;
                    progressOfLoad.Value = progressOfLoad.Maximum;
                    errorConsole.Text = errorConsole.Text + "\n" + now.ToString() + ": Pobieranie danych zakończone ";
                    dataChart.Reverse();
                    break;
                }



            }

        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            LoadHistory.IsEnabled = false;
            await GetDates();
            LoadHistory.IsEnabled = true;
            WriteHistory.IsEnabled = true;
            
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
                errorConsole.Text = "Data początkowa powinna być \nstarsza od końcowej!";
            }
            else
            {
                errorConsole.Text = "";
            }

        }

        void DrawAxis(Color _col, double _thick)
        {
            chartCanvas.Children.Add(new Line()
            {
                X1 = leftDrawingMargin,
                Y1 = topDrawingMargin,
                X2 = leftDrawingMargin,
                Y2 = chartCanvas.RenderSize.Height - bottomDrawingMargin + 10,
                StrokeThickness = _thick,
                Stroke = new SolidColorBrush(_col)
            });
            chartCanvas.Children.Add(new Line()
            {
                X1 = leftDrawingMargin,
                Y1 = topDrawingMargin,
                X2 = leftDrawingMargin - 10,
                Y2 = topDrawingMargin + 10,
                StrokeThickness = _thick,
                Stroke = new SolidColorBrush(_col)
            });
            chartCanvas.Children.Add(new Line()
            {
                X1 = leftDrawingMargin,
                Y1 = topDrawingMargin,
                X2 = leftDrawingMargin + 10,
                Y2 = topDrawingMargin + 10,
                StrokeThickness = _thick,
                Stroke = new SolidColorBrush(_col)
            });
            chartCanvas.Children.Add(new Line()
            {
                X1 = leftDrawingMargin - 10,
                Y1 = chartCanvas.RenderSize.Height - bottomDrawingMargin,
                X2 = chartCanvas.RenderSize.Width - rightDrawingMargin,
                Y2 = chartCanvas.RenderSize.Height - bottomDrawingMargin,
                StrokeThickness = _thick,
                Stroke = new SolidColorBrush(_col)
            });
            chartCanvas.Children.Add(new Line()
            {
                X1 = chartCanvas.RenderSize.Width - rightDrawingMargin,
                Y1 = chartCanvas.RenderSize.Height - bottomDrawingMargin,
                X2 = chartCanvas.RenderSize.Width - rightDrawingMargin - 10,
                Y2 = chartCanvas.RenderSize.Height - bottomDrawingMargin + 10,
                StrokeThickness = _thick,
                Stroke = new SolidColorBrush(_col)
            });
            chartCanvas.Children.Add(new Line()
            {
                X1 = chartCanvas.RenderSize.Width - rightDrawingMargin,
                Y1 = chartCanvas.RenderSize.Height - bottomDrawingMargin,
                X2 = chartCanvas.RenderSize.Width - rightDrawingMargin - 10,
                Y2 = chartCanvas.RenderSize.Height - bottomDrawingMargin - 10,
                StrokeThickness = _thick,
                Stroke = new SolidColorBrush(_col)
            });
        }

        void GetMinMax(out double min, out double max)
        {
            min = 0;
            max = 0;
            if (dataChart.Count < 2)
                return;
            min = Convert.ToDouble(dataChart[0].ExchangeRate.Replace(",","."));
            max = Convert.ToDouble(dataChart[0].ExchangeRate.Replace(",", "."));
            foreach (XMLexample.RekordWykres r in dataChart)
            {
                if (Convert.ToDouble(r.ExchangeRate.Replace(",", ".")) < min)
                    min = Convert.ToDouble(r.ExchangeRate.Replace(",", "."));
                if (Convert.ToDouble(r.ExchangeRate.Replace(",", ".")) > max)
                    max = Convert.ToDouble(r.ExchangeRate.Replace(",", "."));
            }
        }

        void DrawLevel(Color _col, double _y, string _val)
        {
            chartCanvas.Children.Add(new Line()
            {
                X1 = leftDrawingMargin - 4,
                Y1 = _y,
                X2 = chartCanvas.RenderSize.Width - rightDrawingMargin - 10,
                Y2 = _y,
                StrokeThickness = 1.0,
                Stroke = new SolidColorBrush(_col)
            });
            TextBlock textBlock = new TextBlock();
            textBlock.Text = _val;
            textBlock.Foreground = new SolidColorBrush(Colors.Blue);
            Canvas.SetLeft(textBlock, leftDrawingMargin - 25);
            Canvas.SetTop(textBlock, _y - 7);
            chartCanvas.Children.Add(textBlock);
        }

        void DrawCurrencyHistory(Color _col, double _thick)
        {

                chartCanvas.Children.Clear();
                if (dataChart.Count < 2)
                    return;
                double Cmin, Cmax;
                GetMinMax(out Cmin, out Cmax);
                double Xmin = 2 * leftDrawingMargin;
                double Xmax = chartCanvas.RenderSize.Width - (leftDrawingMargin + rightDrawingMargin);
                double Ymin = 2 * topDrawingMargin;
                double Ymax = chartCanvas.RenderSize.Height - (topDrawingMargin + bottomDrawingMargin);
                double stepX = (Xmax - Xmin) / (dataChart.Count - 1);
                double stepY = (Ymax - Ymin) / (Cmax - Cmin);

                XMLexample.RekordWykres prev = null;
                int i = 0;
                foreach (XMLexample.RekordWykres r in dataChart)
                {
                    if (prev == null)
                    {
                        prev = dataChart[0];
                        continue;
                    }
                    i++;

                    chartCanvas.Children.Add(new Line()
                    {
                        X1 = Xmin + (i - 1) * stepX,
                        Y1 = -1 * (Convert.ToDouble(prev.ExchangeRate.Replace(",", ".")) - Cmin) * stepY + Ymax,
                        X2 = Xmin + i * stepX,
                        Y2 = -1 * (Convert.ToDouble(r.ExchangeRate.Replace(",", ".")) - Cmin) * stepY + Ymax,
                        StrokeThickness = _thick,
                        Stroke = new SolidColorBrush(_col)
                    });
                    prev = r;
                    DrawLevel(Colors.Blue, Ymax, Cmin.ToString("0.00"));
                    DrawLevel(Colors.Blue, Ymin, Cmax.ToString("0.00"));
                }
                             
        }

        private void WriteHistory_Click(object sender, RoutedEventArgs e)
        {
            if (dataChart.Any())
            {
                DrawAxis(Colors.Black, 4.0);
                DrawCurrencyHistory(Colors.Red, 5.0);
            }
            else
            {
                chartCanvas.Children.Clear();
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Historia nie zawiera żadnych informacji do narysowania!";
                textBlock.Foreground = new SolidColorBrush(Colors.Red);
                textBlock.FontSize = 20;
                Canvas.SetLeft(textBlock, 50);
                Canvas.SetTop(textBlock, chartCanvas.Height/2);
                chartCanvas.Children.Add(textBlock);

            }
        }

        private void ExitButtonChart_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void ClearCanvas_Click(object sender, RoutedEventArgs e)
        {
            chartCanvas.Children.Clear();
        }


        



    }
}
