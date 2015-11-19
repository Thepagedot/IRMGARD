using System;
using Android.Util;

namespace IRMGARD
{
    public static class TimeProfiler
    {
        const string TAG = "TimeProfiler";

        public static long Start()
        {
            return Java.Lang.JavaSystem.CurrentTimeMillis();
        }

        public static void StopAndLog(string tag, string label, long started)
        {
            if (Env.Debug)
            {
                var stopped = Java.Lang.JavaSystem.CurrentTimeMillis() - started;
                Log.Debug(tag, "Duration({0}): {1} ms", label, stopped);
            }
        }

        public static void LogMemInfo()
        {
            if (Env.Debug)
            {
                var r = Java.Lang.Runtime.GetRuntime();
                Log.Debug(TAG, "Max: {0}, Total: {1}, Free: {2}.", r.MaxMemory(), r.TotalMemory(), r.FreeMemory());
            }
        }
    }
}

