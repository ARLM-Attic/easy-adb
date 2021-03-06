; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Easy ADB"
#define MyAppVersion "0.1"
#define MyAppURL "https://easyadbandroid.codeplex.com/"
#define MyAppExeName "easyADB.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application.
; Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{1882A93F-B04B-4DF5-8294-ECD8977BF454}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=C:\Users\Thomas\Documents\Visual Studio 2010\Projects\easyADB\easyADB\Installer\version 0.1
OutputBaseFilename=setup Easy ADB V0.1
SetupIconFile=C:\Users\Thomas\Documents\Visual Studio 2010\Projects\easyADB\easyADB\android.ico
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[dirs]
name: "adb"
name: "script"

[Files]
Source: "C:\Users\Thomas\Documents\Visual Studio 2010\Projects\easyADB\easyADB\bin\Release\easyADB.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\Thomas\Documents\Visual Studio 2010\Projects\easyADB\easyADB\bin\Release\adb\*"; DestDir: "{app}\adb"; Flags: ignoreversion
Source: "C:\Users\Thomas\Documents\Visual Studio 2010\Projects\easyADB\easyADB\bin\Release\script\*"; DestDir: "{app}\script"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

