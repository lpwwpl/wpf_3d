using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Petzold.Media3D;
using System.Windows.Media.Media3D;
using Microsoft.Win32;
using System.Configuration;
using System.ComponentModel;
using GeometryViz3D.ViewModels;

namespace GeometryViz3D.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainWindowViewModel m_viewModel;

        public MainWindow()
        {
            m_viewModel = new GeometryViz3D.ViewModels.MainWindowViewModel();
            
            this.DataContext = m_viewModel;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }  
    }
}
