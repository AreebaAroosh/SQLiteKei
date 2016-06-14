using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView.Mapping;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace SQLiteKei.Helpers
{
    public static class TreeSaveHelper
    {
        public static void SaveTree(ObservableCollection<TreeItem> tree)
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
                xmlSerializer.Serialize(streamWriter, rootItemDatabasePaths);
            }
        }

        public static ObservableCollection<TreeItem> LoadTree()
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
                var databasePaths = xmlSerializer.Deserialize(streamReader) as List<string>;
                var schemaMapper = new SchemaToViewModelMapper();
                foreach (var path in databasePaths)
                {
                    if (File.Exists(path))
                    {
                        var rootItem = schemaMapper.MapSchemaToViewModel(path);
                        resultCollection.Add(rootItem);
                    }
                }
            }

            return resultCollection;
        }

        private static string GetSaveLocationPath()
        {
            var roamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return Path.Combine(roamingDirectory, "SQLiteKei", "TreeView.xml");;
        }
    }
}
