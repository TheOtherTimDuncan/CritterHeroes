"%~dp0\packages\OpenCover.4.6.166\tools\OpenCover.Console.exe" ^
-target:"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" ^
-targetargs:"%~dp0\CH.Test\bin\Debug\CH.Test.dll"  ^
-output:"%~dp0\TestResults\OpenCover.xml" ^
-filter:"+[CritterHeroes*]*" ^
-hideskipped:All ^
-register:user ^
-skipautoprops

ReportGenerator.bat
