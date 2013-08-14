﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using openVisN;
using openVisN.Library;

namespace openVisN
{
    public class SettingsManagement
    {
        public DataSet MyData;
        string fileName;
        public SettingsManagement(string fileName)
        {
            this.fileName = fileName;
            if (File.Exists(fileName))
            {
                Load();
            }
            else
            {
                MakeNew();
            }
        }

        public void Load()
        {
            MyData = new DataSet("openVisN 0.9");
            MyData.ReadXml(fileName, XmlReadMode.ReadSchema);
            ApplySettings();
        }

        void ApplySettings()
        {
            var signals = AllSignals.DefaultSignals;
            signals.Clear();
            
            foreach (DataRow row in MyData.Tables["Measurements"].Rows)
                signals.Add(new SignalBook(row));

            var terminal = AllSignalGroups.DefaultSignalGroups;

            foreach (DataRow row in MyData.Tables["Terminals"].Rows)
                terminal.Add(new SignalGroupBook(row));
        }

        void MakeNew()
        {
            MyData = new DataSet("openVisN 0.9");
            MyData.Tables.Add("Measurements");
            MyData.Tables.Add("Terminals");
            MyData.Tables.Add("Settings");

            DataTable table = MyData.Tables["Measurements"];
            table.Columns.Add("PointID", typeof(long));
            table.Columns.Add("SignalID", typeof(Guid));
            table.Columns.Add("DeviceName", typeof(string));
            table.Columns.Add("SignalAcronym", typeof (string));
            table.Columns.Add("Description", typeof(string));

            table = MyData.Tables["Terminals"];
            table.Columns.Add("GroupName", typeof(string));
            table.Columns.Add("NominalVoltage", typeof(double));
            table.Columns.Add("CurrentMagnitude", typeof(long));
            table.Columns.Add("CurrentAngle", typeof(long));
            table.Columns.Add("VoltageMagnitude", typeof(long));
            table.Columns.Add("VoltageAngle", typeof(long));
            table.Columns.Add("DFDT", typeof(long));
            table.Columns.Add("Frequency", typeof(long));
            table.Columns.Add("Status", typeof(long));

            table = MyData.Tables["Settings"];
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Value", typeof(string));

            Save();
        }

        public void Edit()
        {
            FrmConfigure win = new FrmConfigure(this);
            win.ShowDialog();
            Load();
        }

        public string ServerIP
        {
           get
           {
               foreach (DataRow row in MyData.Tables["Settings"].Select("Name='ServerIP'"))
               {
                   return (string)row["Value"];
               }
               return "127.0.0.1";
           }
            set
            {
                foreach (DataRow row in MyData.Tables["Settings"].Select("Name='ServerIP'"))
                {
                    row["Value"] = value;
                    return;
                }
                MyData.Tables["Settings"].Rows.Add("ServerIP", value);
            }
        }
        public string HistorianPort
        {
            get
            {
                foreach (DataRow row in MyData.Tables["Settings"].Select("Name='HistorianPort'"))
                {
                    return (string)row["Value"];
                }
                return "38402";
            }
            set
            {
                foreach (DataRow row in MyData.Tables["Settings"].Select("Name='HistorianPort'"))
                {
                    row["Value"] = value;
                    return;
                }
                MyData.Tables["Settings"].Rows.Add("HistorianPort", value);
            }
        }

        public string HistorianDatabase
        {
            get
            {
                foreach (DataRow row in MyData.Tables["Settings"].Select("Name='HistorianDatabase'"))
                {
                    return (string)row["Value"];
                }
                return "PPA";
            }
            set
            {
                foreach (DataRow row in MyData.Tables["Settings"].Select("Name='HistorianDatabase'"))
                {
                    row["Value"] = value;
                    return;
                }
                MyData.Tables["Settings"].Rows.Add("HistorianDatabase", value);
            }
        }

        public string GEPPort
        {
            get
            {
                foreach (DataRow row in MyData.Tables["Settings"].Select("Name='GEPPort'"))
                {
                    return (string)row["Value"];
                }
                return "6175";
            }
            set
            {
                foreach (DataRow row in MyData.Tables["Settings"].Select("Name='GEPPort'"))
                {
                    row["Value"] = value;
                    return;
                }
                MyData.Tables["Settings"].Rows.Add("GEPPort", value);
            }
        }

        public void Save()
        {
            MyData.WriteXml(fileName, XmlWriteMode.WriteSchema);
        }
    }
}
