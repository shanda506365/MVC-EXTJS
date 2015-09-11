﻿using log4net.Appender;
using log4net.Util;

namespace USO.Core.Logging
{
    public class USOFileAppender : RollingFileAppender
    {
        public enum RollingStyleFrequencyMode
        {
            Once,
            Continuous
        }

        public RollingStyleFrequencyMode RollingStyleFrequency { get; set; }

        public bool RollSize
        {
            get { return (RollingStyle == RollingMode.Composite || RollingStyle == RollingMode.Size); }
        }

        protected override void AdjustFileBeforeAppend()
        {
            if (RollingStyle == RollingMode.Size ||
                RollingStyleFrequency == RollingStyleFrequencyMode.Continuous)
            {
                base.AdjustFileBeforeAppend();
            }
            else if (RollSize && ((File != null) && (((CountingQuietTextWriter)QuietWriter).Count >= MaxFileSize)))
            {
                RollOverSize();
            }
        }
    }
}
