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
    [TestFixture, Explicit]
    public class TreeSaveHelperTests : DbTestBase
    {
        private TreeSaveHelper treeSaveHelper;

        private string expectedLocation;

        private ObservableCollection<TreeItem> tree;

        [SetUp]
        public void Setup()
        {
            treeSaveHelper = new TreeSaveHelper();
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
            treeSaveHelper.Save(tree);

            Assert.IsTrue(File.Exists(expectedLocation));
        }

        [Test]
        public void LoadTree_WhenFileDoesNotExist_ReturnsEmptyCollection()
        {
            var tree = treeSaveHelper.Load();

            Assert.IsTrue(!tree.Any());
        }

        [Test]
        public void SaveTreeLoadTree_WithValidCollection_SaveAndLoadTreeCorrectly()
        {
            treeSaveHelper.Save(tree);
            var loadedTree = treeSaveHelper.Load();
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
