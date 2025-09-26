using System.Windows;

namespace ChildSafeSexEducation.Desktop
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Console.WriteLine("🚀 App.OnStartup starting...");
                base.OnStartup(e);
                Console.WriteLine("✅ App.OnStartup completed successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in App.OnStartup: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                MessageBox.Show($"Application startup error: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }
    }
}
