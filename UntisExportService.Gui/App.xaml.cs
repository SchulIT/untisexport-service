using NLog.Targets;
using System.Windows;
using UntisExportService.Gui.NLog;

namespace UntisExportService.Gui
{
    public partial class App : Application
    {
        public App()
        {
            Target.Register<ListViewTarget>("ListView");
        }
    }
}
