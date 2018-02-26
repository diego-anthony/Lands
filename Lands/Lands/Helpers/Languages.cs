using Xamarin.Forms;
using Lands.Interfaces;
using Lands.Resources;

namespace Lands.Helpers
{

    public static class Languages
    {
        static Languages()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Accept
        {
            get { return Resource.Accept; }
        }
        public static string EmailValidation
        {
            get { return Resource.EmailValidation; }
        }
        public static string PasswordValidation
        {
            get { return Resource.PasswordValidation; }
        }
        public static string OtherError
        {
            get { return Resource.OtherError; }
        }
        public static string Error
        {
            get { return Resource.Error; }
        }
        public static string EmailPlaceHolder
        {
            get { return Resource.EmailPlaceHolder; }
        }
        public static string Rememberme
        {
            get { return Resource.Rememberme; }
        }
    }

}
