using System;
using System.Windows;
using System.Windows.Threading;

namespace CountdownTimerApp
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TimeSpan remaining;
        private TimeSpan total;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(MinutesBox.Text, out int minutes)) minutes = 0;
            if (!int.TryParse(SecondsBox.Text, out int seconds)) seconds = 0;

            remaining = TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
            if (remaining.TotalSeconds <= 0)
            {
                MessageBox.Show("Bitte eine gültige Zeit eingeben.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            total = remaining;
            UpdateCountdownUI();
            timer.Start();
            StartButton.IsEnabled = false;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remaining = remaining.Subtract(TimeSpan.FromSeconds(1));
            UpdateCountdownUI();

            if (remaining.TotalSeconds <= 0)
            {
                timer.Stop();
                MessageBox.Show("Zeit abgelaufen!", "Fertig", MessageBoxButton.OK, MessageBoxImage.Information);
                StartButton.IsEnabled = true;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            CountdownLabel.Text = "00:00";
            CountdownBar.Value = 0;
            StartButton.IsEnabled = true;
        }

        private void UpdateCountdownUI()
        {
            CountdownLabel.Text = remaining.ToString(@"mm\:ss");

            if (total.TotalSeconds > 0)
            {
                double percent = (1 - remaining.TotalSeconds / total.TotalSeconds) * 100;
                CountdownBar.Value = percent;
            }
            else
            {
                CountdownBar.Value = 0;
            }
        }
        private void OpenInfo_Click(object sender, RoutedEventArgs e)
        {
            var info = new InfoWindow
            {
                Owner = this
            };
            info.ShowDialog();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


    }
}
