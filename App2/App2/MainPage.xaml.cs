using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        private const string imageUrl = "http://peezy.us.to/output.png";
        private HttpClient httpClient;
        public MainPage()
        {

            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            httpClient = new HttpClient();
            UpdateImage();
        }
        private async void Button1_Clicked(object sender, EventArgs e)
        {
            string imageUrl = "http://peezy.us.to/output.png";
            imageUrl += $"?timestamp={DateTime.Now.Ticks}";

            myImage.Source = ImageSource.FromUri(new Uri(imageUrl));
            myImage.Scale = 1;
            myImage.TranslationX = 0;
            myImage.TranslationY = 0;
        }
        private async void Button2_Clicked(object sender, EventArgs e)
        {
            // Button 2 functionality
            await Navigation.PushAsync(new Page2());
        }
        private async Task UpdateImage()
        {
            try
            {
                var imageData = await httpClient.GetByteArrayAsync(imageUrl);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                myImage.Source = ImageSource.FromStream(() => new System.IO.MemoryStream(imageData));
            }
            catch (Exception ex)
            {
                // Handle any errors that might occur during image retrieval
                Console.WriteLine($"Error loading image: {ex.Message}");
            }
        }

    }
}
