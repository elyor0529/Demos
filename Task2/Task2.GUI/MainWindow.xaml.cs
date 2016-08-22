using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Task2.GUI.Domain;

namespace Task2.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }

        private async void MenuAboutUsButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new SampleMessageDialog
            {
                Message = { Text = "Put content here..." }
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }

        private async void MenuQuitButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sampleMessageDialog = new SampleProgressDialog();

            sampleMessageDialog.MouseDoubleClick+= delegate(object o, MouseButtonEventArgs args)
            {
                Close();
            };

            await DialogHost.Show(sampleMessageDialog, "RootDialog");
        }
    }
}
