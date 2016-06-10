using System;
using System.Collections.Generic;
using Ghpr.Core.Interfaces;

namespace Ghpr.Core.Common
{
    public class Run : IRun
    {
        public List<ITestRun> TestRuns { get; set; }
        public Guid Guid { get; }

        public Run(Guid runGuid)
        {
            Guid = runGuid;
        }

        

    }
}