& .\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -target:"nunit3-console.exe" "-targetargs:""Ghpr.Core.Tests\bin\Release\Ghpr.Core.Tests.dll""" -filter:"+[Ghpr.Core*]* -[Ghpr.Core.Tests*]*" -output:opencoverCoverage.xml

$coveralls = (Resolve-Path "packages/coveralls.net.*/tools/csmacnz.coveralls.exe").ToString()

write-host "======= COVERALLS PATH: " $coveralls " ======="

if ($env:APPVEYOR_PULL_REQUEST_NUMBER -eq $null) {
	& $coveralls --opencover -i opencoverCoverage.xml --repoToken $env:COVERALLS_REPO_TOKEN --commitId $env:APPVEYOR_REPO_COMMIT --commitBranch $env:APPVEYOR_REPO_BRANCH --commitAuthor $env:APPVEYOR_REPO_COMMIT_AUTHOR --commitEmail $env:APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL --commitMessage $env:APPVEYOR_REPO_COMMIT_MESSAGE --jobId $env:APPVEYOR_JOB_ID
}
else {
	write-host "======= PULL REQUEST: " $env:APPVEYOR_PULL_REQUEST_NUMBER " ======="
	& $coveralls --opencover -i opencoverCoverage.xml --repoToken $env:COVERALLS_REPO_TOKEN
}

$result = $LASTEXITCODE

if($result -ne 0){
  exit $result
}