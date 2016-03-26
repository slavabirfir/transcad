using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class PerformenceCounters
    {
        // Page Level declaration
        protected System.Diagnostics.PerformanceCounter cpuCounter;
        protected System.Diagnostics.PerformanceCounter ramCounter; 

    // Put into page load
    public PerformenceCounters()
    {
        cpuCounter = new System.Diagnostics.PerformanceCounter();
        cpuCounter.CategoryName = "Processor";
        cpuCounter.CounterName = "% Processor Time";
        cpuCounter.InstanceName = "_Total";
        ramCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
    }
    // Call this method every time you need to know the current cpu usage. 
    public string getCurrentCpuUsage()
    {
        return cpuCounter.NextValue()+"%";
    }

    // Call this method every time you need to get the amount of the available RAM in Mb 
    public string getAvailableRAM()
    {
        return ramCounter.NextValue() + "Mb";
    }

    }
}
