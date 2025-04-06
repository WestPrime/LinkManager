using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LinkManager
{
    [Transaction(TransactionMode.Manual)]
    public class Link_LoadFrom : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            WorksetConfiguration config = new WorksetConfiguration();
            List<RevitLinkType> links = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkType)).Cast<RevitLinkType>().ToList();
            string dirName = Link_Manager.iconsDirectoryPath + @"ПроектXX_XX\";
            Link_Methods.LoadFrom(links, dirName, config);
            return Result.Succeeded;
        }
    }
}
