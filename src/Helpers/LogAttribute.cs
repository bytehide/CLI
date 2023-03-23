using System;
using System.Diagnostics;
using System.Linq;

namespace Bytehide.CLI.Helpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogAttribute : Attribute
    {
        public bool Silent => DoNotDisturb;
        public bool DoNotDisturb { get; }

        public LogAttribute(bool DoNotDisturb = false)
        {
            this.DoNotDisturb = DoNotDisturb;
        }

        public static bool GetValueOfFirstInStackTrace()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame[] stackFrames = stackTrace.GetFrames();

            foreach (var stackFrame in stackFrames)
            {
                var method = stackFrame.GetMethod();
                LogAttribute attr = (LogAttribute)method.GetCustomAttributes(typeof(LogAttribute), true).FirstOrDefault();
                if (attr != null)
                {
                    bool value = attr.DoNotDisturb;
                    return value;
                }
            }

            return false;
        }
    }
}
