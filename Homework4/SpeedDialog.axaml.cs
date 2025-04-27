using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Diagnostics;
using System;

namespace Homework4
{
    // Modal dialog for entering a speed multiplier for the simulation
    public partial class SpeedDialog : Window
    {
        // The user-entered speed factor (default 1x)
        public double Speed { get; private set; } = 1;

        public SpeedDialog()
        {
            InitializeComponent();
        }
        private void OnConfirmClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(SpeedInput.Text, out double speed) && speed > 0)
            {
                Speed = speed;    // Save the new speed
                Close();          // Close the dialog
            }
            else
            {
                // Show error message if parsing failed or speed <= 0
                ErrorMessage.IsVisible = true;
            }
        }
    }
}

// a speed multiplier for the recipe simulation and returns the Speed .
