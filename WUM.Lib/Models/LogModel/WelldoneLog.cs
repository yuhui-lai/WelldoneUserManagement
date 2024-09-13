namespace WUM.Lib.Models.LogModel
{
    public class WelldoneLog
    {
        public DateTimeOffset LogDate { get; set; }
        public string LogLevel { get; set; } = "info";
        public string LogMsg { get; set; }
        public string UserId { get; set; }
        public string LogSource { get; set; }
        public string Exception { get; set; }
    }
}
