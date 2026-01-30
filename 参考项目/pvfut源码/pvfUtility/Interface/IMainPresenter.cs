using System;
using System.Threading.Tasks;

namespace pvfUtility.Interface
{
    internal interface IMainPresenter
    {
        void DoAction(string statusText, Func<IProgress<int>, Task> action, bool closeAllDocument = false);

        void SetStatusText(string text);
    }
}