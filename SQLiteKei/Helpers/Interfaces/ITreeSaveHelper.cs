using System.Collections.ObjectModel;

using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.Helpers.Interfaces
{
    public interface ITreeSaveHelper
    {
        void Save(ObservableCollection<TreeItem> tree);
        ObservableCollection<TreeItem> Load();
    }
}
