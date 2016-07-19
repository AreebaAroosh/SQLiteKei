using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLiteKei.ViewModels.DBTreeView.Mapping
{
    /// <summary>
    /// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
    /// </summary>
    internal class SchemaToViewModelMapper
    {
        private string databasePath;

        private DatabaseHandler dbHandler;

        private readonly ILog logger = LogHelper.GetLogger();

        /// <summary>
        /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        /// <returns></returns>
        public DatabaseItem MapSchemaToViewModel(string databasePath)
        {
            logger.Info("Trying to load database file at " + databasePath);
            this.databasePath = databasePath;
            dbHandler = new DatabaseHandler(databasePath);

            TableFolderItem tableFolder = MapTables();
            FolderItem viewFolder = MapViews();
            FolderItem indexFolder = MapIndexes();
            FolderItem triggerFolder = MapTriggers();

            var databaseItem = new DatabaseItem()
            {
                DisplayName = Path.GetFileNameWithoutExtension(databasePath),
                DatabasePath = databasePath
            };

            databaseItem.Items.Add(tableFolder);
            databaseItem.Items.Add(viewFolder);
            databaseItem.Items.Add(indexFolder);
            databaseItem.Items.Add(triggerFolder);

            logger.Info("Loaded database " + databaseItem.DisplayName + ".");
            return databaseItem;
        }

        private TableFolderItem MapTables()
        {
            List<TableItem> tableViewItems = GenerateTableItems();

            var tableFolder = new TableFolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Tables") };

            foreach (var item in tableViewItems)
            {
                tableFolder.Items.Add(item);
            }

            return tableFolder;
        }

        private List<TableItem> GenerateTableItems()
        {
            var tables = dbHandler.GetTables();
            var tableViewItems = new List<TableItem>();

            foreach (var table in tables)
            {
                tableViewItems.Add(new TableItem
                {
                    DisplayName = table.Name,
                    DatabasePath = databasePath
                });
            }

            return tableViewItems;
        }


        private FolderItem MapIndexes()
        {
            var indexes = dbHandler.GetIndexes();

            IEnumerable indexNames = indexes.Select(x => x.Name);

            var indexFolder = new FolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Indexes") };

            foreach (string indexName in indexNames)
            {
                indexFolder.Items.Add(new IndexItem
                {
                    DisplayName = indexName,
                    DatabasePath = databasePath
                });
            }

            return indexFolder;
        }

        private FolderItem MapTriggers()
        {
            var triggers = dbHandler.GetTriggers();
            IEnumerable triggerNames = triggers.Select(x => x.Name);

            var triggerFolder = new FolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Triggers") };

            foreach (string triggerName in triggerNames)
            {
                triggerFolder.Items.Add(new TriggerItem
                {
                    DisplayName = triggerName,
                    DatabasePath = databasePath
                });
            }

            return triggerFolder;
        }

        private FolderItem MapViews()
        {
            var views = dbHandler.GetViews();
            IEnumerable viewNames = views.Select(x => x.Name);

            var viewFolder = new FolderItem { DisplayName = LocalisationHelper.GetString("TreeItem_Views") };

            foreach (string viewName in viewNames)
            {
                viewFolder.Items.Add(new ViewItem
                {
                    DisplayName = viewName,
                    DatabasePath = databasePath
                });
            }

            return viewFolder;
        }
    }
}
