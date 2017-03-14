using DocumentAdder.Actions;
using DocumentAdder.Types;
using System;
using System.Collections.Generic;
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Actions.SaveOrLoadActions.SaveSettings();
        }

        private void SettingsListBox_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {           
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;
            var pages = item?.Content as Pages;
            if (pages != null) SettingsFrame.Content = pages.PageRef;
        }
    }
}
