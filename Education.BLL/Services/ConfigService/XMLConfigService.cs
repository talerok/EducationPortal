using Education.BLL.Services.ConfigService.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Education.BLL.Services
{
    public class Data
    {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string IconPath { get; set; }
    }

    public class XMLConfigService : IConfigService
    {
        private string Path;
        private Data Config;

        private Mutex Mutex = new Mutex();

        public string ConnectionString
        {
            get
            {
                return Config.ConnectionString;
            }
            set
            {
                Mutex.WaitOne();
                try
                {
                    Config.ConnectionString = value;
                    Save();
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
                
            }
        }

        public string Name
        {
            get
            {
                return Config.Name;
            }
            set
            {
                Mutex.WaitOne();
                try
                {
                    Config.Name = value;
                    Save();
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }
        }

        public string IconPath
        {
            get
            {
                return Config.IconPath;
            }
            set
            {
                Mutex.WaitOne();
                try
                {
                    Config.IconPath = value;
                    Save();
                }
                finally
                {
                    Mutex.ReleaseMutex();
                }
            }
        }

        private void Save()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(Data));
            using (FileStream fs = new FileStream(Path, FileMode.Create))
                formatter.Serialize(fs, Config);
        }

        private void CreateNew()
        {
            Config = new Data
            {
                ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EDCTest;Trusted_Connection=True;",
                IconPath = "favicon.ico",
                Name = "EducationPortal"
            };
            Save();
        }

        private void Load()
        {
            if (!File.Exists(Path))
            {
                CreateNew();
                return;
            }

            XmlSerializer formatter = new XmlSerializer(typeof(Data));
            using (FileStream fs = new FileStream(Path, FileMode.OpenOrCreate))
                Config = (Data)formatter.Deserialize(fs);
        }

        public XMLConfigService(string path)
        {
            Path = path;
            Load();
        }

    }
}
