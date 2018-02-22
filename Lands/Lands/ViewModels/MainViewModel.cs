namespace Lands.ViewModels
{
    public class MainViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }
        public LandsViewModel Lands { get; set; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            _instance = this;
            this.Login = new LoginViewModel();
        }
        #endregion
        #region Singleton
        private static MainViewModel _instance;

        public static MainViewModel GetInstance()
        {
            MainViewModel instance = _instance ?? new MainViewModel();
            return instance;
        }
        #endregion
    }
}
