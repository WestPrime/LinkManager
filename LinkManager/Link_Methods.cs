using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace LinkManager
{
    public class Link_Methods
    {
        public static List<RevitLinkType> GetLinks(Document doc) // Получить все ссылки в проекте
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            List<RevitLinkType> links = collector.OfClass(typeof(RevitLinkType)).Cast<RevitLinkType>().ToList();
            return links;
        }
        public static void Create(Document doc, string dirName, RevitLinkOptions options) // Добавить...
        {
            ImportPlacement placement = new ImportPlacement();
            MessageBoxResult result = MessageBox.Show("По общим координатам / с совмещением внутренних начал", 
                                                      "Выбор размещения связей в проекте", 
                                                      MessageBoxButton.YesNo, 
                                                      MessageBoxImage.Information, 
                                                      MessageBoxResult.Yes);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    placement = ImportPlacement.Shared;
                    break;
                case MessageBoxResult.No:
                    placement = ImportPlacement.Origin;
                    break;
            }
            string[] paths = Directory.GetFiles(dirName, "*.rvt", SearchOption.AllDirectories);
            foreach (string pathName in paths)
            {
                FilePath path = new FilePath(pathName);
                Transaction t = new Transaction(doc, "Добавить связь");
                t.Start();
                try
                {
                    LinkLoadResult link = RevitLinkType.Create(doc, path, options);
                    RevitLinkInstance.Create(doc, link.ElementId, placement);
                }
                catch { }
                t.Commit();
            }
        }
        public static void LoadFrom(List<RevitLinkType> links, string dirName, WorksetConfiguration config) // Обновить из...
        {
            foreach (RevitLinkType link in links)
            {
                string[] paths = Directory.GetFiles(dirName, link.Name, SearchOption.AllDirectories);
                if (paths.Length != 0)
                {
                    foreach (string pathName in paths)
                    {
                        FilePath path = new FilePath(pathName);
                        link.LoadFrom(path, config);
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Связь " + link.Name + " не найдена в указанной директории",
                        "Ошибка обновления связи",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Error,
                        MessageBoxResult.Yes
                    );

                    switch (result)
                    {
                        case MessageBoxResult.Yes: // Retry
                            LoadFrom(new List<RevitLinkType> { link }, dirName, config);
                            break;

                        case MessageBoxResult.No: // Cancel
                        default:
                            // Дополнительная обработка отмены при необходимости
                            break;
                    }
                }
            }
        }
        public static void Reload(List<RevitLinkType> links) // Обновить
        {
            foreach (var link in links)
            {
                link.Reload();
            }
        }
        public static void Unload(List<RevitLinkType> links) // Выгрузить
        {
            foreach (var link in links)
            {
                link.Unload(null);
            }
        }
        public static void Delete(Document doc, List<RevitLinkType> links) // Удалить
        {
            foreach (RevitLinkType link in links)
            {
                Transaction t = new Transaction(doc, "Удалить связь");
                t.Start();
                doc.Delete(link.Id);
                t.Commit();
            }
        }
    }
}
