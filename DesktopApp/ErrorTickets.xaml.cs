using ChildSafeSexEducation.Desktop.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ChildSafeSexEducation.Desktop
{
    public partial class ErrorTickets : UserControl
    {
        private ObservableCollection<ErrorTicket> _tickets;

        public ErrorTickets()
        {
            InitializeComponent();
            _tickets = new ObservableCollection<ErrorTicket>();
            
            LoadSampleTickets();
        }

        private void LoadSampleTickets()
        {
            // Sample tickets for demonstration
            _tickets.Add(new ErrorTicket
            {
                Id = 1,
                Title = "Login Issue",
                Description = "User reported unable to login with valid credentials",
                Status = "Open",
                Priority = "High",
                CreatedDate = DateTime.Now.AddDays(-2),
                AssignedTo = "Admin",
                Reporter = "john_doe"
            });

            _tickets.Add(new ErrorTicket
            {
                Id = 2,
                Title = "Translation Missing",
                Description = "Some Burmese translations are not displaying correctly",
                Status = "In Progress",
                Priority = "Medium",
                CreatedDate = DateTime.Now.AddDays(-1),
                AssignedTo = "Developer",
                Reporter = "admin"
            });

            _tickets.Add(new ErrorTicket
            {
                Id = 3,
                Title = "Email Not Sending",
                Description = "Daily log emails are not being sent to parents",
                Status = "Open",
                Priority = "High",
                CreatedDate = DateTime.Now.AddHours(-5),
                AssignedTo = "Admin",
                Reporter = "system"
            });

            _tickets.Add(new ErrorTicket
            {
                Id = 4,
                Title = "UI Layout Issue",
                Description = "Content editor dialog is too small on high DPI displays",
                Status = "Resolved",
                Priority = "Low",
                CreatedDate = DateTime.Now.AddDays(-3),
                AssignedTo = "Developer",
                Reporter = "admin"
            });

            TicketsDataGrid.ItemsSource = _tickets;
        }

        private void CreateTicketButton_Click(object sender, RoutedEventArgs e)
        {
            var createTicketWindow = new CreateTicketWindow();
            createTicketWindow.TicketCreated += (ticket) =>
            {
                _tickets.Add(ticket);
            };
            createTicketWindow.ShowDialog();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSampleTickets();
        }

        private void ViewTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ErrorTicket ticket)
            {
                var ticketDetails = $"Ticket Details:\n\n" +
                                  $"ID: {ticket.Id}\n" +
                                  $"Title: {ticket.Title}\n" +
                                  $"Description: {ticket.Description}\n" +
                                  $"Status: {ticket.Status}\n" +
                                  $"Priority: {ticket.Priority}\n" +
                                  $"Created: {ticket.CreatedDate:MM/dd/yyyy HH:mm}\n" +
                                  $"Assigned To: {ticket.AssignedTo}\n" +
                                  $"Reporter: {ticket.Reporter}";
                
                MessageBox.Show(ticketDetails, "Ticket Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ErrorTicket ticket)
            {
                var editTicketWindow = new EditTicketWindow(ticket);
                editTicketWindow.TicketUpdated += (updatedTicket) =>
                {
                    var index = _tickets.IndexOf(ticket);
                    if (index >= 0)
                    {
                        _tickets[index] = updatedTicket;
                    }
                };
                editTicketWindow.ShowDialog();
            }
        }

        private void ResolveTicketButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ErrorTicket ticket)
            {
                var result = MessageBox.Show($"Mark ticket '{ticket.Title}' as resolved?", 
                                           "Resolve Ticket", 
                                           MessageBoxButton.YesNo, 
                                           MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    ticket.Status = "Resolved";
                    ticket.ResolvedDate = DateTime.Now;
                    
                    // Refresh the display
                    TicketsDataGrid.ItemsSource = null;
                    TicketsDataGrid.ItemsSource = _tickets;
                }
            }
        }
    }

    public class ErrorTicket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = "Open";
        public string Priority { get; set; } = "Medium";
        public DateTime CreatedDate { get; set; }
        public DateTime? ResolvedDate { get; set; }
        public string AssignedTo { get; set; } = string.Empty;
        public string Reporter { get; set; } = string.Empty;
    }
}
