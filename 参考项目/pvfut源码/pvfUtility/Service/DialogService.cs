using System;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace pvfUtility.Service
{
    internal static class DialogService
    {
        public static TaskDialogResult Message(string instructionText, string text,
            TaskDialogStandardIcon icons, TaskDialogStandardButtons buttons, IntPtr handle)
        {
            var info = new TaskDialog
            {
                OwnerWindowHandle = handle,
                Caption = "pvfUtility",
                InstructionText = instructionText,
                Text = text,
                Icon = icons,
                StandardButtons = buttons
            };
            return info.Show();
        }

        public static bool AskYesNo(string instructionText, string text, IntPtr handle)
        {
            return Message(instructionText, text, TaskDialogStandardIcon.Information,
                       TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No, handle)
                   == TaskDialogResult.Yes;
        }

        public static void Error(string instructionText, string text, IntPtr handle)
        {
            Message(instructionText, text, TaskDialogStandardIcon.Error,
                TaskDialogStandardButtons.Yes, handle);
        }
    }
}