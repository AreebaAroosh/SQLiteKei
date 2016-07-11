using log4net;

using SQLiteKei.Helpers.Interfaces;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView.Mapping;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace SQLiteKei.Helpers
{
    public class TreeSaveHelper : ITreeSaveHelper
    {
        private ILog logger = LogHelper.GetLogger();

        public void Save(ObservableCollection<TreeItem> tree)
        {
            var targetPath = GetSaveLocationPath();
            var targetDirectory = Path.GetDirectoryName(targetPath);

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            var rootItemDatabasePaths = tree.Select(x => x.DatabasePath).ToList();
            var xmlSerializer = new XmlSerializer(typeof(List<string>));

            using (var streamWriter = new StreamWriter(targetPath))
            {
                try
                {
                    xmlSerializer.Serialize(streamWriter, rootItemDatabasePaths);
                    logger.Info("Successfully saved database tree.");
                }
                catch(Exception ex)
                {
                    logger.Error("Unable to save database tree.", ex);
                }
            }
        }

        public ObservableCollection<TreeItem> Load()
        {
            var targetPath = GetSaveLocationPath();

            if (!File.Exists(targetPath))
            {
                return new ObservableCollection<TreeItem>();
            }

            var xmlSerializer = new XmlSerializer(typeof(List<string>));
            var resultCollection = new ObservableCollection<TreeItem>();

            using (var streamReader = new StreamReader(targetPath))
            {
                try
                {
                    logger.Info("Restoring recently used databases...");

                    var databasePaths = xmlSerializer.Deserialize(streamReader) as List<string>;
                    var schemaMapper = new SchemaToViewModelMapper();

                    foreach (var path in databasePaths)
                    {
                        if (File.Exists(path))
                        {
                            var rootItem = schemaMapper.MapSchemaToViewModel(path);
                            resultCollection.Add(rootItem);
                        }
                        else
                        {
                            logger.Info("Could not restore database file at " + path + "\nThe file might have been moved or deleted outside of SQLK.");
                        }
                    }
                }
                catch(Exception ex)
                {
                    logger.Error("Could not load database tree.", ex);
                }
            }

            return resultCollection;
        }

        private static string GetSaveLocationPath()
        {
            var roamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return Path.Combine(roamingDirectory, "SQLiteKei", "TreeView.xml");
        }
    }
}
