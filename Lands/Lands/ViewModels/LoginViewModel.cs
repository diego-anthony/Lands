using System;
using Lands.Services;
using Lands.Views;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {
        #region ApiServices
        private ApiService m_apiService;
        #endregion

        #region Attributes
        private string _email;
        private bool _isEnabled;
        private bool _isRunning;
        private string _password;
        #endregion

        #region Properties
        public string Email
        {
            get { return _email; }
            set { SetValue(ref _email, value); }
        }
        public string Password
        {
            get { return _password; }
            set { SetValue(ref _password, value); }
        }
        public bool IsRunning
        {
            get { return _isRunning; }
            set { SetValue(ref _isRunning, value); }
        }
        public bool IsRemembered { get; set; }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetValue(ref _isEnabled, value); }
        }
        #endregion

        #region Constructor
        public LoginViewModel()
        {
            m_apiService = new ApiService();
            this.IsRemembered = true;
            IsEnabled = true;
            this.Email = "dsanthony997@gmail.com";
            this.Password = "123";
            LoginCommand = new Command(LoginAsync);
        }
        #endregion

        #region Commands

        public Command LoginCommand { get;}

        private async void LoginAsync()
        {
            IsEnabled = false;
            IsRunning = true;
            if (string.IsNullOrEmpty(this.Email))
            {
                ShowErrorMessageAlertAsync("You must enter an email.");
                IsEnabled = true;
            }
            else if (string.IsNullOrEmpty(this.Password))
            {
                ShowErrorMessageAlertAsync("You must enter an password.");
                IsEnabled = true;
            }
            else
            {
                var connection = await m_apiService.CheckConnection();
                if (connection.IsSuccess)
                {
                    var urlBase = "http://landsapi97.azurewebsites.net";
                    var token = await this.m_apiService.GetToken(urlBase,this.Email,this.Password);

                    if (token != null)
                    {
                        if (!string.IsNullOrEmpty(token.AccessToken))
                        {
                            IsEnabled = true;
                            this.Email = string.Empty;
                            this.Password = string.Empty;
                            var mainViewModel = MainViewModel.GetInstance();
                            mainViewModel.Token = token;
                            mainViewModel.Lands = new LandsViewModel();
                            await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());
                        }
                        else
                        {
                            this.Password = string.Empty;
                            ShowErrorMessageAlertAsync(token.ErrorDescription);
                        }
                    }
                    else
                    {
                        ShowErrorMessageAlertAsync("Something was wrong, please try later");
                        this.IsEnabled = true;
                        this.Password = string.Empty;
                    }
                }
                else
                {
                    ShowErrorMessageAlertAsync(connection.Message);
                    this.IsEnabled = true;
                    this.Password = string.Empty;
                }
            }
            IsRunning = false;
            this.IsEnabled = true;
        }

        private async void ShowErrorMessageAlertAsync(string message)
        {
            await Application.Current.MainPage.DisplayAlert(
                        "Error",
                        message,
                        "Ok");
        }
        #endregion
    }
}
