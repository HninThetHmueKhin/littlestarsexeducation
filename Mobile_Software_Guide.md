# Child Safe Sex Education - Mobile Application Guide

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

The Child Safe Sex Education Mobile Application is a cross-platform educational platform built using .NET Multi-platform App UI (MAUI). It provides age-appropriate, culturally sensitive sex education for children aged 8-15 years on both iOS and Android devices. The application features modern UI design, multilingual support (English/Burmese), and AI-powered educational content delivery optimized for mobile devices.

## 📱 System Requirements

### Android Requirements
- **Operating System**: Android 7.0 (API level 24) or higher
- **RAM**: 2 GB minimum, 4 GB recommended
- **Storage**: 100 MB available space
- **Screen Resolution**: 720x1280 minimum
- **Architecture**: ARM64, ARMv7, or x86_64

### iOS Requirements
- **Operating System**: iOS 11.0 or higher
- **Device**: iPhone 6s or newer, iPad Air 2 or newer
- **RAM**: 2 GB minimum, 4 GB recommended
- **Storage**: 100 MB available space
- **Architecture**: ARM64

### Development Requirements
- **Visual Studio 2022**: Community Edition or higher
- **.NET 6.0 SDK**: Latest version
- **MAUI Workload**: .NET Multi-platform App UI workload
- **Android SDK**: For Android development
- **Xcode**: For iOS development (Mac only)

## 🚀 Installation Guide

### For End Users

#### Android Installation
1. **Download APK**
   - Download `ChildSafeSexEducation.Mobile.apk` from releases
   - Enable "Install from unknown sources" in Android settings
   - Tap the APK file to install

2. **Google Play Store** (when available)
   - Search for "Child Safe Sex Education"
   - Tap "Install" button
   - Follow on-screen instructions

#### iOS Installation
1. **App Store** (when available)
   - Search for "Child Safe Sex Education"
   - Tap "Get" button
   - Follow on-screen instructions

2. **TestFlight** (Beta testing)
   - Install TestFlight app
   - Use invitation link to install beta version

### For Developers

#### Prerequisites Setup
1. **Install Visual Studio 2022**
   - Download from: https://visualstudio.microsoft.com/
   - Select ".NET Multi-platform App UI development" workload

2. **Install .NET 6.0 SDK**
   ```bash
   dotnet --version
   ```

3. **Install MAUI Workload**
   ```bash
   dotnet workload install maui
   ```

#### Build from Source
1. **Clone Repository**
   ```bash
   git clone https://github.com/yourusername/ChildSafeSexEducation.git
   cd ChildSafeSexEducation/MobileApp
   ```

2. **Restore Dependencies**
   ```bash
   dotnet restore
   ```

3. **Build Application**
   ```bash
   # For Android
   dotnet build -f net6.0-android
   
   # For iOS (Mac only)
   dotnet build -f net6.0-ios
   ```

4. **Run Application**
   ```bash
   # Android
   dotnet run -f net6.0-android

   # iOS (Mac only)
   dotnet run -f net6.0-ios
   ```

## ✨ Features

### Core Features
- **🌍 Multilingual Support**: English and Burmese language options
- **👤 User Registration**: Age-appropriate user profile creation (8-15 years)
- **📚 Educational Topics**: Four main categories with age-filtered content
- **🤖 AI Chat System**: Interactive educational bot with safety filtering
- **💬 Modern Dialogs**: Custom dialog system with translated messages
- **📱 Mobile Optimized**: Touch-friendly interface and responsive design
- **🔒 Safety Features**: Content filtering and inappropriate content detection

### Mobile-Specific Features
- **Touch Navigation**: Swipe and tap gestures
- **Responsive Design**: Adapts to different screen sizes
- **Offline Support**: Works without internet connection
- **Battery Optimization**: Efficient power usage
- **Push Notifications**: Educational reminders (future feature)

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
   - **English**: Tap "English" button
   - **မြန်မာ (Burmese)**: Tap "မြန်မာ" button
3. The interface will update to your selected language

#### Step 2: User Registration
1. Enter your name in the "What's your name?" field
2. Select your age from the picker (8-15 years)
3. Tap "Start Learning" to proceed
4. The application will validate your input and create your profile

#### Step 3: Main Interface
1. **Welcome Message**: View personalized greeting
2. **Chat Interface**: Type questions in the input field
3. **Topics Button**: Tap "📚 Topics" to browse educational content
4. **Send Button**: Tap "Send" to submit your message

### Using the Topics System

#### Browsing Topics
1. Tap the "📚 Topics" button on the main page
2. View available educational categories in card format:
   - **Body Parts**: Learn about anatomy and hygiene
   - **Personal Safety**: Understand boundaries and safety
   - **Growing Up**: Explore developmental changes
   - **Friendships**: Learn about healthy relationships
3. Tap on any topic card to view related questions

