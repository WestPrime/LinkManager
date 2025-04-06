using Autodesk.Revit.UI;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace LinkManager
{
    public class Link_Manager : IExternalApplication
    {
        public static string assemblyLocation = Assembly.GetExecutingAssembly().Location, 
                             tabName = "Менеджер связей", 
                             iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\LinkManagerData\";

        public Result OnStartup(UIControlledApplication application)
        {
            application.CreateRibbonTab(tabName);
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Управление");
            PushButtonData buttonData1 = new PushButtonData(nameof(Link_Create), "Добавить связи", assemblyLocation, typeof(Link_Create).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + @"blue.png"))
            };
            PushButtonData buttonData2 = new PushButtonData(nameof(Link_LoadFrom), "Обновить связи", assemblyLocation, typeof(Link_LoadFrom).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + @"blue.png"))
            };
            PushButtonData buttonData3 = new PushButtonData(nameof(Link_TestUI), "Тестировать UI WPF", assemblyLocation, typeof(Link_TestUI).FullName)
            {
                LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + @"blue.png"))
            };
            panel.AddItem(buttonData1);
            panel.AddItem(buttonData2);
            panel.AddItem(buttonData3);
            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
