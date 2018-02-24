using System;
using Lands.Models;
using Lands.Views;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LandItemViewModel: Land
    {
        #region Commands
        public Command SelectLandCommand { get; set; }
        #endregion

        #region Constructors
        public LandItemViewModel() => SelectLandCommand = new Command(SelectLandAsync);
        #endregion

        #region Methods
        private async void SelectLandAsync()
        {
            MainViewModel.GetInstance().Land = new LandViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new LandTabbedPage());
        }
        #endregion
    }
}
