<p align="center">
  <a href="https://ghpreporter.github.io/"><img src="https://github.com/GHPReporter/GHPReporter.github.io/blob/master/img/logo-small.png?raw=true" alt="Project icon"></a>
  <br><br>
  <b>Some Links:</b><br>
  <a href="https://github.com/GHPReporter/Ghpr.Core">Core</a> |
  <a href="https://github.com/GHPReporter/Ghpr.MSTest">MSTest</a> |
  <a href="https://github.com/GHPReporter/Ghpr.MSTestV2">MSTestV2</a> |
  <a href="https://github.com/GHPReporter/Ghpr.NUnit">NUnit</a> |
  <a href="https://github.com/GHPReporter/Ghpr.SpecFlow">SpecFlow</a> |
  <a href="https://github.com/GHPReporter/Ghpr.Console">Console</a> |
  <a href="https://github.com/GHPReporter/GHPReporter.github.io/">Site Repo</a>
</p>

[![Build status](https://ci.appveyor.com/api/projects/status/ix1epmijw6uc780w/branch/master?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-core/branch/master)
[![Build status](https://dev.azure.com/ghpreporter/Ghpr.Core/_apis/build/status/Ghpr.Core-CI)](https://dev.azure.com/ghpreporter/Ghpr.Core/_build/latest?definitionId=2)
[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.Core.svg)](https://www.nuget.org/packages/Ghpr.Core)
[![Coverage Status](https://coveralls.io/repos/github/GHPReporter/Ghpr.Core/badge.svg?branch=master)](https://coveralls.io/github/GHPReporter/Ghpr.Core?branch=master)
[![codecov](https://codecov.io/gh/GHPReporter/Ghpr.Core/branch/master/graph/badge.svg)](https://codecov.io/gh/GHPReporter/Ghpr.Core)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/0a299da7eea3464a8652ee5d2fea28f5)](https://www.codacy.com/app/GHPReporter/Ghpr.Core?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=GHPReporter/Ghpr.Core&amp;utm_campaign=Badge_Grade)
[![CodeFactor](https://www.codefactor.io/repository/github/ghpreporter/ghpr.core/badge)](https://www.codefactor.io/repository/github/ghpreporter/ghpr.core)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FGHPReporter%2FGhpr.Core.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FGHPReporter%2FGhpr.Core?ref=badge_shield)

# Ghpr.Core

Easy-to-use .NET test reporting tool for several testing frameworks

# Usage

|Testing framework|Repository|Nuget version|Examples Repository|CI|
|---|---|---|---|---|
|Core|[Ghpr.Core](https://github.com/GHPReporter/Ghpr.Core)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.Core.svg)](https://www.nuget.org/packages/Ghpr.Core)|-|[![Build status](https://ci.appveyor.com/api/projects/status/ix1epmijw6uc780w?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-core)|
|NUnit 3|[Ghpr.NUnit](https://github.com/GHPReporter/Ghpr.NUnit#usage)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.NUnit.svg)](https://www.nuget.org/packages/Ghpr.NUnit)|[View examples](https://github.com/GHPReporter/Ghpr.NUnit.Examples)|[![Build status](https://ci.appveyor.com/api/projects/status/edl1eag5luk5v4xs?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-nunit)|
|MSTest|[Ghpr.MSTest](https://github.com/GHPReporter/Ghpr.MSTest#usage)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.MSTest.svg)](https://www.nuget.org/packages/Ghpr.MSTest)|[View examples](https://github.com/GHPReporter/Ghpr.MSTest.Examples)|[![Build status](https://ci.appveyor.com/api/projects/status/0surlhjtkckdiw18?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-mstest)|
|SpecFlow|[Ghpr.SpecFlow](https://github.com/GHPReporter/Ghpr.SpecFlow)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.SpecFlowPlugin.svg)](https://www.nuget.org/packages/Ghpr.SpecFlowPlugin)|[View examples](https://github.com/GHPReporter/Ghpr.SpecFlow.Examples)|[![Build status](https://ci.appveyor.com/api/projects/status/jtmugpb1axnpc97g?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-specflow)|
|Console App|[Ghpr.Console](https://github.com/GHPReporter/Ghpr.Console)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.Console.svg)](https://www.nuget.org/packages/Ghpr.Console)|-|[![Build status](https://ci.appveyor.com/api/projects/status/1nhj8penho50h2ro?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-console)|

# Demo Report

You can view [Demo report](http://ghpreporter.github.io/report/) on our [site](http://ghpreporter.github.io/)

# Release notes

You can find it [here](https://github.com/GHPReporter/Ghpr.Core/blob/master/RELEASE_NOTES.md) for all packages.

# About Settings file

Standard settings file is .json file with the following structure:
``` json
{
  "default": {
    "outputPath": "C:\\_GHPReporter_Core_Report",
    "dataServiceFile": "Ghpr.LocalFileSystem.dll",
    "loggerFile": "",
    "sprint": "",
    "reportName": "GHP Report",
    "projectName": "Awesome project",
    "runName": "",
    "runGuid": "",
    "realTimeGeneration": "True",
    "runsToDisplay": "5",
    "testsToDisplay": "5",
    "retention": {
      "amount": 1000,
      "till": "2017-01-25 10:00:00"
    }
  },
  "projects": [
    {
      "pattern": "*CoolTests.dll",
      "settings": {
        "outputPath": "C:\\_GHPReporter_Core_Report\\CoolTests"
      }
    },
    {
      "pattern": "*AwesomeTests.dll",
      "settings": {
        "outputPath": "C:\\_GHPReporter_Core_Report\\AwesomeTests"
      }
    }
  ]
}
```
For Ghpr.Core it is called `Ghpr.Core.Settings.json`. This file is included in NuGet package. For different testing frameworks (MSTest, NUnit, SpecFlow) there are separate settings files. Separate files are needed to let Ghpr.Core use different settings for different testing frameworks. 

The `default` section stands for defalut configuration, which includes the following information:

 - `runsToDisplay`: if > 0 the reporter will load only this specified number of the latest runs on test run page.

 - `testsToDisplay`: if > 0 the reporter will load only this specified number of the latest test runs on test history page.

 - `dataServiceFile`: the name of the library which contains implementation of IDataService, will be distributed as a separate NuGet package will you should include as a dependency in your solution with tests. Can't be empty.

 - `loggerFile`: the name the library that will be used for internal logging of Ghpr itself.

 - `retention` - settings for running clean up job:

   - `amount` - total runs that will be left, all other will be deleted.
   
   - `till` - all runs with finish date older than  this value will be deleted. Date format is `yyyy-MM-dd hh:mm:ss`
   
`projects` section is the array of pairs `pattern` and `settings`.
 
 - `pattern` - is a wildcard for you project name.
 
 - `settings` - has the same structure as `default` sections. If your project name matches the pattern then `settings` section will be applied (instead of `default` section) as GHPReporter settings for your test run.

# View report locally

#### Firefox

 - Go to `about:config`
 - Find `security.fileuri.strict_origin_policy` parameter
 - Set it to `false`
 - Please make sure to restart the browser
 
#### Chrome

 - Close your all instances of your Chrome
 - Launch the new instance with `--allow-file-access-from-files` option:
    - eg C:\PATH_TO\chrome.exe --allow-file-access-from-files

# How to publish the report in Jenkins

 - In the configuration of your job, in the "Post-build actions", you just have to add a "Publish HTML reports" with the correct informations.
 
Known Issues : 
 - Due to the CSP (Content Security Policy), the report used for the functionals tests is not viewable on Jenkins with the default value defined for the CSP. So, for solving this issue, the CSP is automatically forced after each restart with a specific value. For that, a line is added in the C:\Program Files (x86)\Jenkins\jenkins.xml file, like this :
... 
 <arguments>-Xrs -Xmx256m -Dhudson.lifecycle=hudson.lifecycle.WindowsServiceLifecycle "-Dhudson.model.DirectoryBrowserSupport.CSP=" -jar -Dmail.smtp.starttls.enable=true "%BASE%\jenkins.war" --httpPort=8080 --webroot="%BASE%\war"</arguments>
...

 - The screenshots generated with Selenium work only when there were made with browsers like Firefox or Chrome (Doesn't work with IE) 
 

# Contributing

Anyone contributing is welcome. Write [issues](https://github.com/GHPReporter/Ghpr.Core/issues), create [pull requests](https://github.com/GHPReporter/Ghpr.Core/pulls).

# License

[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FGHPReporter%2FGhpr.Core.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FGHPReporter%2FGhpr.Core?ref=badge_large)
