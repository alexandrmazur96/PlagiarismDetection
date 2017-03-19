using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using DocumentAdder.Helpers;
using DocumentAdder.Types;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace DocumentAdder.View.SettingsViews
{
    /// <summary>
    /// Логика взаимодействия для RepositorySettingsView.xaml
    /// </summary>
    public partial class RepositorySettingsView : Page
    {

        private readonly ObservableCollection<RepositoryPath> _repositoryCollection =
            ViewModel.SettingsViewModels.RepositorySettingViewModel.RepositoryModel.CollectionPaths;

        public RepositorySettingsView()
        {
            InitializeComponent();

            PathListView.ItemsSource = _repositoryCollection;

            PathListView.DisplayMemberPath = "StoragePath";
        }

        /// <summary>
        /// Выполняется удаление из контекстного меню в PathListView (ListBox в RepositorySettings)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepositoryContextDeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            List<RepositoryPath> tmp = ViewModel.SettingsViewModels.RepositorySettingViewModel.RepositoryModel.CollectionPaths.ToList();
            foreach (var selectedItem in PathListView.SelectedItems)
            {
                tmp.Remove(selectedItem as RepositoryPath);
            }

            ViewModel.SettingsViewModels.RepositorySettingViewModel.RepositoryModel.CollectionPaths =
                new ObservableCollection<RepositoryPath>(tmp);

            PathListView.ItemsSource =
                ViewModel.SettingsViewModels.RepositorySettingViewModel.RepositoryModel.CollectionPaths;
        }

        /// <summary>
        /// Выполняется копирование выбранных путей в буффер обмена ОС
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepositoryContextCopyBtn_OnClick(object sender, RoutedEventArgs e)
        {
            StringBuilder textToClipboard = new StringBuilder();
            foreach (var selectedItem in PathListView.SelectedItems)
            {
                textToClipboard.Append((selectedItem as RepositoryPath)?.StoragePath);
                textToClipboard.Append(Environment.NewLine);
            }
            Clipboard.SetText(textToClipboard.ToString());
        }
    }
}
