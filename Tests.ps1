$opencover = "$($env:USERPROFILE)\.nuget\packages\opencover\4.7.922\tools\OpenCover.Console.exe"

write-host "======= OPENCOVER PATH: " $opencover " ======="

& $opencover -register:user -target:"nunit3-console.exe" "-targetargs:""Ghpr.Tests.Tests\bin\Release\netcoreapp2.1\Ghpr.Core.Tests.dll""" -filter:"+[Ghpr.Core*]* -[Ghpr.Tests.Tests*]*" -output:opencoverCoverage.xml