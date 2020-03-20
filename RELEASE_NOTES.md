# Release notes

 ## v0.9.11

Includes some minor bug fixes and improvements for `Ghpr.NUnit`, `Ghpr.MSTestV2` and `Ghpr.Console`:

 - Screenshots are now saved when generating the report from `.trx` file: [Bug](https://github.com/GHPReporter/Ghpr.MSTestV2/issues/19) 
 - The `Ghpr.NUnit` is now working with .NET Core: [Bug](https://github.com/GHPReporter/Ghpr.NUnit/issues/77)
 - `Ghpr.Console` released with both changes

 ## v0.9.10

Includes some minor bug fixes and improvements:

 - Fixed screenshots bug in the `Ghpr.Nunit`: [Bug](https://github.com/GHPReporter/Ghpr.NUnit/issues/67) 
 - Added support for .NET Framework v4.8 for `Ghpr.Core` and `Ghpr.NUnit`
 - Fixed test status bug in `Ghpr.Core`: [Bug](https://github.com/GHPReporter/Ghpr.Core/issues/138)
 - Fixed `Ghpr.NUnit` DateTime bug: [Bug](https://github.com/GHPReporter/Ghpr.NUnit/issues/68)
 - Fixed `Ghpr.MSTestV2` bug report generation: [Bug](https://github.com/GHPReporter/Ghpr.MSTestV2/issues/20)
 - Fixed cleanup job for `Ghpr.Console`: [Bug](https://github.com/GHPReporter/Ghpr.NUnit/issues/72)

 ## v0.9.9.9

Includes some new features and bug fixes:

 - Escaping test output is optional now: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/128) 
 - Added support for .NET Framework v4.6.2 (and other Framework versions): [Feature](https://github.com/GHPReporter/Ghpr.NUnit/issues/31)
 - .NET Core 2.0 support: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/54)
 - .NET Core 3.0 is supported too: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/131)
 - Fixed bug with screenshot sizing: [Bug](https://github.com/GHPReporter/Ghpr.Core/issues/134)
 - Added new `favicon` for the report
 
## v0.9.9

Includes new way of working with `.settings.json` files (**BRAKING CHANGE!**):

 - New settings file structure: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/107). For more details, please read [`readme` file](https://github.com/GHPReporter/Ghpr.Core#about-settings-file)
 - Fixed small issue with collapsed test list view
 - Fixed small issue with run summary data
 
 ## v0.9.4

Includes new UI for the report and some fixes:

 - New UI created: [PR](https://github.com/GHPReporter/Ghpr.Core/pull/82)
 - Unit tests, code coverage and code quality checks are added: [PR1](https://github.com/GHPReporter/Ghpr.Core/pull/97), [PR2](https://github.com/GHPReporter/Ghpr.Core/pull/96), [PR3](https://github.com/GHPReporter/Ghpr.Core/pull/95)
 - MSTest V2: DataTestMethod is now supported: [Feature](https://github.com/GHPReporter/Ghpr.MSTestV2/issues/1)
 - Possibility to display next/prev test in current run added: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/52)
 - Test categories improved: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/50)
 - Azure Pipelines are set up: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/86)
 - NUnit.Engine 3.9.0 is now supported: [Feature](https://github.com/GHPReporter/Ghpr.NUnit/issues/38)
 
## v0.9.3

Includes several fixes/improvements:

 - SpecFlow 2.4 is now supported: [Issue](https://github.com/GHPReporter/Ghpr.SpecFlow/issues/24)
 - Entire report name now can be set up from `.json` settings: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/76)
 - Octicons updated: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/77)
 - Plotly library updated: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/75)
 - Timeline chart (alpha version) added for tests run: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/62)
 - Mapping bug fixed for test description: [Bug](https://github.com/GHPReporter/Ghpr.Core/pull/79). Special thanks to [@nunomdc](https://github.com/nunomdc) 
 - Stack trace is now highlighted: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/78)
 - Added support for MSTestV2 DataTestMethod tests: [Feature](https://github.com/GHPReporter/Ghpr.MSTestV2/issues/1)
 - Test chart is now clickable. Each point on the test history chart leads to corresponding test: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/74)
 - Runs chart is now clickable. Each bar on the chart leads to corresponding run page: [Feature](https://github.com/GHPReporter/Ghpr.Core/issues/73)

## v0.9.2.1

This includes one new feature for MSTest package: [Related pull request](https://github.com/GHPReporter/Ghpr.MSTest/pull/9)

Now `Description` is included in the report correctly. Special thanks to [@moreira-joao](https://github.com/moreira-joao)

## v0.9.2

This includes one new feature + some small changes.

 - Added clean up job: [Related issue](https://github.com/GHPReporter/Ghpr.Core/issues/57). Now old runs can be deleted using `retention settings`: 
 ``` json
  "retention": {
    "amount": 10,
    "till": "2018-06-29 10:00:00"
  }
 ```
**WARNING:** Clean up job will run each time the new report is generated. Only `amount` total runs will be left, all other will be deleted. Also all runs with finish date older than `till` will be deleted too. There is no way to restore the data, so please use these settings carefully.
 
## v0.9.1

This includes some small fixes/improvements comparing to v0.9.

 - Fixed bug with NUnit screenshots when the report is generated from `.xml` file with NUnit test results.
 - Added Test Duration to Test Run Dto to be able to set it in different ways for different testing frameworks.
 
## v0.9

This version includes some breaking changes and new features. Please use v0.9.0.5 as it contains some bug fixes.

 - Test screenshots are now stored in a separate `.json` files. This speeds up loading of test list in the report.
 - Test output is now stored in a separate `.json` file: [Related issue](https://github.com/GHPReporter/Ghpr.Core/issues/40). This speeds up loading of test list in the report.
 - Added support for NUnit screenshots: [Related issue](https://github.com/GHPReporter/Ghpr.NUnit/issues/37). 
By default all attachments with extentions `.png`, `.jpeg` and `.bmp` will be considered as test screenshots and displayed in the report. 
 - Added support for output from NUnit test suites: [Related issue](https://github.com/GHPReporter/Ghpr.NUnit/issues/36).
Now extra test output will be displayed in a separate Tab on Test Run page.
 - Data service was moved to an extra NuGet package. So now there will be an extra dependency to a [new NuGet package](https://www.nuget.org/packages/Ghpr.LocalFileSystem/).
The idea is that in the future several ways of working with data will be supported (some work with Couch DB is already in progress).
 - Some extra packages ([1](https://www.nuget.org/packages/Ghpr.SimpleFileLogger/), [2](https://www.nuget.org/packages/Ghpr.SerilogToSeqLogger/)) were created to use internal Ghpr logging to diagnose some issues with the tool itself.
 - Small changes in the title font size on the report pages.

## v0.8 and earlier

Previous releases included such features as:

 - Test history
 - Runs history
 - Hierarchical test list
 - Test scresnshots
 - Real-time test report generation
 - All other features
