using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Diagnostics;
using System;

namespace Homework4
{
    public partial class SpeedDialog : Window
    {
        public double Speed { get; private set; } = 1;

        public SpeedDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(SpeedInput.Text, out double speed) && speed > 0)
            {
                Speed = speed;
                Close();
            }
            else
            {
                ErrorMessage.IsVisible = true;
            }
        }
    }
}