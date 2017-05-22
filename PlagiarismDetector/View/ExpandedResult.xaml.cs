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
using System.Windows.Shapes;
using PlagiarismDetector.Types;
using PlagiarismDetector.ViewModel;

namespace PlagiarismDetector.View
{
    /// <summary>
    /// Логика взаимодействия для ExpandedResult.xaml
    /// </summary>
    public partial class ExpandedResult : Window
    {
        public ExpandedResult()
        {
            InitializeComponent();
        }        

        private void MenuItemOpenInExplorer_OnClick(object sender, RoutedEventArgs e)
        {
            if (ExpandedResultListView.SelectedItem is PlagiarismDetectExpandedResult selectedPath) ExpandedResultViewModel.Model.SelectedPath = selectedPath.ResultFilePath;
        }
    }
}
