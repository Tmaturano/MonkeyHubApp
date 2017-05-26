using Version.Plugin;

namespace MonkeyHubApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        //Install-Package Xam.Plugin.Version
        public string Versao => CrossVersion.Current.Version;
    }
}
