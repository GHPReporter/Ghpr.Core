# Release notes

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

Previous releases included such feature as:

 - Test history
 - Runs history
 - Hierarchical test list
 - Test scresnshots
 - Real-time test report generation
 - All other features
