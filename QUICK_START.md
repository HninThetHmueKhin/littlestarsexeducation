# 🚀 Quick Start Guide - Safe Learning Chatbot

## ⏱️ 15-Minute Setup

### Step 1: Install .NET SDK (5 minutes)
1. Download .NET 6.0 SDK from: https://dotnet.microsoft.com/download
2. Run the installer
3. Open Command Prompt and verify: `dotnet --version`

### Step 2: Run Setup (2 minutes)
1. Double-click `Setup/setup.bat`
2. Wait for "Setup completed successfully!"

### Step 3: Start the App (3 minutes)
1. Open Command Prompt
2. Navigate to Backend folder: `cd Backend`
3. Run: `dotnet run`
4. Wait for "Now listening on: http://localhost:5000"

### Step 4: Access the App (1 minute)
- **Desktop**: Open `Desktop/wrapper.html`
- **Web**: Go to `http://localhost:5000`
- **Mobile**: Use phone browser to `http://localhost:5000`

## 🎯 Features Overview

### Child-Safe Design
- ✅ Age verification (8-15 years)
- ✅ Content filtering
- ✅ Safe, educational responses
- ✅ No inappropriate content

### Topics Available
1. **Body Parts** - Learning about human body
2. **Personal Safety** - Understanding boundaries
3. **Growing Up** - Puberty and changes
4. **Healthy Relationships** - Friendships and safety

### User Flow
1. Enter name and age
2. Choose "Start Learning"
3. Browse topics or ask questions
4. Get age-appropriate answers

## 📱 Mobile & Desktop Support

### Desktop Version
- Open `Desktop/wrapper.html` for native app experience
- Full-screen interface
- Desktop controls and status

### Mobile Version
- Responsive design
- Touch-friendly interface
- Works on phones and tablets

## 🔧 Building Executable

To create a standalone .exe file:

```bash
# Run the build script
Setup/build-exe.bat

# Or manually:
cd Backend
dotnet publish -c Release -r win-x64 --self-contained true
```

## 🛡️ Security Features

- **Content Filtering**: Blocks inappropriate language
- **Age Restrictions**: Content filtered by age (8-15)
- **Safe Responses**: Pre-approved educational content
- **No External APIs**: All content is local and safe

## 🆘 Troubleshooting

### "dotnet command not found"
- Install .NET SDK from Microsoft website
- Restart Command Prompt

### "Port 5000 in use"
- Close other applications
- Or change port in Program.cs

### "Can't connect from phone"
- Use computer's IP address instead of localhost
- Find IP: `ipconfig` (Windows)
- Make sure phone is on same WiFi

## 📁 Project Structure

```
ChildSafeSexEducation/
├── Backend/           # .NET Web API
├── Frontend/          # Web Interface
├── Desktop/           # Desktop wrapper
├── Setup/             # Installation scripts
└── README.md          # Full documentation
```

## 🎨 Design Features

- **Green Theme**: Calming, child-friendly colors
- **Responsive**: Works on all screen sizes
- **Accessible**: Easy to read and navigate
- **Modern UI**: Clean, intuitive interface

## ✅ Ready to Use!

Your Safe Learning Chatbot is now ready! The application provides a secure, educational environment for children to learn about important topics in an age-appropriate way.

**Remember**: Always supervise children when using educational technology, and encourage open communication about any questions they may have.
