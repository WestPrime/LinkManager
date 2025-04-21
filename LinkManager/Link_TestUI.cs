using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows;

namespace LinkManager
{
    [Transaction(TransactionMode.Manual)]
    public class Link_TestUI : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (Application.Current == null)
            {
                new Application();
            }

            var window = new MainWindow();
            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}
