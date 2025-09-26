# Little Star - Simple Android App

A simplified Android version of the Child-Safe Sex Education application.

## Features

- **User Registration**: Name and age validation (8-15 years)
- **Interactive Chat**: AI-powered responses about safe topics
- **Learning Topics**: 4 main categories with descriptions
- **Modern UI**: Material Design with child-friendly colors

## Quick Start

### Prerequisites
- Android Studio or Android SDK
- Java 8 or higher
- Android device or emulator

### Build Instructions

1. **Open in Android Studio**:
   - Open Android Studio
   - Select "Open an existing project"
   - Navigate to this folder

2. **Build APK**:
   ```bash
   # Windows
   build-android.bat
   
   # Or manually
   gradlew assembleDebug
   ```

3. **Install on Device**:
   ```bash
   adb install app/build/outputs/apk/debug/app-debug.apk
   ```

### Features Included

- ✅ User registration with age validation
- ✅ Interactive chat interface
- ✅ 4 learning topics (Body Parts, Safety, Growing Up, Friendships)
- ✅ Simple AI responses based on keywords
- ✅ Material Design UI
- ✅ Child-friendly color scheme

### Topics Covered

1. **Body Parts** 🧸 - Learning about your body and keeping clean
2. **Personal Safety** 🛡️ - Important safety tips for children
3. **Growing Up** 🌱 - What to expect as you grow older
4. **Healthy Relationships** 👫 - How to build healthy friendships

### How to Use

1. Enter your name and age (8-15 years)
2. Start chatting with the AI about safe topics
3. Click "Topics" to explore learning categories
4. Ask questions about body parts, safety, growing up, or friendships

## Technical Details

- **Language**: Kotlin
- **UI Framework**: Android Views with Material Design
- **Minimum SDK**: 21 (Android 5.0)
- **Target SDK**: 34 (Android 14)
- **Architecture**: Simple MVC pattern

## File Structure

```
app/
├── src/main/
│   ├── java/com/littlestar/childsafesexeducation/
│   │   ├── MainActivity.kt          # Main activity
│   │   ├── Models.kt                # Data models
│   │   ├── ChatAdapter.kt           # Chat messages adapter
│   │   └── TopicAdapter.kt          # Topics list adapter
│   ├── res/
│   │   ├── layout/                  # UI layouts
│   │   ├── values/                  # Strings, colors, themes
│   │   └── drawable/                # Icons and backgrounds
│   └── AndroidManifest.xml
├── build.gradle                     # App dependencies
└── proguard-rules.pro
```

This is a simplified version that can be built and deployed in 15 minutes!
