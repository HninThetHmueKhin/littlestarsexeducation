# Child Safe Sex Education - Desktop Application Guide

## 📋 Table of Contents
1. [Overview](#overview)
2. [System Requirements](#system-requirements)
3. [Installation Guide](#installation-guide)
4. [Features](#features)
5. [User Guide](#user-guide)
6. [Technical Details](#technical-details)
7. [Troubleshooting](#troubleshooting)
8. [Support](#support)

## 🎯 Overview

The Child Safe Sex Education Desktop Application is a Windows-based educational platform designed to provide age-appropriate, culturally sensitive sex education for children aged 8-15 years. Built using Windows Presentation Foundation (WPF) and .NET 6, the application features modern UI design, multilingual support (English/Burmese), and AI-powered educational content delivery.

## 💻 System Requirements

### Minimum Requirements
- **Operating System**: Windows 10 (version 1903 or later) or Windows 11
- **Processor**: Intel Core i3 or AMD equivalent
- **Memory**: 4 GB RAM
- **Storage**: 100 MB available space
- **Display**: 1024x768 resolution
- **.NET Runtime**: .NET 6.0 Desktop Runtime

### Recommended Requirements
- **Operating System**: Windows 11
- **Processor**: Intel Core i5 or AMD equivalent
- **Memory**: 8 GB RAM
- **Storage**: 500 MB available space
- **Display**: 1920x1080 resolution or higher
- **.NET Runtime**: .NET 6.0 Desktop Runtime

## 🚀 Installation Guide

### Method 1: Direct Installation (Recommended)
1. **Download the Application**
   - Download `ChildSafeSexEducation.Desktop.exe` from the releases page
   - Save to your desired location (e.g., Desktop or Program Files)

2. **Install .NET 6.0 Runtime** (if not already installed)
   - Download from: https://dotnet.microsoft.com/download/dotnet/6.0
   - Select "Desktop Runtime" for Windows
   - Run the installer and follow the setup wizard

3. **Run the Application**
   - Double-click `ChildSafeSexEducation.Desktop.exe`
   - The application will start automatically

### Method 2: Build from Source
1. **Prerequisites**
   - Install Visual Studio 2022 (Community Edition or higher)
   - Install .NET 6.0 SDK
   - Install Windows App SDK

2. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/ChildSafeSexEducation.git
   cd ChildSafeSexEducation/DesktopApp
   ```

3. **Build the Application**
   ```bash
   dotnet restore
   dotnet build --configuration Release
   ```

4. **Run the Application**
   ```bash
   dotnet run
   ```

## ✨ Features

### Core Features
- **🌍 Multilingual Support**: English and Burmese language options
- **👤 User Registration**: Age-appropriate user profile creation (8-15 years)
- **📚 Educational Topics**: Four main categories with age-filtered content
- **🤖 AI Chat System**: Interactive educational bot with safety filtering
- **💬 Modern Dialogs**: Custom dialog system with translated messages
- **📧 Email Integration**: Daily log notifications for parents/guardians
- **🔒 Safety Features**: Content filtering and inappropriate content detection

### Educational Content
- **Body Parts**: Basic anatomy and hygiene education
- **Personal Safety**: Boundaries and safety awareness
- **Growing Up**: Developmental changes and puberty
- **Friendships**: Healthy relationships and social skills

## 📖 User Guide

### Getting Started

#### Step 1: Language Selection
1. Launch the application
2. Choose your preferred language:
   - **English**: Click "English" button
   - **မြန်မာ (Burmese)**: Click "မြန်မာ" button
3. The interface will update to your selected language

#### Step 2: User Registration
1. Enter your name in the "What's your name?" field
2. Select your age from the dropdown menu (8-15 years)
3. Click "Start Learning" to proceed
4. The application will validate your input and create your profile

#### Step 3: Main Interface
1. **Welcome Message**: View personalized greeting
2. **Chat Interface**: Type questions in the input field
3. **Topics Button**: Click "📚 Topics" to browse educational content
4. **Send Button**: Click "Send" to submit your message

### Using the Topics System

#### Browsing Topics
1. Click the "📚 Topics" button on the main page
2. View available educational categories:
   - **Body Parts**: Learn about anatomy and hygiene
   - **Personal Safety**: Understand boundaries and safety
   - **Growing Up**: Explore developmental changes
   - **Friendships**: Learn about healthy relationships
3. Click on any topic card to view related questions

#### Viewing Questions
1. Select a topic from the topics page
2. View available questions for that topic
3. Click on any question to see the detailed answer
4. Use the back button to return to topics

#### Chat System
1. Type your question in the chat input field
2. Click "Send" or press Enter
3. The AI bot will provide an age-appropriate response
4. Continue the conversation by asking follow-up questions

### Advanced Features

#### Email Notifications
1. **Daily Log**: Parents can receive daily activity summaries
2. **Test Email**: Verify email functionality
3. **Email Validation**: Automatic Gmail recommendation

#### Error Handling
- **Validation Errors**: Clear messages for invalid input
- **System Errors**: User-friendly error dialogs
- **Recovery**: Automatic error recovery and retry options

## 🔧 Technical Details

### Architecture
- **Framework**: Windows Presentation Foundation (WPF)
- **Language**: C# (.NET 6)
- **UI Pattern**: MVVM (Model-View-ViewModel)
- **Data Storage**: JSON files
- **Email**: SMTP integration

### Key Components
- **MainWindow.xaml**: Main application interface
- **WelcomePage.xaml**: Language selection and registration
- **ModernDialog.cs**: Custom dialog system
- **LanguageService.cs**: Translation management
- **ContentService.cs**: Educational content delivery
- **NLPService.cs**: AI chat functionality

### File Structure
```
DesktopApp/
├── MainWindow.xaml          # Main interface
├── MainWindow.xaml.cs       # Main interface logic
├── ModernDialog.cs          # Custom dialog system
├── ModernMessageBox.cs      # Dialog helper class
├── Services/
│   ├── LanguageService.cs   # Translation service
│   ├── ContentService.cs    # Content management
│   ├── NLPService.cs        # AI chat service
│   ├── EmailService.cs      # Email functionality
│   └── UserStorageService.cs # Data persistence
├── Models/
│   ├── User.cs              # User data model
│   └── Topic.cs             # Educational content model
└── appsettings.json         # Configuration file
```

## 🛠️ Troubleshooting

### Common Issues

#### Application Won't Start
- **Issue**: Application fails to launch
- **Solution**: 
  1. Ensure .NET 6.0 Desktop Runtime is installed
  2. Check Windows version compatibility
  3. Run as administrator if needed

#### Language Not Changing
- **Issue**: Language selection doesn't update interface
- **Solution**:
  1. Restart the application
  2. Check if translation files are present
  3. Verify language selection was successful

#### Chat Not Responding
- **Issue**: AI chat system not providing responses
- **Solution**:
  1. Check internet connection
  2. Restart the application
  3. Verify content files are present

#### Email Not Working
- **Issue**: Email notifications not sending
- **Solution**:
  1. Check email configuration in appsettings.json
  2. Verify SMTP settings
  3. Check firewall settings

### Performance Issues

#### Slow Loading
- **Solution**:
  1. Close other applications
  2. Check available memory
  3. Restart the application

#### High Memory Usage
- **Solution**:
  1. Restart the application
  2. Check for memory leaks
  3. Update to latest version

### Error Messages

#### "Missing Information"
- **Cause**: Incomplete user registration
- **Solution**: Fill in all required fields (name and age)

#### "Invalid Age"
- **Cause**: Age outside 8-15 range
- **Solution**: Select age between 8-15 years

#### "Invalid Email"
- **Cause**: Incorrect email format
- **Solution**: Enter valid email address

## 📞 Support

### Getting Help
- **Documentation**: Check this guide for common issues
- **GitHub Issues**: Report bugs and request features
- **Email Support**: Contact support team

### Reporting Issues
When reporting issues, please include:
1. **Operating System**: Windows version
2. **Application Version**: Version number
3. **Error Message**: Exact error text
4. **Steps to Reproduce**: How the issue occurred
5. **Screenshots**: Visual evidence if applicable

### Feature Requests
- **GitHub Issues**: Submit feature requests
- **Community Forum**: Discuss ideas with other users
- **Email**: Send detailed feature descriptions

## 📄 License

This application is licensed under the MIT License. See LICENSE file for details.

## 🔄 Updates

### Checking for Updates
- **Automatic**: Application checks for updates on startup
- **Manual**: Download latest version from GitHub releases
- **Notifications**: Update notifications in the application

### Update Process
1. Download latest version
2. Close current application
3. Install new version
4. Restart application

---

**Version**: 1.0.0  
**Last Updated**: December 2024  
**Compatibility**: Windows 10/11, .NET 6.0
