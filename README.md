<p align="center">
  <a href="https://ghpreporter.github.io/"><img src="https://github.com/GHPReporter/GHPReporter.github.io/blob/master/img/logo-small.png?raw=true" alt="Project icon"></a>
  <br><br>
  <b>Some Links:</b><br>
  <a href="https://github.com/GHPReporter/Ghpr.Core">Core</a> |
  <a href="https://github.com/GHPReporter/Ghpr.MSTest">MSTest</a> |
  <a href="https://github.com/GHPReporter/Ghpr.NUnit">NUnit</a> |
  <a href="https://github.com/GHPReporter/Ghpr.SpecFlow">SpecFlow</a> |
  <a href="https://github.com/GHPReporter/Ghpr.Console">Console</a> |
  <a href="https://github.com/GHPReporter/GHPReporter.github.io/">Site Repo</a>
</p>

[![Language](http://gh-toprated.info/Badges/LanguageBadge?user=GHPReporter&repo=Ghpr.Core&theme=light&fontWeight=bold)](https://github.com/GHPReporter/Ghpr.Core)
[![Build status](https://ci.appveyor.com/api/projects/status/ix1epmijw6uc780w?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-core)
[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.Core.svg)](https://www.nuget.org/packages/Ghpr.Core)

# Ghpr.Core

Easy-to-use .NET test reporting tool for several testing frameworks

# Usage

|Testing framework|Repository|Nuget version|Examples Repository|CI|
|---|---|---|---|---|
|Core|[Ghpr.Core](https://github.com/GHPReporter/Ghpr.Core)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.Core.svg)](https://www.nuget.org/packages/Ghpr.Core)|-|[![Build status](https://ci.appveyor.com/api/projects/status/ix1epmijw6uc780w?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-core)|
|NUnit 3|[Ghpr.NUnit](https://github.com/GHPReporter/Ghpr.NUnit#usage)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.NUnit.svg)](https://www.nuget.org/packages/Ghpr.NUnit)|[View examples](https://github.com/GHPReporter/Ghpr.NUnit.Examples)|[![Build status](https://ci.appveyor.com/api/projects/status/edl1eag5luk5v4xs?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-nunit)|
|MSTest|[Ghpr.MSTest](https://github.com/GHPReporter/Ghpr.MSTest#usage)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.MSTest.svg)](https://www.nuget.org/packages/Ghpr.MSTest)|[View examples](https://github.com/GHPReporter/Ghpr.MSTest.Examples)|[![Build status](https://ci.appveyor.com/api/projects/status/0surlhjtkckdiw18?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-mstest)|
|SpecFlow|[Ghpr.SpecFlow](https://github.com/GHPReporter/Ghpr.SpecFlow)|[![NuGet Version](https://img.shields.io/nuget/v/Ghpr.SpecFlowPlugin.svg)](https://www.nuget.org/packages/Ghpr.SpecFlowPlugin)|[View examples](https://github.com/GHPReporter/Ghpr.SpecFlow.Examples)|[![Build status](https://ci.appveyor.com/api/projects/status/jtmugpb1axnpc97g?svg=true)](https://ci.appveyor.com/project/elv1s42/ghpr-specflow)|

<p align="center">
  <a href="https://raw.githubusercontent.com/GHPReporter/Ghpr.Core/master/packages.png"><img src="https://raw.githubusercontent.com/GHPReporter/Ghpr.Core/master/packages.png" alt="Diagram"></a>
</p>

# Demo Report

You can view [Demo report](http://ghpreporter.github.io/report/) on our [site](http://ghpreporter.github.io/)

# About Settings file

Standard settings file is .json file with the following structure:
``` json
{
	"outputPath":"C:\\_GHPReporter_Core_Report",
	"sprint":"",
	"runName":"",
	"runGuid":"",
	"realTimeGeneration":"True",
	"runsToDisplay": "5",
	"testsToDisplay": "5"  
}
```
For Ghpr.Core it is called `Ghpr.Core.Settings.json`. This file is included in NuGet package. For different testing frameworks (MSTest, NUnit, SpecFlow) there are separate settings files. Separate files are needed to let Ghpr.Core use different settings for different testing frameworks. 

Parameter `runsToDisplay`: if >0 the reporter will load only this specified number of the latest runs on test run page.

Parameter `testsToDisplay`: if >0 the reporter will load only this specified number of the latest test runs on test history page.

# View report locally

#### Firefox

 - Go to `about:config`
 - Find `security.fileuri.strict_origin_policy` parameter
 - Set it to `false`
 
#### Chrome

 - Close your Chrome
 - Launch it with `--allow-file-access-from-files` option:
    - eg C:\PATH TO\chrome.exe --allow-file-access-from-files

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
