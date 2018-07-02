# Release notes

## v0.9

This version includes some breaking changes and new features. Please use v0.9.0.3 as it contains some bug fixes

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
