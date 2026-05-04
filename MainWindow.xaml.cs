using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace CurrencyConverter
{
    public partial class MainWindow : Window
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
            LoadCurrencies();
        }

        private void LoadCurrencies()
        {
            var currencies = new List<string> { "USD", "EUR", "RUB", "GBP", "JPY" };
            cmbFrom.ItemsSource = currencies;
            cmbTo.ItemsSource = currencies;
            cmbFrom.SelectedIndex = 0;
            cmbTo.SelectedIndex = 1;
        }

        private async void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string from = cmbFrom.SelectedItem.ToString();
                string to = cmbTo.SelectedItem.ToString();
                decimal amount = decimal.Parse(txtAmount.Text);

                string url = $"https://api.exchangerate-api.com/v4/latest/{from}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();

                var rates = JsonConvert.DeserializeObject<ExchangeRates>(json);
                decimal rate = rates.Rates[to];
                decimal result = amount * rate;

                txtResult.Text = $"{result:F2} {to}";
            }
            catch (Exception ex)
            {
                txtResult.Text = $"Ошибка: ЭТО НЕ ЦЫФРЫ БРООО";
            }
        }
    }

    public class ExchangeRates
    {
        public string Base { get; set; }
        public string Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }
}