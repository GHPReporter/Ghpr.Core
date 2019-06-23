$opencover = "$($env:USERPROFILE)\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe"

write-host "======= OPENCOVER PATH: " $opencover " ======="

$nunit = "$($env:USERPROFILE)\.nuget\packages\nunit.consolerunner\3.10.0\tools\nunit3-console.exe"

write-host "======= NUNIT PATH: " $nunit " ======="

& $opencover -register:user -target:$nunit "-targetargs:""Ghpr.Tests.Tests\bin\Release\netcoreapp2.1\Ghpr.Core.Tests.dll""" -filter:"+[Ghpr.Core*]* -[Ghpr.Tests.Tests*]*" -output:opencoverCoverage.xml