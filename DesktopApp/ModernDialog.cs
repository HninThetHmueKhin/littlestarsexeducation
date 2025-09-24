using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ChildSafeSexEducation.Desktop
{
    public partial class ModernDialog : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        public ModernDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Window properties
            Title = "Dialog";
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ResizeMode = ResizeMode.NoResize;
            Background = Brushes.Transparent;
            AllowsTransparency = true;
            WindowStyle = WindowStyle.None;
            SizeToContent = SizeToContent.WidthAndHeight;
            MinWidth = 300;
            MaxWidth = 500;

            // Main container
            var mainBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(255, 255, 255)), // White
                CornerRadius = new CornerRadius(16),
                Margin = new Thickness(10)
            };

            // Add shadow effect
            mainBorder.Effect = new DropShadowEffect
            {
                Color = Colors.Black,
                Direction = 270,
                ShadowDepth = 10,
                BlurRadius = 20,
                Opacity = 0.3
            };

            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Header
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); // Content
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); // Buttons

            // Header
            var headerBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0, 212, 170)), // PrimaryGreen
                CornerRadius = new CornerRadius(16, 16, 0, 0),
                Padding = new Thickness(20, 12, 20, 12)
            };

            var headerGrid = new Grid();
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Icon
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Title
            headerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto }); // Close button

            // Icon
            var iconText = new TextBlock
            {
                Name = "IconText",
                FontSize = 24,
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 0, 15, 0)
            };

            // Title
            var titleText = new TextBlock
            {
                Name = "TitleText",
                Text = "Dialog",
                FontSize = 18,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.White,
                VerticalAlignment = VerticalAlignment.Center
            };

            // Close button
            var closeButton = new Button
            {
                Name = "CloseButton",
                Content = "✕",
                Background = Brushes.Transparent,
                Foreground = Brushes.White,
                FontSize = 16,
                FontWeight = FontWeights.Bold,
                Width = 30,
                Height = 30,
                Cursor = Cursors.Hand
            };
            closeButton.Click += CloseButton_Click;

            headerGrid.Children.Add(iconText);
            headerGrid.Children.Add(titleText);
            headerGrid.Children.Add(closeButton);

            Grid.SetColumn(iconText, 0);
            Grid.SetColumn(titleText, 1);
            Grid.SetColumn(closeButton, 2);

            headerBorder.Child = headerGrid;

            // Content
            var contentBorder = new Border
            {
                Padding = new Thickness(20, 15, 20, 15)
            };

            var messageText = new TextBlock
            {
                Name = "MessageText",
                Text = "Message",
                FontSize = 14,
                Foreground = new SolidColorBrush(Color.FromRgb(45, 52, 54)), // TextDark
                TextWrapping = TextWrapping.Wrap,
                LineHeight = 20,
                MaxWidth = 450 // Prevent text from being too wide
            };

            contentBorder.Child = messageText;

            // Buttons
            var buttonBorder = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(248, 249, 250)), // Light background
                CornerRadius = new CornerRadius(0, 0, 16, 16),
                Padding = new Thickness(20, 12, 20, 12)
            };

            var buttonPanel = new StackPanel
            {
                Name = "ButtonPanel",
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center
            };

            buttonBorder.Child = buttonPanel;

            // Add to grid
            mainGrid.Children.Add(headerBorder);
            mainGrid.Children.Add(contentBorder);
            mainGrid.Children.Add(buttonBorder);

            Grid.SetRow(headerBorder, 0);
            Grid.SetRow(contentBorder, 1);
            Grid.SetRow(buttonBorder, 2);

            mainBorder.Child = mainGrid;
            Content = mainBorder;

            // Store references for later use
            IconText = iconText;
            TitleText = titleText;
            MessageText = messageText;
            ButtonPanel = buttonPanel;
        }

        public TextBlock IconText { get; private set; } = null!;
        public TextBlock TitleText { get; private set; } = null!;
        public TextBlock MessageText { get; private set; } = null!;
        public StackPanel ButtonPanel { get; private set; } = null!;

        public static MessageBoxResult Show(string message, string title = "Dialog", 
            MessageBoxButton buttons = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.None)
        {
            var dialog = new ModernDialog();
            return dialog.ShowDialog(message, title, buttons, icon);
        }

        public MessageBoxResult ShowDialog(string message, string title, MessageBoxButton buttons, MessageBoxImage icon)
        {
            TitleText.Text = title;
            MessageText.Text = message;
            
            // Set icon
            SetIcon(icon);
            
            // Create buttons
            CreateButtons(buttons);
            
            // Show dialog
            ShowDialog();
            
            return Result;
        }

        private void SetIcon(MessageBoxImage icon)
        {
            IconText.Text = icon switch
            {
                MessageBoxImage.Error => "❌",
                MessageBoxImage.Warning => "⚠️",
                MessageBoxImage.Information => "ℹ️",
                MessageBoxImage.Question => "❓",
                _ => "ℹ️"
            };
        }

        private void CreateButtons(MessageBoxButton buttons)
        {
            ButtonPanel.Children.Clear();

            switch (buttons)
            {
                case MessageBoxButton.OK:
                    AddButton("OK", MessageBoxResult.OK, true);
                    break;
                case MessageBoxButton.OKCancel:
                    AddButton("Cancel", MessageBoxResult.Cancel, false);
                    AddButton("OK", MessageBoxResult.OK, true);
                    break;
                case MessageBoxButton.YesNo:
                    AddButton("No", MessageBoxResult.No, false);
                    AddButton("Yes", MessageBoxResult.Yes, true);
                    break;
                case MessageBoxButton.YesNoCancel:
                    AddButton("Cancel", MessageBoxResult.Cancel, false);
                    AddButton("No", MessageBoxResult.No, false);
                    AddButton("Yes", MessageBoxResult.Yes, true);
                    break;
            }
        }

        private void AddButton(string text, MessageBoxResult result, bool isPrimary)
        {
            var button = new Button
            {
                Content = text,
                Margin = new Thickness(8, 0, 0, 0),
                Padding = new Thickness(16, 6, 16, 6),
                FontSize = 13,
                FontWeight = FontWeights.Medium,
                MinWidth = 70,
                Height = 32,
                Cursor = Cursors.Hand
            };

            if (isPrimary)
            {
                button.Background = new SolidColorBrush(Color.FromRgb(0, 212, 170)); // PrimaryGreen
                button.Foreground = Brushes.White;
            }
            else
            {
                button.Background = new SolidColorBrush(Color.FromRgb(248, 249, 250)); // Light background
                button.Foreground = new SolidColorBrush(Color.FromRgb(45, 52, 54)); // TextDark
                button.BorderBrush = new SolidColorBrush(Color.FromRgb(206, 212, 218));
                button.BorderThickness = new Thickness(1);
            }

            button.Click += (s, e) =>
            {
                Result = result;
                Close();
            };

            ButtonPanel.Children.Add(button);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
