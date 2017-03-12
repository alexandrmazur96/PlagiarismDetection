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

namespace DocumentAdder.View.SettingsViews
{
    /// <summary>
    /// Логика взаимодействия для RepositorySettingsView.xaml
    /// </summary>
    public partial class RepositorySettingsView : Page
    {
        public RepositorySettingsView()
        {
            InitializeComponent();
            PathListView.ItemsSource =
                ViewModel.SettingsViewModels.RepositorySettingViewModel.RepositoryModel.CollectionPaths;
            PathListView.DisplayMemberPath = "StoragePath";
        }
    }
}
