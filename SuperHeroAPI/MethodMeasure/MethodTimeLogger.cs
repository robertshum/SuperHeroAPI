using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SuperHeroAPI.MethodMeasure
{
    public static class MethodTimeLogger
    {
        public static ILogger? Logger;
        public static void Log(MethodBase methodBase, TimeSpan timeSpan, string message)
        {
            Logger?.LogInformation("***{Class}.{Method}: Finish - {message} in {Duration}ms.",
                methodBase.DeclaringType!.Name, methodBase.Name, message, timeSpan.Milliseconds);
        }
    }
}
