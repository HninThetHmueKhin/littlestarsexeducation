using System.Windows;
using System.Windows.Controls;

namespace ChildSafeSexEducation.Desktop
{
    public partial class CreateTicketWindow : Window
    {
        public event Action<ErrorTicket>? TicketCreated;

        public CreateTicketWindow()
        {
            InitializeComponent();
            TitleTextBox.Focus();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleTextBox.Text))
            {
                MessageBox.Show("Please enter a title for the ticket.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                MessageBox.Show("Please enter a description for the ticket.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                DescriptionTextBox.Focus();
                return;
            }

            var ticket = new ErrorTicket
            {
                Id = GetNextTicketId(),
                Title = TitleTextBox.Text.Trim(),
                Description = DescriptionTextBox.Text.Trim(),
                Priority = ((ComboBoxItem)PriorityComboBox.SelectedItem).Tag.ToString()!,
                AssignedTo = ((ComboBoxItem)AssignedToComboBox.SelectedItem).Tag.ToString()!,
                Status = "Open",
                CreatedDate = DateTime.Now,
                Reporter = "admin"
            };

            TicketCreated?.Invoke(ticket);
            MessageBox.Show("Ticket created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private int GetNextTicketId()
        {
            // In a real application, this would get the next ID from a database
            return 100; // Placeholder
        }
    }
}
