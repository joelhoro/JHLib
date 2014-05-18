using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SeamCarving
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
//        string pathin = @"..\..\assets2\" + "sc_desert_s.png";
        string pathin = @"c:\users\joel\pictures\" + "sc_desert_s.png";
        //pathin += "800px-Broadway_tower_edit.jpg";
        Seamcarving seamcarving;

        public MainWindow()
        {
            InitializeComponent();
            image1.Source = new BitmapImage(new Uri(pathin));
            var bitmap = new Bitmap(pathin);
            seamcarving = new Seamcarving(bitmap);
        }


        private void Process()
        {

            var backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (s,a) => 
                {
                    int N = 100;
                    for (int i = 0; i < N;i++ )
                    {
                        string filename = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".jpg";
                        seamcarving.Iterate();

                        this.Dispatcher.Invoke(() =>
                        {
                            progressbar1.Value = 100 * (1+i) / N;
                            //seamcarving.bitmap.Save(filename);
                            //image2.Source = new BitmapImage(new Uri(filename));
                            //textbox.Text = filename;
                        });
                    }

                };

            backgroundWorker.RunWorkerCompleted += (s, a) =>
                {
                    this.UpdateLayout();
                    button1.IsEnabled = true;
                };

            button1.IsEnabled = false;
            progressbar1.Value = 0;

            backgroundWorker.RunWorkerAsync();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process();
        }
    }
}
