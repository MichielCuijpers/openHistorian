//******************************************************************************************************
//  FirstStageWriterSettings.cs - Gbtc
//
//  Copyright � 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  09/18/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using System;

namespace GSF.SortedTreeStore.Services.Writer
{
    /// <summary>
    /// The settings for the <see cref="FirstStageWriter{TKey,TValue}"/>
    /// </summary>
    public class FirstStageWriterSettings
    {
        private int m_rolloverInterval = 10000;
        private int m_rolloverSizeMb = 200;
        private int m_maximumAllowedMb = 300;
        private IncrementalStagingFileSettings m_stagingFileSettings = new IncrementalStagingFileSettings();

        /// <summary>
        /// The number of milliseconds before data is flushed to the disk. 
        /// </summary>
        /// <remarks>
        /// Must be between 1,000 ms and 60,000 ms.
        /// </remarks>
        public int RolloverInterval
        {
            get
            {
                return m_rolloverInterval;
            }
            set
            {
                if (value < 1000)
                {
                    m_rolloverInterval = 1000;
                }
                else if (value > 60000)
                {
                    m_rolloverInterval = 60000;
                }
                else
                {
                    m_rolloverInterval = value;
                }
            }
        }

        /// <summary>
        /// The size at which a rollover will be signaled
        /// </summary>
        /// <remarks>
        /// Must be at least 1MB. Upper Limit should be Memory Constrained, but not larger than 1024MB.
        /// </remarks>
        public int RolloverSizeMb
        {
            get
            {
                return m_rolloverSizeMb;
            }
            set
            {
                if (value < 1)
                {
                    m_rolloverSizeMb = 1;
                }
                else if (value > 1024)
                {
                    m_rolloverSizeMb = 1024;
                }
                else
                {
                    m_rolloverSizeMb = value;
                }
            }
        }

        /// <summary>
        /// The size after which the incoming write queue will pause
        /// to wait for rollovers to complete.
        /// </summary>
        /// <remarks>
        /// It is recommended to make this value larger than <see cref="RolloverSizeMb"/>.
        /// If this value is smaller than <see cref="RolloverSizeMb"/> then <see cref="RolloverSizeMb"/> will be used.
        /// Must be at least 1MB. Upper Limit should be Memory Constrained, but not larger than 1024MB.
        /// </remarks>
        public int MaximumAllowedMb
        {
            get
            {
                return m_maximumAllowedMb;
            }
            set
            {
                if (value < 1)
                {
                    m_maximumAllowedMb = 1;
                }
                else if (value > 1024)
                {
                    m_maximumAllowedMb = 1024;
                }
                else
                {
                    m_maximumAllowedMb = value;
                }
            }
        }

        /// <summary>
        /// The settings that will be used for the incremental file.
        /// </summary>
        public IncrementalStagingFileSettings StagingFileSettings
        {
            get
            {
                return m_stagingFileSettings;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                m_stagingFileSettings = value;
            }
        }

        /// <summary>
        /// Clones the <see cref="FirstStageWriterSettings"/>
        /// </summary>
        /// <returns></returns>
        public FirstStageWriterSettings Clone()
        {
            var other = (FirstStageWriterSettings)MemberwiseClone();
            other.m_stagingFileSettings = m_stagingFileSettings.Clone();
            return other;
        }
    }
}