﻿using System;

namespace Geexbox.Logging.RollingFile.Internal
{
    public struct LogMessage
    {
        public DateTimeOffset Timestamp { get; set; }
        public string Message { get; set; }
    }
}
