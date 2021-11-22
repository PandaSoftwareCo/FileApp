using FileApp.Framework;
using FileApp.Models;

namespace FileApp.ViewModels
{
    public class DetailViewModel : BaseModel
    {
        private SearchResult _selectedFile;
        public SearchResult SelectedFile
        {
            get
            {
                return _selectedFile;
            }
            set
            {
                if (_selectedFile != value)
                {
                    _selectedFile = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
