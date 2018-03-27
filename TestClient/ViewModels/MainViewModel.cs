using iADAATPA.MTProvider.Helpers;
using iADAATPA.MTProvider.Extensions;
using Sdl.LanguagePlatform.TranslationMemoryApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestClient.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ITranslationProvider _mtProvider;
        private ICommand _translateCommand;
        private string _translation;
        private readonly SynchronizationContext _syncContext;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel(ITranslationProvider mtProvider)
        {
            _mtProvider = mtProvider;
            _translateCommand = new DelegateCommand(Translate);
            // we asume this ctor is called from the UI thread
            _syncContext = SynchronizationContext.Current;
        }

        private void Translate()
        {
            var langdir = _mtProvider.GetLanguageDirection(new Sdl.LanguagePlatform.Core.LanguagePair("en", "lv"));
            var res = langdir.SearchText(null, Source);

            _syncContext.Post(o => {
                Translation = res.First().TranslationProposal.TargetSegment.ToString();
            }, null);
        }

        public ICommand TranslateCommand => _translateCommand;

        public string Source { get; set; }

        public string Translation
        {
            get => _translation;
            set
            {
                if (value != _translation)
                {
                    _translation = value;
                    this.Notify(PropertyChanged);
                }
            }
        }
    }
}
