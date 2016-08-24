using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Task2.Common.Data;
using Task2.Common.Models;
using Task2.GUI.Domain;
using Task2.GUI.ViewModels;

namespace Task2.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ReportViewModel _model = new ReportViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _model;
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MenuToggleButton.IsChecked = false;
        }

        private void MenuQuitButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _model.Reports = await HrmDbRepository.GetSalaryReportNotAssigned(DateTime.Now, 1, 100);
        }
    }
}
