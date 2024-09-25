using System.Text;

namespace DriverRegisterKiosk
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnSendButtonClicked(object sender, EventArgs e)
        {
            var text = UserInput.Text;
            var isImportant = IsImportantCheckbox.IsChecked; // boolean hodnota checkboxu

            if (!string.IsNullOrEmpty(text))
            {

                using var httpClient = new HttpClient();
                // Vytvoření JSON obsahu, který obsahuje text a boolean
                var content = new StringContent($"{{\"licenseText\":\"{text}\", \"isLoading\": {isImportant.ToString().ToLower()}}}", Encoding.UTF8, "application/json");
                /*
                var response = await httpClient.PostAsync("https://192.168.0.206:7105/plates/", content);
                
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Úspěch", "Data byla úspěšně odeslána", "OK");
                }
                else
                {
                    await DisplayAlert("Chyba", "Nepodařilo se odeslat data", "OK");
                }
                */
                try
                {
                    var response2 = await httpClient.PostAsync("http://10.0.2.2:5202/plates/", content);
                    if (response2.IsSuccessStatusCode)
                    {
                        await DisplayAlert("Úspěch", "Data byla úspěšně odeslána2", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Chyba", "Nepodařilo se odeslat data", "OK");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                }
            }

        }
    }
}

