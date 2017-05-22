using System.Collections.ObjectModel;
using PlagiarismDetector.Helpers;
using PlagiarismDetector.Types;

namespace PlagiarismDetector.Model
{
    public class ExpandedResultModel : BaseModel
    {
        #region Properties

        public ObservableCollection<PlagiarismDetectExpandedResult> Results
        {
            get { return ResultDispatcher.SimilarityResult; }
            set { ResultDispatcher.SimilarityResult = value; NotifyPropertyChanged(); }
        }

        public string SelectedPath { get; set; }

        #endregion
    }
}