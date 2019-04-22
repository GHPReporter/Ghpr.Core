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

$codecov = (Resolve-Path "packages/Codecov.*/tools/codecov.exe").ToString()

write-host "======= CODECOV PATH: " $codecov " ======="
& $codecov -f "opencoverCoverage.xml"

if($result -ne 0){
  exit $result
}