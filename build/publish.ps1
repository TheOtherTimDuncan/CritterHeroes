$Version = Read-Host "Version: "

$Target = ".\publish"
$Temp = ".\temp"
$Assembly = ".\SharedAssemblyInfo.cs"

# Cleanup
Remove-Item .\publish -Force -Recurse -ErrorAction SilentlyContinue
Remove-Item .\temp -Force -Recurse -ErrorAction SilentlyContinue

# Update version

Write-Host "`nSetting version in $($Assembly) to $($Version)`n" -ForegroundColor Yellow

$AssemblyVersion = 'AssemblyVersion("' + $Version + '")';
$FileVersion = 'AssemblyFileVersion("' + $Version + '")';

(Get-Content $Assembly) | ForEach-Object  { 
           % {$_ -replace 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $AssemblyVersion } |
           % {$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $FileVersion }
} | Set-Content -Path $Assembly -Encoding UTF8 -Force

# Create release build and deploy to temp location
& "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\msbuild.exe" ".\..\CritterHeroes.sln" /t:Clean,Rebuild,Publish /p:"Configuration=Release,Platform=Any CPU,DeployOnBuild=true,PublishProfile=Release" /verbosity:minimal

# Pre-compile views
& "C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\aspnet_compiler.exe" -v / -p $Temp -c -d "$($Target)\CritterHeroes.Web"

Write-Host "`nCreating package...`n" -ForegroundColor Yellow

Copy-Item ".\..\CH.DatabaseMigrator\bin\Release\*.*" -Destination "$($Target)\DatabaseMigrator" -Exclude *.vshost*.*,*.xml,ConnectionStrings.config,AppSettings.config

# If running in the console, wait for input before closing.
if ($Host.Name -eq "ConsoleHost")
{ 
    Write-Host "Press any key to continue..."
    $Host.UI.RawUI.FlushInputBuffer()   # Make sure buffered input doesn't "press a key" and skip the ReadKey().
    $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyUp") > $null
}

