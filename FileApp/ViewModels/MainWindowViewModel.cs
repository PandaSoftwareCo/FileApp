using FileApp.Framework;
using FileApp.Interfaces;
using FileApp.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FileApp.ViewModels
{
    public class MainWindowViewModel : BaseModel
    {
        private readonly IFileSearch _fileSearch;
        private IAppSettings _settings;

        public IAppSettings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
                string docPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

                Path = _settings.Path;
                Pattern = _settings.Pattern;
                Term = _settings.Term;
            }
        }

        private readonly IOptionsMonitor<IAppSettings> _optionsMonitor;
        private readonly ILogger<MainWindowViewModel> _logger;
        private readonly IDisposable _changeListener;

        private string _path;
        [Required]
        public string Path
        {
            get
            {
                return _path;
            }
            set
            {
                if (_path != value)
                {
                    _path = value;
                    LoadCommand?.RaiseCanExecuteChanged();
                    OnPropertyChanged();
                }
            }
        }

        private string _pattern;
        [Required]
        public string Pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                if (_pattern != value)
                {
                    _pattern = value;
                    LoadCommand?.RaiseCanExecuteChanged();
                    OnPropertyChanged();
                }
            }
        }

        private string _term;
        [Required]
        public string Term
        {
            get
            {
                return _term;
            }
            set
            {
                if (_term != value)
                {
                    _term = value;
                    LoadCommand?.RaiseCanExecuteChanged();
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<SearchResult> _files = new();
        public ObservableCollection<SearchResult> Files
        {
            get
            {
                return _files;
            }
            set
            {
                if (_files != value)
                {
                    _files = value;
                    OnPropertyChanged();
                }
            }
        }

        private object _devailView;
        public object DetailView
        {
            get { return _devailView; }
            set { _devailView = value; OnPropertyChanged(); }
        }

        private string _status = "Ready";
        [Required]
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainWindowViewModel(IFileSearch fileSearch, IAppSettings settings, IOptionsMonitor<IAppSettings> optionsMonitor, ILogger<MainWindowViewModel> logger, ILoggerFactory loggerFactory)
        {
            _fileSearch = fileSearch ?? throw new ArgumentNullException(nameof(fileSearch));
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _changeListener = _optionsMonitor.OnChange(OnOptionsChange);

            DetailView = new DetailViewModel();
        }

        private void OnOptionsChange(IAppSettings settings, string name)
        {
            Settings = settings;
        }

        ~MainWindowViewModel()
        {
            _changeListener?.Dispose();
        }

        private IDelegateCommand _loadCommand;

        public IDelegateCommand LoadCommand
        {
            get
            {
                if (_loadCommand == null)
                {
                    _loadCommand = new DelegateCommand(async (parameter) => await Load(parameter), CanLoad);
                }
                return _loadCommand;
            }
        }

        private async Task Load(object parameter)
        {
            var watch = Stopwatch.StartNew();
            var window = System.Windows.Application.Current.MainWindow;
            window.Cursor = System.Windows.Input.Cursors.Wait;

            var files = await Task<List<SearchResult>>.Run(() => _fileSearch.SearchFiles(Path, Pattern, Term));
            foreach (var file in files)
            {
                Files.Add(file);
            }

            watch.Stop();
            var time = watch.Elapsed;
            Status = $"ELAPSED {time}";
            _logger.LogInformation(Status);
            window.Cursor = System.Windows.Input.Cursors.Arrow;
            SaveCommand?.RaiseCanExecuteChanged();
        }

        private bool CanLoad(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Path) && !string.IsNullOrWhiteSpace(Term);
        }

        private IDelegateCommand _saveCommand;

        public IDelegateCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new DelegateCommand(async (parameter) => await Save(parameter), CanSave);
                }
                return _saveCommand;
            }
        }

        private async Task Save(object parameter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt|JSON file (*.json)|*.json|All files (*.*)|*.*"; // Filter files by extension
            dlg.OverwritePrompt = true;
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                string filename = dlg.FileName;
                await File.WriteAllTextAsync(dlg.FileName, JsonConvert.SerializeObject(Files, new JsonSerializerSettings {
                    Formatting = Formatting.Indented
                }));
            }
        }

        private bool CanSave(object parameter)
        {
            return Files.Count > 0;
        }
    }
}
