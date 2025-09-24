; Safe Learning Chatbot Installer Script
; Requires Inno Setup to compile

[Setup]
AppName=Safe Learning Chatbot
AppVersion=1.0.0
AppPublisher=Safe Learning Solutions
AppPublisherURL=https://example.com
AppSupportURL=https://example.com/support
AppUpdatesURL=https://example.com/updates
DefaultDirName={autopf}\SafeLearningChatbot
DefaultGroupName=Safe Learning Chatbot
AllowNoIcons=yes
LicenseFile=LICENSE.txt
OutputDir=dist
OutputBaseFilename=SafeLearningChatbot-Setup
SetupIconFile=icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 6.1

[Files]
Source: "..\DesktopApp\bin\Release\net6.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "README.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\Safe Learning Chatbot"; Filename: "{app}\ChildSafeSexEducation.Desktop.exe"
Name: "{group}\{cm:UninstallProgram,Safe Learning Chatbot}"; Filename: "{uninstallexe}"
Name: "{autodesktop}\Safe Learning Chatbot"; Filename: "{app}\ChildSafeSexEducation.Desktop.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\Safe Learning Chatbot"; Filename: "{app}\ChildSafeSexEducation.Desktop.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\ChildSafeSexEducation.Desktop.exe"; Description: "{cm:LaunchProgram,Safe Learning Chatbot}"; Flags: nowait postinstall skipifsilent
