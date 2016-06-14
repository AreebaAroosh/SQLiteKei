using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using NUnit.Framework;

using SQLiteKei.Helpers;
using SQLiteKei.IntegrationTests.Base;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.IntegrationTests.Helpers
{
    [TestFixture]
    public class TreeSaveHelperTests : DbTestBase
    {
        private string expectedLocation;

        private ObservableCollection<TreeItem> tree;

        [SetUp]
        public void Setup()
        {
            var roamingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            expectedLocation = Path.Combine(roamingDirectory, "SQLiteKei", "TreeView.xml");

            ClearRoamingDirectory();

            tree = new ObservableCollection<TreeItem>
            {
                new DatabaseItem
                {
                    DisplayName = DATABASEFILENAME,
                    DatabasePath = targetDatabaseFilePath,
                    Items = new ObservableCollection<TreeItem>
                    {
                        new TableFolderItem
                        {
                            DisplayName = "Folder",
                            DatabasePath = "DatabasePath",
                            Items = new ObservableCollection<TreeItem>
                            {
                                new TableItem
                                {
                                    DisplayName = "Table",
                                    DatabasePath = "DatabasePath"
                                }
                            }
                        }
                    }
                }
            };
        }

        [Test]
        public void SaveTree_WithValidTree_SavesFileInExpectedLocation()
        {
            TreeSaveHelper.SaveTree(tree);

            Assert.IsTrue(File.Exists(expectedLocation));
        }

        [Test]
        public void LoadTree_WhenFileDoesNotExist_ReturnsEmptyCollection()
        {
            var tree = TreeSaveHelper.LoadTree();

            Assert.IsTrue(!tree.Any());
        }

        [Test]
        public void SaveTreeLoadTree_WithValidCollection_SaveAndLoadTreeCorrectly()
        {
            TreeSaveHelper.SaveTree(tree);
            var loadedTree = TreeSaveHelper.LoadTree();
            var firstTreeItem = tree.First();
            var firstLoadedTreeItem = loadedTree.First();

            Assert.AreEqual(firstTreeItem.DisplayName, firstLoadedTreeItem.DisplayName);
            Assert.AreEqual(firstTreeItem.DatabasePath, firstLoadedTreeItem.DatabasePath);
        }

        private void ClearRoamingDirectory()
        {
            var expectedDirectory = Path.GetDirectoryName(expectedLocation);
            if (Directory.Exists(expectedDirectory))
            {
                Directory.Delete(expectedDirectory, true);
            }
        }
    }
}
