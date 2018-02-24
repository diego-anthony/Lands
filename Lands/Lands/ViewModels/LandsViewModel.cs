using Lands.Models;
using Lands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LandsViewModel: BaseViewModel
    {
        #region Services
        private ApiService m_apiService;
        #endregion

        #region Attributes
        private ObservableCollection<LandItemViewModel> m_lands;
        private bool m_isRefreshing;
        private string m_filter;
        #endregion

        #region Properties
        public ObservableCollection<LandItemViewModel> Lands
        {
            get { return m_lands; }
            set { SetValue(ref m_lands, value); }
        }
        public bool IsRefreshing
        {
            get { return m_isRefreshing; }
            set { SetValue(ref m_isRefreshing, value); }
        }
        public string Filter
        {
            get { return m_filter; }
            set
            {
                SetValue(ref m_filter, value);
                Search();
            }
        }
        #endregion

        #region Commands
        public Command RefreshCommand { get; }
        public Command SearchCommand { get; }
        public Command SelectLandCommand { get; set; }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            RefreshCommand = new Command(LoadLandsAsync);
            SearchCommand = new Command(Search);
            SelectLandCommand = new Command(LoadLandsAsync);
            m_apiService = new ApiService();
            this.LoadLandsAsync();
        }
        #endregion

        #region Methods
        private async void LoadLandsAsync()
        {
            var connection = await this.m_apiService.CheckConnection();
            IsRefreshing = true;
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    connection.Message,
                    "Accept");
                await Application.Current.MainPage.Navigation.PopToRootAsync();
                return;
            }

            var response = await m_apiService.GetList<Land>(
                "https://restcountries.eu",
                "/rest",
                "/v2/all");
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
            }
            // Corregir

            MainViewModel.GetInstance().LandsList = (List<Land>) response.Result;
            this.Lands = new ObservableCollection<LandItemViewModel>(this.ToLandItemViewModel());
            IsRefreshing = false;
        }

        private IEnumerable<LandItemViewModel> ToLandItemViewModel()
        {
            return MainViewModel.GetInstance().LandsList.Select(l => new LandItemViewModel()
            {
                Alpha2Code = l.Name,
                Alpha3Code = l.Alpha3Code,
                AltSpellings = l.AltSpellings,
                Area = l.Area,
                Borders = l.Borders,
                CallingCodes = l.CallingCodes,
                Capital = l.Capital,
                Cioc = l.Cioc,
                Currencies = l.Currencies,
                Demonym = l.Demonym,
                Flag = l.Flag,
                Gini = l.Gini,
                Languages = l.Languages,
                Latlng = l.Latlng,
                Name = l.Name,
                NativeName = l.NativeName,
                NumericCode = l.NumericCode,
                Population = l.Population,
                Region = l.Region,
                RegionalBlocs = l.RegionalBlocs,
                Subregion = l.Subregion,
                Timezones = l.Timezones,
                TopLevelDomain = l.TopLevelDomain,
                Translations = l.Translations,
            });
        }

        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Lands = new ObservableCollection<LandItemViewModel>(this.ToLandItemViewModel());
            }
            else
            {
                this.Lands = new ObservableCollection<LandItemViewModel>(
                    this.ToLandItemViewModel().Where(l => l.Name.ToLower().Contains(this.Filter) ||
                                          l.Capital.ToLower().Contains(this.Filter)));
            }
        }
        #endregion
    }
}
