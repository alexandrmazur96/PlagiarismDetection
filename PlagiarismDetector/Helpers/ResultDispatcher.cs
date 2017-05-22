using System.Collections.ObjectModel;
using PlagiarismDetector.Types;

namespace PlagiarismDetector.Helpers
{
    public static class ResultDispatcher
    {
        private static ObservableCollection<PlagiarismDetectExpandedResult> _similarityResult;

        public static ObservableCollection<PlagiarismDetectExpandedResult> SimilarityResult
        {
            get { return _similarityResult; }
            set {
                _similarityResult = value != null ? new ObservableCollection<PlagiarismDetectExpandedResult>(value) : new ObservableCollection<PlagiarismDetectExpandedResult>();
            }
        }
    }
}