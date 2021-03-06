﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PlagiarismDetector.Actions;
using PlagiarismDetector.Types;
using PlagiarismDetector.ViewModel;

namespace PlagiarismDetector
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {            
            var acceptClear = MessageBox.Show("Вы действительно хотите выйти?",
                "Подтверждение выхода",
                MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (acceptClear == MessageBoxResult.Yes)
            {
                SaveOrLoadActions.SaveSettings();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void MenuItemSelectExpandedResult_OnClick(object sender, RoutedEventArgs e)
        {
            if (PlagiarismDetectResultsListView.SelectedItem is PlagiarismDetectResult plagiarismDetectResult)
                MainViewModel.M_Model.SelectedResult = plagiarismDetectResult.SimilarityDocuments;
        }
    }
}
