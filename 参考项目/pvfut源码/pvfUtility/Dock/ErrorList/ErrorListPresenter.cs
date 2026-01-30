using System.Collections.Generic;
using pvfUtility.Model;
using pvfUtility.Shell;

namespace pvfUtility.Dock.ErrorList
{
    internal class ErrorListPresenter
    {
        public List<ErrorListItem> ErrorItems = new List<ErrorListItem>();
        public static ErrorListPresenter Instance { get; } = new ErrorListPresenter();

        public ErrorListDockView View { get; } = new ErrorListDockView();

        public void AddError(ErrorListItem item)
        {
            View.AddError(item);
            ErrorItems.Add(item);
        }

        public void RemoveError(string fileName)
        {
            var item = ErrorItems.Find(x => x.FileName == fileName);
            if (item == null)
                return;
            ErrorItems.Remove(item);
            View.RemoveError(item);
            RemoveError(fileName);
        }
    }
}