using DocumentAdder.Actions;
using DocumentAdder.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using DocumentAdder.ViewModel;

namespace DocumentAdder
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

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Actions.SaveOrLoadActions.SaveSettings();
           await LogViewModel.PushLogToDbAsync();
        }

        private void SettingsListBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.Assert(e.OriginalSource != null, "e.OriginalSource != null");
            var item = ItemsControl.ContainerFromElement(sender as ListBox, (DependencyObject) e.OriginalSource) as ListBoxItem;
            if (item?.Content is Pages pages) SettingsFrame.Content = pages.PageRef;
        }
    }
}
