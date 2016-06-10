using System;
using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.ViewModels.DBTreeView
{
    // TODO replace with FolderItems for Triggers, Views and Indexes lateron
    [Obsolete("Needs to be replaced with concrete FolderItems for each type so they can have individual context menu actions.")]
    public class FolderItem : DirectoryItem
    {
    }
}
