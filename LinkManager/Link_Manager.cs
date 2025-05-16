using Autodesk.Revit.UI;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace LinkManager
{
    [DisplayName("Revit Link Manager")]
    [Description("C# Programming case")]
    public class Link_Manager : IExternalApplication
    {
        /// <summary>
        /// Конвертер Bitmap в BitmapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
        public static string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                             tabName = "Менеджер связей";
        public Result OnStartup(UIControlledApplication application)
        {
            BitmapImage blueCircle =   BitmapToBitmapImage(Properties.Resources.blue);
            BitmapImage greenPlus =    BitmapToBitmapImage(Properties.Resources.green_plus);
            BitmapImage reloadArrows = BitmapToBitmapImage(Properties.Resources.reload);

            application.CreateRibbonTab(tabName);
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Управление");

            PushButtonData[] pushButtons = new PushButtonData[]
            {
                new PushButtonData(nameof(Link_TestUI),   "Тестировать UI WPF", assemblyLocation, typeof(Link_TestUI).FullName  ) { LargeImage = blueCircle   }
            };
            foreach (PushButtonData buttonData in pushButtons) panel.AddItem(buttonData); 
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
