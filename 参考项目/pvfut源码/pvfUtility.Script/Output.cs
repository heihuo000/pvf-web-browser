using System.Runtime.InteropServices;
using pvfUtility.Core;

namespace pvfUtility.Script
{
    [Guid("115F40AB-3F24-49A1-9FC3-0C8BAB6173D0")]
    public interface IOutput
    {
        void AppendText(string text);
    }
    [Guid("2C5B7580-4038-4d90-BABD-8B83FCE5A464")]
    [ClassInterface(ClassInterfaceType.None)]
    public class Output :IOutput
    {

        public void AppendText(string text)
        {
            AppCore.OutInpuyText(text);
        }
    }
}
