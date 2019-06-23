$opencover = "$($env:USERPROFILE)\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe"

write-host "======= OPENCOVER PATH: " $opencover " ======="

$nunit = "$($env:USERPROFILE)\.nuget\packages\nunit.consolerunner\3.10.0\tools\nunit3-console.exe"

write-host "======= NUNIT PATH: " $nunit " ======="

$build = "$($env:APPVEYOR_BUILD_FOLDER)"

write-host "======= BUILD PATH: " $build " ======="

$sourceRoot = "$($env:USERPROFILE)\.nuget\packages\nunit.consolerunner\3.10.0\tools"
$destinationRoot = $build + "\Ghpr.Tests.Tests\bin\Release\netcoreapp2.1\"

Copy-Item -Path $sourceRoot -Filter "*.*" -Recurse -Destination $destinationRoot -Container

& $opencover -register:user -target:"nunit3-console.exe" "-targetargs:""Ghpr.Tests.Tests\bin\Release\netcoreapp2.1\Ghpr.Core.Tests.dll""" -filter:"+[Ghpr.Core*]* -[Ghpr.Tests.Tests*]*" -output:opencoverCoverage.xml