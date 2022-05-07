namespace wrenstone.statics
{
    /// <summary>
    /// a static class for log file names
    /// </summary>
    public static class Logs
    {
        static string Extension =>
            ".log";

        public static string CallingAssembly =>
            $"{Strings.CallingAssemblyName}{Extension}";

        public static string ExecutingAssembly =>
            $"{Strings.ExecutingAssemblyName}{Extension}";
    }
}
