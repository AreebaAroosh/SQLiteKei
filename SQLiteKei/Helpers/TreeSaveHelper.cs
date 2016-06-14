using SQLiteKei.ViewModels.DBTreeView.Base;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var xmlSerializer = InitSerializer();

            using (var streamWriter = new StreamWriter(targetPath))
            {
                xmlSerializer.Serialize(streamWriter, tree);
            }
        }

        public static ObservableCollection<TreeItem> LoadTree()
        {
            var targetPath = GetSaveLocationPath();

            if (!File.Exists(targetPath))
            {
                return new ObservableCollection<TreeItem>();
            }

            var xmlSerializer = InitSerializer();

            using (var streamReader = new StreamReader(targetPath))
            {
                return xmlSerializer.Deserialize(streamReader) as ObservableCollection<TreeItem>;
            }
        }

        private static XmlSerializer InitSerializer()
        {
            var types = new List<Type>();

            // Get all inheriting types of TreeItem so that the whole tree can be serialized
            foreach (Type type in Assembly.GetAssembly(typeof (TreeItem)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf((typeof (TreeItem)))))
            {
                types.Add(type);
            }

            return new XmlSerializer(typeof(ObservableCollection<TreeItem>), types.ToArray());
        }

        private static string GetSaveLocationPath()
        {
            var roamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            return Path.Combine(roamingDirectory, "SQLiteKei", "TreeView.xml");;
        }
    }
}
