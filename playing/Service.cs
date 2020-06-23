using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace playing
{
    public static class Service
    {
        public static VersionInfo AssemblyVersion { get; private set; } = new VersionInfo();

        public static void Start()
        {
            var parentFrame = new StackFrame(1, true);
            AssemblyVersion.ServiceVersion = parentFrame?.GetMethod()?.ReflectedType?.Assembly?.GetName()?.ToString() ?? "";
            AssemblyVersion.CoreVersion = typeof(Service).Assembly?.GetName()?.ToString() ?? "";
        }

    }
    public class VersionInfo
    {
        public string ServiceVersion { get; set; }
        public string CoreVersion { get; set; }
    }
}
