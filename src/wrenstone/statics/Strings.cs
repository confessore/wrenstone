using System.Reflection;

namespace wrenstone.statics
{
    /// <summary>
    /// a static class for commonly used strings
    /// </summary>
    public static class Strings
    {
        /// <summary>
        /// the name of the assembly that calls this property
        /// </summary>
        public static string? CallingAssemblyName =>
            Assembly.GetCallingAssembly().GetName().Name;

        /// <summary>
        /// the name of the assembly that executes this property
        /// </summary>
        public static string? ExecutingAssemblyName =>
            Assembly.GetExecutingAssembly().GetName().Name;
    }
}
