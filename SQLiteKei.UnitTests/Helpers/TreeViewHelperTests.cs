#region usings

using NUnit.Framework;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#endregion

namespace SQLiteKei.UnitTests.Helpers
{
    [TestFixture]
    public class TreeViewHelperTests
    {
        private ICollection<TreeItem> hierarchy;

        private TableItem LookupItem;

        private FolderItem LookupItemParent;

        private DatabaseItem LookupItemDatabase;

        private TableItem ComparisonItem;

        private FolderItem ComparisonItemParent;

        [SetUp]
        public void Setup()
        {
            LookupItem = new TableItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1"
            };

            LookupItemParent = new FolderItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1",
                Items = new ObservableCollection<TreeItem> { LookupItem }
            };

            LookupItemDatabase = new DatabaseItem
            {
                DisplayName = "Name",
                DatabasePath = "Database1",
                Items = new ObservableCollection<TreeItem> { LookupItemParent }
            };

            ComparisonItem = new TableItem
            {
                DisplayName = "Name",
                DatabasePath = "Database2"
            };

            ComparisonItemParent = new FolderItem
            {
                DisplayName = "Name",
                DatabasePath = "Database2",
                Items = new ObservableCollection<TreeItem> { ComparisonItem }
            };

            hierarchy = new ObservableCollection<TreeItem>
            {
                LookupItemDatabase,
                new DatabaseItem
                {
                    DisplayName = "Name",
                    DatabasePath = "Database2",
                    Items = new ObservableCollection<TreeItem>
                    {
                       ComparisonItemParent
                    }
                }
            };
        }

        [Test]
        public void RemoveItemFromHierarchy_WithExistingItem_RemovesSpecifiedItem()
        {
            TreeViewHelper.RemoveItemFromHierarchy(hierarchy, LookupItem);

            var result = LookupItemParent.Items.Any();
            Assert.IsFalse(result);
        }

        [Test]
        public void RemoveItemFromHierarchy_WithExistingItem_DoesNotRemoveItemsWithSameNameFromSameDatabase()
        {
            TreeViewHelper.RemoveItemFromHierarchy(hierarchy, LookupItem);

            var result = LookupItemDatabase.Items.Any();
            Assert.IsTrue(result);
        }

        [Test]
        public void RemoveItemFromHierarchy_WithExistingItem_DoesNotRemoveItemsWithSameNameFromOtherDatabase()
        {
            TreeViewHelper.RemoveItemFromHierarchy(hierarchy, LookupItem);

            var result = ComparisonItemParent.Items.Any();
            Assert.IsTrue(result);
        }
    }
}