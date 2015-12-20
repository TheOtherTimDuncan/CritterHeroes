"%~dp0\packages\ReportGenerator.2.3.5.0\tools\ReportGenerator.exe" ^
-reports:"%~dp0\TestResults\OpenCover.xml" ^
-targetdir:"%~dp0\TestResults\OpenCover"

start TestResults\OpenCover\index.htm
