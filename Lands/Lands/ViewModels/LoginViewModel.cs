using Lands.Views;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class LoginViewModel: BaseViewModel
    {
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
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an email.",
                    "Ok");
                IsEnabled = true;
            }
            else if (string.IsNullOrEmpty(this.Password))
            {
                
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must enter an password.",
                    "Ok");
                IsEnabled = true;
            }
            else if (this.Email != "dsanthony997@gmail.com" || this.Password != "123")
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    "Email or password incorrect.",
                    "Ok");
                IsEnabled = true;
                this.Password = string.Empty;
            }
            else
            {
                IsEnabled = true;

                this.Email = string.Empty;
                this.Password = string.Empty;
                MainViewModel.GetInstance().Lands = new LandsViewModel();
                await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());
            }
            IsRunning = false;
        }
        #endregion
    }
}
