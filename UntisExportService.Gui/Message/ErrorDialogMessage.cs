using System;

namespace UntisExportService.Gui.Message
{
    public class ErrorDialogMessage : DialogMessage
    {
        public Exception Exception { get; set; }
    }
}
