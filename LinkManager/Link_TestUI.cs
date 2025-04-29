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
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            var window = new MainWindow(doc);
            window.ShowDialog();
            window.Close();
            return Result.Succeeded;
        }
    }
}
