using System.Diagnostics;
using System.Windows.Input;
using PlagiarismDetector.Helpers;
using PlagiarismDetector.Model;

namespace PlagiarismDetector.ViewModel
{
    public class ExpandedResultViewModel
    {
        public static ExpandedResultModel Model { get; private set; }

        public ICommand OpenInExplorerCommand { get; set; }

        public ExpandedResultViewModel()
        {
            OpenInExplorerCommand = new DelegateCommand(action => OpenInExplorer());
        }

        static ExpandedResultViewModel()
        {
            Model = new ExpandedResultModel();
        }

        /// <summary>
        /// Реализация функционала "Открыть в проводнике".
        /// </summary>
        private static void OpenInExplorer()
        {
            var prFolder = new Process();
            var psi = new ProcessStartInfo();
            string file = Model.SelectedPath;
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Normal;
            psi.FileName = "explorer";
            psi.Arguments = @"/n, /select, " + file;
            prFolder.StartInfo = psi;
            prFolder.Start();
        }
    }
}