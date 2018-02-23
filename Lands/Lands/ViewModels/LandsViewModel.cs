using Lands.Models;
using Lands.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LandsViewModel: BaseViewModel
    {
        #region Services
        private ApiService m_apiService;
        #endregion

        #region Attributes
        private ObservableCollection<Land> m_lands;
        #endregion

        #region Properties
        public ObservableCollection<Land> Lands
        {
            get { return m_lands; }
            set { SetValue(ref m_lands, value); }
        }
        #endregion

        #region Constructors
        public LandsViewModel()
        {
            m_apiService = new ApiService();
            this.LoadLandsAsync();
        }
        #endregion

        #region Methods
        private async void LoadLandsAsync()
        {
            var connection = await this.m_apiService.CheckConnection();
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
            var list = (List<Land>)response.Result;
            this.Lands = new ObservableCollection<Land>(list);
        }
        #endregion
    }
}
