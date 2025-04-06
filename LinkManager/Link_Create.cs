using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LinkManager
{
    [Transaction(TransactionMode.Manual)]
    public class Link_Create : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            RevitLinkOptions options = new RevitLinkOptions(true);
            string dirName = Link_Manager.iconsDirectoryPath + @"ПроектXX_XX\";
            Link_Methods.Create(doc, dirName, options);
            return Result.Succeeded;
        }
    }
}
