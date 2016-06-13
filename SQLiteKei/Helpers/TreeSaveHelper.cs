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
        public static void SaveTree(ObservableCollection<TreeItem> tree, string filePath)
        {
            var xmlSerializer = InitSerializer();
            
            using (var streamWriter = new StreamWriter(filePath))
            {
                xmlSerializer.Serialize(streamWriter, tree);
            }
        }

        public static ObservableCollection<TreeItem> LoadTree(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new ObservableCollection<TreeItem>();
            }

            var xmlSerializer = InitSerializer();

            using (var streamReader = new StreamReader(filePath))
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
    }
}
