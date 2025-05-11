using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

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
        public static void ChangeType(Document doc, RevitLinkType link, AttachmentType type) // Изменить тип связи
        {
            Transaction t = new Transaction(doc, "Изменить тип связи");
            t.Start();
            link.AttachmentType = type;
            t.Commit();
        }
        public static void Create(Document doc, string dirName, RevitLinkOptions options, ImportPlacement placement) // Добавить...
        {
            FilePath path = new FilePath(dirName);
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
        public static void LoadFrom(List<RevitLinkType> links, string dirName, WorksetConfiguration config) // Обновить из...
        {
            foreach (RevitLinkType link in links)
            {
                try
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
                }
                catch { }
            }
        }
        public static void Reload(List<RevitLinkType> links) // Обновить
        {
            foreach (RevitLinkType link in links)
            {
                link.Reload();
            }
        }
        public static void Unload(List<RevitLinkType> links) // Выгрузить
        {
            foreach (RevitLinkType link in links)
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
        public static void SavePositions (List<RevitLinkType> links) // Сохранить положения
        {
            foreach(var link in links)
            {
                link.SavePositions(null);
            }
        }
        public static void PublishCoordinates (Document doc, List<RevitLinkType> links) // Передать координаты в связанную модель
        {
            foreach(var link in links)
            {
                ProjectLocation projectLocation = link.Document.ActiveProjectLocation;
                LinkElementId locationId = new LinkElementId(link.Id, projectLocation.Id);
                doc.PublishCoordinates(locationId);
            }
        }
        public static void AcquierCoordinates(Document doc, List<RevitLinkType> links) // Получить координаты из связанной модели
        {
            foreach (var link in links)
            {
                doc.AcquireCoordinates(link.Id);
            }
        }
    }
}
