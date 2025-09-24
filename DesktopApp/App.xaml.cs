using System.Windows;

namespace ChildSafeSexEducation.Desktop
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Set application properties
            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }
    }
}
