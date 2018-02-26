using Lands.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lands.ViewModels
{
    public class LandViewModel:BaseViewModel
    {
        #region Attributes
        private ObservableCollection<Border> m_borders;
        private ObservableCollection<Currency> m_currencies;
        private ObservableCollection<Language> m_languages;
        private Translations m_translations;
        #endregion

        #region Properties
        public Land Land { get; set; }
        public ObservableCollection<Border> Borders
        {
            get { return m_borders; }
            set { SetValue(ref m_borders, value); }
        }
        public ObservableCollection<Currency> Currencies
        {
            get { return m_currencies; }
            set { SetValue(ref m_currencies, value); }
        }
        public ObservableCollection<Language> Languages
        {
            get { return m_languages; }
            set { SetValue(ref m_languages, value); }
        }
        public Translations Translations
        {
            get { return m_translations; }
            set { SetValue(ref m_translations, value); }
        }
        #endregion

        #region Constructors
        public LandViewModel(Land land)
        {
            this.Land = land;
            LoadBorders();
            Currencies = new ObservableCollection<Currency>(this.Land.Currencies);
            Languages = new ObservableCollection<Language>(this.Land.Languages);
            Translations = this.Land.Translations;
        }
        #endregion

        #region Methods
        private void LoadBorders()
        {
            this.Borders = new ObservableCollection<Border>();

            foreach (var border in this.Land.Borders)
            {
                var land = MainViewModel.GetInstance()
                                        .LandsList
                                        .Where(l => l.Alpha3Code == border)
                                        .FirstOrDefault();
                if (land != null)
                {
                    Borders.Add(new Border()
                    {
                        Code = land.Alpha3Code,
                        Name = land.Name
                    });
                }
            }
        }
        #endregion
    }
}
