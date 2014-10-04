﻿//******************************************************************************************************
//  ArchiveListLogSettings.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/04/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//
//******************************************************************************************************

using System;
using System.IO;
using GSF.IO;
using GSF.SortedTreeStore.Services.Writer;

namespace GSF.SortedTreeStore.Services.Writer
{
    /// <summary>
    /// The settings for a <see cref="RolloverLogFile"/>.
    /// </summary>
    public class RolloverLogSettings
    {
        private string m_logPath = string.Empty;
        private string m_logFilePrefix = "Rollover";
        private string m_logFileExtension = ".RolloverLog";

        /// <summary>
        /// Gets if this archive log will be file backed. 
        /// This is true as long as <see cref="LogPath"/> is assigned
        /// a value.
        /// </summary>
        public bool IsFileBacked
        {
            get
            {
                return m_logPath != string.Empty;
            }
        }

        /// <summary>
        /// The path to store all log files. Can be an empty string to 
        /// not enable file based logging.
        /// </summary>
        public string LogPath
        {
            get
            {
                return m_logPath;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    m_logPath = string.Empty;
                    return;
                }
                PathHelpers.ValidatePathName(value);
                m_logPath = value;
            }
        }

        /// <summary>
        /// The prefix to assign to all log files. Can be string.empty
        /// </summary>
        public string LogFilePrefix
        {
            get
            {
                return m_logFilePrefix;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    m_logFilePrefix = string.Empty;
                    return;
                }
                PathHelpers.ValidatePathName(value);
                m_logFilePrefix = value;
            }
        }

        /// <summary>
        /// The file extension to write the log files.
        /// </summary>
        public string LogFileExtension
        {
            get
            {
                return m_logFileExtension;
            }
            set
            {
                m_logFileExtension = PathHelpers.FormatExtension(value);
            }
        }


        /// <summary>
        /// Gets the wildcard search string for a log file.
        /// </summary>
        internal string SearchPattern
        {
            get
            {
                if (LogFilePrefix == string.Empty)
                {
                    return "*" + LogFileExtension;
                }
                return LogFilePrefix + " *" + LogFileExtension;
            }
        }

        /// <summary>
        /// Generates a new file name.
        /// </summary>
        /// <returns></returns>
        internal string GenerateNewFileName()
        {
            if (!IsFileBacked)
                throw new Exception("Cannot generate a file name when the log is not a file backed log");
            if (LogFilePrefix == string.Empty)
            {
                return Path.Combine(LogPath, Guid.NewGuid().ToString() + LogFileExtension);
            }
            return Path.Combine(LogPath, LogFilePrefix + " " + Guid.NewGuid().ToString() + LogFileExtension);
        }

        /// <summary>
        /// Creates a clone of this class.
        /// </summary>
        /// <returns></returns>
        public RolloverLogSettings Clone()
        {
            var obj = (RolloverLogSettings)MemberwiseClone();
            return obj;
        }
    }
}