using System.Windows;

namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ViewModelLocator.Init();
            ChangeTheme(new Uri("pack://application:,,,/Themes/LightTheme.xaml"));
        }

        public static void ChangeTheme(Uri themeUri)
        {
            ResourceDictionary newTheme = new ResourceDictionary() { Source = themeUri };

            var mergedDictionaries = Current.Resources.MergedDictionaries;

            if (mergedDictionaries.Count > 0)
            {
                mergedDictionaries.Clear();
            }

            mergedDictionaries.Add(newTheme);
        }
    }
}
