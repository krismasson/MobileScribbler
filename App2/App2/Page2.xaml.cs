using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using System.IO;
using System.ComponentModel;
using System.Reflection;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Shapes;

namespace App2
{


    public partial class Page2 : ContentPage
    {
        private bool isDrawing;
        private SKPath currentPath;

        public Page2()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();


            textInputEntry.Text = SharedData.EnteredText;
            textInputEntry.Focused += OnTextInputEntryFocused;
            if (string.IsNullOrEmpty(SharedData.EnteredText))
            {
                textInputEntry.Text = "style of Paul Barson";
            }
            else
            {
                textInputEntry.Text = SharedData.EnteredText;
            }


        }
        private void OnTextInputEntryFocused(object sender, FocusEventArgs e)
        {
            // Select all text in the Entry control when it receives focus
            textInputEntry.CursorPosition = 0;
            textInputEntry.SelectionLength = textInputEntry.Text != null ? textInputEntry.Text.Length : 0;
        }


        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            var canvas = e.Surface.Canvas;
            canvas.Clear(SKColors.White);

            foreach (var path in PathManager.Paths)
            {
                var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    StrokeWidth = 5,
                    StrokeCap = SKStrokeCap.Round,
                    StrokeJoin = SKStrokeJoin.Round
                };

                canvas.DrawPath(path, paint);
            }

            if (currentPath != null)
            {
                var paint = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = SKColors.Black,
                    StrokeWidth = 5,
                    StrokeCap = SKStrokeCap.Round,
                    StrokeJoin = SKStrokeJoin.Round
                };

                canvas.DrawPath(currentPath, paint);
            }
        }

        private void OnCanvasViewTouch(object sender, SKTouchEventArgs e)
        {
            switch (e.ActionType)
            {
                case SKTouchAction.Pressed:
                    isDrawing = true;
                    currentPath = new SKPath();
                    currentPath.MoveTo(e.Location);
                    break;
                case SKTouchAction.Moved:
                    if (isDrawing)
                        currentPath.LineTo(e.Location);
                    break;
                case SKTouchAction.Released:
                    if (isDrawing)
                    {
                        PathManager.Paths.Add(currentPath);
                        currentPath = null;
                    }
                    isDrawing = false;
                    break;
            }

            ((SKCanvasView)sender).InvalidateSurface();
            e.Handled = true;
        }

        private void OnEraseButtonClicked(object sender, System.EventArgs e)
        {
            PathManager.Paths.Clear();
            canvasView.InvalidateSurface();
        }
        private void OnButton1Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "illustration in the style of Moebius";
        }
        private void OnButton2Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "A painting in the style of Monet";
        }
        private void OnButton3Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "abstract art";
        }
        private void OnButton4Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "style of Paul Barson";
        }
        private void OnButton5Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "style of Jean DuBuffet";
        }
        private void OnButton6Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "style of Chris Dyer";
        }
        private void OnButton7Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "style of Lisa Frank";
        }
        private void OnButton8Clicked(object sender, System.EventArgs e)
        {
            textInputEntry.Text = "style of Ernst Ludwig Kirchner";
        }
        private void OnButton9Clicked(object sender, System.EventArgs e)
        {
            //
            string randomText = PrintHelper.GetRandomText();
            textInputEntry.Text = randomText;
        }

        private async void OnNewButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
        private void OnTextInputEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            SharedData.EnteredText = e.NewTextValue;
        }
        private async void OnSendButtonClicked(object sender, EventArgs e)
        {

            // Convert the drawing to an image
            var image = ConvertDrawingToImage();

            // Create a multipart form data content
            var content = new MultipartFormDataContent();

            // Add the image file to the content
            var stream = new MemoryStream(image);
            var fileContent = new StreamContent(stream);
            content.Add(fileContent, "file", "drawing.png");

            // Add the 'text' parameter to the content
            content.Add(new StringContent(SharedData.EnteredText), "text");

            // Create an HttpClient instance
            using (var client = new HttpClient())
            {
                // Send the request to the REST URL
                var response = await client.PostAsync("http://peezy.us.to/upload.php", content);

                // Check the response status
                if (response.IsSuccessStatusCode)
                {
                    // Handle successful response
                    // ...
                }
                else
                {
                    // Handle error response
                    // ...
                }
            }
        }
        private byte[] ConvertDrawingToImage()
        {
            var image = new SKImageInfo((int)canvasView.CanvasSize.Width, (int)canvasView.CanvasSize.Height);
            using (var surface = SKSurface.Create(image))
            {
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                foreach (var path in PathManager.Paths)
                {
                    var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black,
                        StrokeWidth = 5,
                        StrokeCap = SKStrokeCap.Round,
                        StrokeJoin = SKStrokeJoin.Round
                    };

                    canvas.DrawPath(path, paint);
                }

                if (currentPath != null)
                {
                    var paint = new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black,
                        StrokeWidth = 5,
                        StrokeCap = SKStrokeCap.Round,
                        StrokeJoin = SKStrokeJoin.Round
                    };

                    canvas.DrawPath(currentPath, paint);
                }

                using (var imageSnapshot = surface.Snapshot())
                {
                    using (var data = imageSnapshot.Encode(SKEncodedImageFormat.Png, 100))
                    {
                        return data.ToArray();
                    }
                }
            }

        }
    }
}