COPY "%~dp0\AppSettings.config" "%~dp0\..\CritterHeroes.Web\AppSettings.config"
COPY "%~dp0\AppSettings.config" "%~dp0\..\CH.Test\AppSettings.config"
COPY "%~dp0\AppSettings.Migrations.config" "%~dp0\..\CH.DatabaseMigrator\AppSettings.config"

COPY "%~dp0\RescueGroups.config" "%~dp0\..\CritterHeroes.Web\RescueGroups.config"
COPY "%~dp0\RescueGroups.config" "%~dp0\..\CH.RescueGroupsExplorer\RescueGroups.config"
COPY "%~dp0\RescueGroups.config" "%~dp0\..\CH.Test\RescueGroups.config"
COPY "%~dp0\RescueGroups.config" "%~dp0\..\CH.RescueGroupsImporter\RescueGroups.config"