#### Viewing Questions
1. Select a topic from the topics page
2. View available questions for that topic
3. Tap on any question to see the detailed answer
4. Use the back button to return to topics

#### Chat System
1. Type your question in the chat input field
2. Tap "Send" or press Enter
3. The AI bot will provide an age-appropriate response
4. Continue the conversation by asking follow-up questions

### Mobile Navigation

#### Touch Gestures
- **Tap**: Select items and buttons
- **Swipe**: Navigate between pages
- **Pinch**: Zoom in/out (if supported)
- **Long Press**: Access additional options

#### Back Navigation
- **Back Button**: Return to previous page
- **Swipe Back**: Swipe from left edge to go back
- **Home Button**: Return to main page

## 🔧 Technical Details

### Architecture
- **Framework**: .NET Multi-platform App UI (MAUI)
- **Language**: C# (.NET 6)
- **UI Pattern**: MVVM (Model-View-ViewModel)
- **Data Storage**: JSON files
- **Platform**: iOS and Android

### Key Components
- **MainPage.xaml**: Main application interface
- **WelcomePage.xaml**: Language selection and registration
- **TopicsPage.xaml**: Educational topics display
- **QuestionsPage.xaml**: Questions interface
- **AnswerPage.xaml**: Answer display
- **Services/**: Business logic and data services

### File Structure
```
MobileApp/
├── Views/
│   ├── MainPage.xaml          # Main interface
│   ├── WelcomePage.xaml       # Language selection
│   ├── TopicsPage.xaml        # Topics display
│   ├── QuestionsPage.xaml     # Questions interface
│   └── AnswerPage.xaml        # Answer display
├── Services/
│   ├── LanguageService.cs     # Translation service
│   ├── ContentService.cs      # Content management
│   ├── NLPService.cs          # AI chat service
│   └── ModernDialogService.cs # Dialog system
├── Models/
│   ├── User.cs                # User data model
│   └── Topic.cs               # Educational content model
└── ChildSafeSexEducation.Mobile.csproj
```

### Platform-Specific Features
- **Android**: Material Design components, Android-specific optimizations
- **iOS**: iOS design guidelines, native iOS components
- **Shared**: Common UI components and business logic

## 🛠️ Troubleshooting

### Common Issues

#### Application Won't Start
- **Issue**: Application fails to launch
- **Solution**: 
  1. Ensure device meets minimum requirements
  2. Restart the device
  3. Reinstall the application
  4. Check available storage space

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

#### Performance Issues
- **Issue**: Slow performance or crashes
- **Solution**:
  1. Close other applications
  2. Restart the device
  3. Check available memory
  4. Update to latest version

### Android-Specific Issues

#### Installation Failed
- **Issue**: APK installation fails
- **Solution**:
  1. Enable "Install from unknown sources"
  2. Check APK file integrity
  3. Ensure sufficient storage space

#### Permission Denied
- **Issue**: App requests unnecessary permissions
- **Solution**:
  1. Check app permissions in settings
  2. Grant required permissions
  3. Reinstall if necessary

### iOS-Specific Issues

#### Installation Failed
- **Issue**: App installation fails
- **Solution**:
  1. Check iOS version compatibility
  2. Ensure sufficient storage space
  3. Restart device and try again

#### App Crashes
- **Issue**: Application crashes unexpectedly
- **Solution**:
  1. Force close and restart app
  2. Restart device
  3. Update to latest version

### Error Messages

#### "Missing Information"
- **Cause**: Incomplete user registration
- **Solution**: Fill in all required fields (name and age)

#### "Invalid Age"
- **Cause**: Age outside 8-15 range
- **Solution**: Select age between 8-15 years

#### "Network Error"
- **Cause**: Internet connection issues
- **Solution**: Check internet connection and try again

## 📞 Support

### Getting Help
- **Documentation**: Check this guide for common issues
- **GitHub Issues**: Report bugs and request features
- **Email Support**: Contact support team

### Reporting Issues
When reporting issues, please include:
1. **Device Information**: Make, model, OS version
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
- **App Store/Play Store**: Update through official stores

### Update Process
1. Download latest version
2. Close current application
3. Install new version
4. Restart application

## 🚀 Deployment

### Android Deployment
1. **Debug Build**
   ```bash
   dotnet build -f net6.0-android -c Debug
   ```

2. **Release Build**
   ```bash
   dotnet build -f net6.0-android -c Release
   ```

3. **Generate APK**
   ```bash
   dotnet publish -f net6.0-android -c Release
   ```

### iOS Deployment
1. **Debug Build**
   ```bash
   dotnet build -f net6.0-ios -c Debug
   ```

2. **Release Build**
   ```bash
   dotnet build -f net6.0-ios -c Release
   ```

3. **Archive for App Store**
   - Use Visual Studio or Xcode
   - Follow Apple's submission guidelines

---

**Version**: 1.0.0  
**Last Updated**: December 2024  
**Compatibility**: Android 7.0+, iOS 11.0+, .NET 6.0
