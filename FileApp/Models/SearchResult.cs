using FileApp.Framework;
using Newtonsoft.Json;

namespace FileApp.Models
{
    public class SearchResult : BaseModel
    {
        private string _file;
        [JsonProperty("file")]
        public string File
        {
            get
            {
                return _file;
            }
            set
            {
                if (_file != value)
                {
                    _file = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _line;
        [JsonProperty("line")]
        public string Line
        {
            get
            {
                return _line;
            }
            set
            {
                if (_line != value)
                {
                    _line = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
