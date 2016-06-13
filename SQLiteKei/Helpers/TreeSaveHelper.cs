using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.ObjectModel;
using System.IO;

using System.Xml.Serialization;

namespace SQLiteKei.Helpers
{
    public static class TreeSaveHelper
    {
        public static void SaveTree(ObservableCollection<TreeItem> tree, string filePath)
        {
           

            XmlSerializer xs = new XmlSerializer(typeof (ObservableCollection<TreeItem>),
                new[] {typeof (DatabaseItem), typeof (FolderItem), typeof (TableItem)});
            //using (StreamWriter wr = new StreamWriter("test.xml"))
            //{
            //    xs.Serialize(wr, hierarchy);
            //}

            using (StreamReader sr = new StreamReader("test.xml"))
            {
                var x = new ObservableCollection<TreeItem>();

                x = xs.Deserialize(sr) as ObservableCollection<TreeItem>;
            }
        }

        public static void LoadTree(string filePath)
        {
            var t = InitSerializer();
        }

        private static XmlSerializer InitSerializer()
        {
            List<Type> types = new List<Type>();

            // Get all inheriting types of TreeItem
            foreach (Type type in Assembly.GetAssembly(typeof (TreeItem)).GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf((typeof (TreeItem)))))
            {
                types.Add(type);
            }
            
            return new XmlSerializer(typeof(TreeItem), types.ToArray());
        }
    }
}
