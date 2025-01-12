#define public Dependency_NoExampleSetup
#include "CodeDependencies.iss"

[Setup]
AppName=HunterPie
WizardStyle=modern
DefaultDirName={autopf}\HunterPie
AppVersion=Latest
DefaultGroupName=HunterPie
UninstallDisplayIcon={app}\HunterPie.exe
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
OutputDir=installer

[Dirs]
Name: {app}; Permissions: users-modify

[UninstallDelete]
Type: filesandordirs; Name: "{app}\*"

[Files]
Source: "WorkSans-ExtraLight.ttf"; DestDir: "{autofonts}"; FontInstall: "Work Sans ExtraLight"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "WorkSans-Light.ttf"; DestDir: "{autofonts}"; FontInstall: "Work Sans Light"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "WorkSans-Medium.ttf"; DestDir: "{autofonts}"; FontInstall: "Work Sans Medium"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "WorkSans-SemiBold.ttf"; DestDir: "{autofonts}"; FontInstall: "Work Sans SemiBold"; Flags: onlyifdoesntexist uninsneveruninstall
Source: "WorkSans.ttf"; DestDir: "{autofonts}"; FontInstall: "Work Sans"; Flags: onlyifdoesntexist uninsneveruninstall

[Icons]
Name: "{commondesktop}\HunterPie"; Filename: "{app}\HunterPie.exe"; WorkingDir: "{app}"

[Code]
var
  DownloadPage: TDownloadWizardPage;

function OnDownloadProgress(const Url, FileName: String; const Progress, ProgressMax: Int64): Boolean;
begin
  if Progress = ProgressMax then
    Log(Format('Successfully downloaded file to {tmp}: %s', [FileName]));
  Result := True
end;

procedure InitializeWizard;
begin
  Dependency_AddDotNet80Desktop
  DownloadPage := CreateDownloadPage(SetupMessage(msgWizardPreparing), SetupMessage(msgPreparingDesc), @OnDownloadProgress);
end;

const
  SHCONTCH_NOPROGRESSBOX = 4;
  SHCONTCH_RESPONDYESTOALL = 16;

procedure Unzip(ZipFile, TargetFolder: String);
var
  shellobj: variant;
  ZipFileV, TargetFolderV: variant;
  SrcFolder, DestFolder: variant;
  shellFolderItems: variant;
begin
  if FileExists(ZipFile) then begin
    ForceDirectories(TargetFolder);
    shellobj := CreateOleObject('Shell.Application');
    ZipFileV := string(ZipFile);
    TargetFolderV := string(TargetFolder);
    SrcFolder := shellobj.NameSpace(ZipFileV);
    DestFolder := shellobj.NameSpace(TargetFolderV);
    shellFolderItems := SrcFolder.Items;
    DestFolder.CopyHere(shellFolderItems, SHCONTCH_NOPROGRESSBOX or SHCONTCH_RESPONDYESTOALL);
  end;
end;

procedure ExtractFile(src, target: String);
begin
  Unzip(ExpandConstant(src), ExpandConstant(target));
end;

function NextButtonClick(CurPageID: Integer): Boolean;
begin
  if CurPageID = wpReady then begin
    DownloadPage.Clear;
    DownloadPage.Add('https://api.hunterpie.com/v1/version/latest', 'latest.zip', '');
    DownloadPage.Show;
    try
      try
        DownloadPage.Download
        ExtractFile('{tmp}\latest.zip', '{app}');
        Result := true
      except
        if DownloadPage.AbortedByUser then
          Log('Aborted by user.')
        else
          SuppressibleMsgBox(AddPeriod(GetExceptionMessage), mbCriticalError, MB_OK, IDOK);
        Result := false
      end;
    finally
      DownloadPage.Hide;
    end;
  end else
    Result := True;
end;

