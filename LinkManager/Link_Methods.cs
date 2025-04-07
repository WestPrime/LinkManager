using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.IO;

namespace LinkManager
{
    public class Link_Methods
    {
        public static void Create(Document doc, string dirName, RevitLinkOptions options) // Добавить...
        {
            ImportPlacement placement = new ImportPlacement();
            TaskDialog dialog = new TaskDialog("Размещение связей");
            dialog.MainIcon = TaskDialogIcon.TaskDialogIconInformation;
            dialog.MainInstruction = "Выбор размещения связей в проекте";
            dialog.MainContent = "По общим координатам / с совмещением внутренних начал";
            dialog.CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No;
            dialog.DefaultButton = TaskDialogResult.Yes;
            TaskDialogResult result = dialog.Show();
            if (result == TaskDialogResult.Yes)
            {
                placement = ImportPlacement.Shared;
            }
            else if (result == TaskDialogResult.No)
            {
                placement = ImportPlacement.Origin;
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
                    TaskDialog dialog = new TaskDialog("Ошибка обновления связи");
                    dialog.MainIcon = TaskDialogIcon.TaskDialogIconError;
                    dialog.MainInstruction = "Не удалось найти связь";
                    dialog.MainContent = "Связь " + link.Name + " не найдена в указанной директории";
                    dialog.CommonButtons = TaskDialogCommonButtons.Retry | TaskDialogCommonButtons.Cancel;
                    dialog.DefaultButton = TaskDialogResult.Retry;
                    TaskDialogResult result = dialog.Show();
                    if (result == TaskDialogResult.Retry)
                    {
                        LoadFrom(new List<RevitLinkType> { link }, dirName, config);
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
