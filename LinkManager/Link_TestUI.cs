using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var window = new MyWpfWindow();
            window.ShowDialog();

            return Result.Succeeded;
        }
    }
}
