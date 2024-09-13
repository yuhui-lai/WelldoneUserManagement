namespace WUM.Lib.Utilities
{
    public static class TimeUtil
    {
        public static DateTimeOffset UtcNow()
        {
            return DateTimeOffset.UtcNow;
        }

        public static DateTimeOffset ToUtc(this DateTimeOffset local)
        {
            return local.ToUniversalTime();
        }
    }
}
