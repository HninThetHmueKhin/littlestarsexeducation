using System.Windows;
using System.Windows.Controls;

namespace ChildSafeSexEducation.Desktop
{
    public partial class EditTicketWindow : Window
    {
        private readonly ErrorTicket _ticket;
        public event Action<ErrorTicket>? TicketUpdated;

        public EditTicketWindow(ErrorTicket ticket)
        {
            InitializeComponent();
            _ticket = ticket;
            LoadTicketData();
        }

        private void LoadTicketData()
        {
            TitleTextBox.Text = _ticket.Title;
            DescriptionTextBox.Text = _ticket.Description;
            
            // Set status
            foreach (ComboBoxItem item in StatusComboBox.Items)
            {
                if (item.Tag.ToString() == _ticket.Status)
                {
                    StatusComboBox.SelectedItem = item;
                    break;
                }
            }
            
            // Set priority
            foreach (ComboBoxItem item in PriorityComboBox.Items)
            {
                if (item.Tag.ToString() == _ticket.Priority)
                {
                    PriorityComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
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

            // Update ticket
            _ticket.Title = TitleTextBox.Text.Trim();
            _ticket.Description = DescriptionTextBox.Text.Trim();
            _ticket.Status = ((ComboBoxItem)StatusComboBox.SelectedItem).Tag.ToString()!;
            _ticket.Priority = ((ComboBoxItem)PriorityComboBox.SelectedItem).Tag.ToString()!;

            TicketUpdated?.Invoke(_ticket);
            MessageBox.Show("Ticket updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
