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
        private bool m_isRefreshing;
        private ObservableCollection<Land> m_lands;
        private string m_filter;
        private List<Land> m_landList;
        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get { return m_isRefreshing; }
            set { SetValue(ref m_isRefreshing, value); }
        }
        public ObservableCollection<Land> Lands
        {
            get { return m_lands; }
            set { SetValue(ref m_lands, value); }
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
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            RefreshCommand = new Command(LoadLandsAsync);
            SearchCommand = new Command(Search);
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
            m_landList = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(m_landList);
            IsRefreshing = false;
        }
        private void Search()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                this.Lands = new ObservableCollection<Land>(m_landList);
            }
            else
            {
                this.Lands = new ObservableCollection<Land>(
                    m_landList.Where(l => l.Name.ToLower().Contains(this.Filter) ||
                                          l.Capital.ToLower().Contains(this.Filter)));
            }
        }
        #endregion
    }
}
