using System.Windows;
using ChildSafeSexEducation.Desktop.Models;
using ChildSafeSexEducation.Desktop.Services;

namespace ChildSafeSexEducation.Desktop
{
    public partial class BlogWindow : Window
    {
        private readonly Blog _blog;
        private readonly LanguageService _languageService;

        public BlogWindow(Blog blog, LanguageService languageService)
        {
            InitializeComponent();
            _blog = blog;
            _languageService = languageService;
            LoadBlogContent();
        }

        private void LoadBlogContent()
        {
            BlogTitle.Text = _blog.Title;
            BlogCategory.Text = $"Category: {_blog.Category}";
            BlogDescription.Text = _blog.Description;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
