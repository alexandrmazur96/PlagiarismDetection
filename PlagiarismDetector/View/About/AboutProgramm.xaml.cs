using System;
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
using System.Windows.Shapes;

namespace PlagiarismDetector.View.About
{
    /// <summary>
    /// Логика взаимодействия для AboutProgramm.xaml
    /// </summary>
    public partial class AboutProgramm : Window
    {
        public AboutProgramm()
        {
            InitializeComponent();
        }

        private void AboutProgramm_OnClosing(object sender, CancelEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
