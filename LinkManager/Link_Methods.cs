using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
            List<RevitLinkType> links = collector.OfClass(typeof(RevitLinkType)).Cast<RevitLinkType>().Where(it => it.IsNestedLink == false).ToList();
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
            try
            {
                LinkLoadResult link = RevitLinkType.Create(doc, path, options);
                RevitLinkInstance.Create(doc, link.ElementId, placement);
            }
            catch (Autodesk.Revit.Exceptions.ApplicationException ex)
            {
                MessageBox.Show("Ошибка", ex.Message);
            }
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
                catch (Autodesk.Revit.Exceptions.ApplicationException ex)
                {
                    MessageBox.Show("Ошибка", ex.Message);
                }
            }
        }
        public static void Reload(List<RevitLinkType> links) // Обновить
        {
            foreach (RevitLinkType link in links)
            {
                try
                {
                    link.Reload();
                }
                catch (Autodesk.Revit.Exceptions.ApplicationException ex)
                {
                    MessageBox.Show("Ошибка", ex.Message);
                }
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
            Transaction t = new Transaction(doc, "Удалить связь");
            t.Start();
            foreach (RevitLinkType link in links)
            {
                try
                {
                    doc.Delete(link.Id);
                }
                catch (Autodesk.Revit.Exceptions.ApplicationException ex)
                {
                    MessageBox.Show("Ошибка", ex.Message);
                }
            }
            t.Commit();
        }
        public static void SavePositions (List<RevitLinkType> links) // Сохранить положения
        {
            foreach (var link in links)
            {
                try
                {
                    link.SavePositions(null);
                }
                catch (Autodesk.Revit.Exceptions.ApplicationException ex)
                {
                    MessageBox.Show("Ошибка", ex.Message);
                }
            }
        }
        public static void PublishCoordinates (Document doc, RevitLinkType link) // Передать координаты в связанную модель
        {
            RevitLinkInstance Link = null;
            List<RevitLinkInstance> revitLinks = new FilteredElementCollector(doc).OfClass(typeof(RevitLinkInstance)).Cast<RevitLinkInstance>().ToList();
            foreach (RevitLinkInstance revitLink in revitLinks)
            {
                if (revitLink.GetTypeId() == link.Id)
                {
                    Link = revitLink;
                }
            }
            ProjectLocation projectLocation = Link.Document.ActiveProjectLocation;
            LinkElementId locationId = new LinkElementId(Link.Id, projectLocation.Id);
            Transaction t = new Transaction(doc, "Передать координаты");
            t.Start();
            try
            {
                doc.PublishCoordinates(locationId);
            }
            catch (Autodesk.Revit.Exceptions.ApplicationException ex)
            {
                MessageBox.Show("Ошибка", ex.Message);
            }
            t.Commit();
        }
        public static void AcquierCoordinates(Document doc, RevitLinkType link) // Получить координаты из связанной модели
        {
            Transaction t = new Transaction(doc, "Получить координаты");
            t.Start();
            try
            {
                doc.AcquireCoordinates(link.Id);
            }
            catch (Autodesk.Revit.Exceptions.InvalidOperationException e)
            {
                MessageBox.Show("Координаты не были получены. Выбранная связь имеет те же координаты, что и главная модель", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            t.Commit();
        }
    }
}
