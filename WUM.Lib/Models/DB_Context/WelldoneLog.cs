using System;
using System.Collections.Generic;

namespace WUM.Lib.Models.DB_Context;

public partial class WelldoneLog
{
    public long Id { get; set; }

    public DateTimeOffset? LogDate { get; set; }

    public string? LogLevel { get; set; }

    public string? LogMsg { get; set; }

    public string? UserId { get; set; }

    public string? LogSource { get; set; }

    public string? Exception { get; set; }
}
