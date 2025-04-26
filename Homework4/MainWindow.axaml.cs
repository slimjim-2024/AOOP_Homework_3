using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Diagnostics;

namespace Homework4
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void OnSpeedButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SpeedDialog();
            await dialog.ShowDialog(this);

            if (dialog.Speed > 0)
            {
                SpeedButton.Content = $"Speed ({dialog.Speed}x)";
            }
        }
    }
}