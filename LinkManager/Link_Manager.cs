using Autodesk.Revit.UI;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace LinkManager
{
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
                // Сохраняем Bitmap в MemoryStream в формате PNG (можно выбрать другой формат)
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0; // Сбрасываем позицию потока на начало

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad; // Загружаем изображение сразу
                bitmapImage.StreamSource = memory;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); // Опционально: делает изображение неизменяемым (полезно для многопоточности)

                return bitmapImage;
            }
        }

        public static string assemblyLocation   = Assembly.GetExecutingAssembly().Location, 
                             tabName            = "Менеджер связей", 
                             iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\LinkManagerData\";

        public Result OnStartup(UIControlledApplication application)
        {
            BitmapImage blueCircle = BitmapToBitmapImage(Properties.Resources.blue);
            BitmapImage greenPlus = BitmapToBitmapImage(Properties.Resources.green_plus);
            BitmapImage reloadArrows = BitmapToBitmapImage(Properties.Resources.reload);

            application.CreateRibbonTab(tabName);
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "Управление");
            PushButtonData buttonData1 = new PushButtonData(nameof(Link_Create),   "Добавить связи",     assemblyLocation, typeof(Link_Create).FullName  ) { LargeImage = greenPlus    };
            PushButtonData buttonData2 = new PushButtonData(nameof(Link_LoadFrom), "Обновить связи",     assemblyLocation, typeof(Link_LoadFrom).FullName) { LargeImage = reloadArrows };
            PushButtonData buttonData3 = new PushButtonData(nameof(Link_TestUI),   "Тестировать UI WPF", assemblyLocation, typeof(Link_TestUI).FullName  ) { LargeImage = blueCircle   };
            panel.AddItem(buttonData1); panel.AddItem(buttonData2); panel.AddItem(buttonData3);
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
