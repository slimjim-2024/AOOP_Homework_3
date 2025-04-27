using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Diagnostics;

namespace Homework4
{
    public partial class MessageBox : Window
    {
        public string Message { get; }

        public MessageBox(string title, string message)
        {
            Title = title; 
            Message = message;
            DataContext = this; 
            InitializeComponent();
        }

        // Handler for OK button; closes the dialog
        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

// This file defines a reusable MessageBox window for showing errors or information to the user.
