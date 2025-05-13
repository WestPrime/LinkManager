// Функции для контекстного меню

using Autodesk.Revit.DB;
using System.Linq;
using System.Windows;

namespace LinkManager
{
    partial class MainWindow
    {
        public void ListView_CheckSelected(object sender, RoutedEventArgs e)
        {
            LinkItem[] selectedItems = LinkItems.Where(item => item.IsSelected).ToArray();
            LinksListView.SelectedItems.Clear();
            foreach (LinkItem item in selectedItems)
            {
                item.IsSelected = true;
            }
        }

        public void ListView_UncheckSelected(object sender, RoutedEventArgs e)
        {
            LinkItem[] selectedItems = LinkItems.Where(item => item.IsSelected).ToArray();
            foreach (LinkItem item in selectedItems)
            {
                item.IsSelected = false;
            }
        }

        public void ListView_OverlaySelected(object sender, RoutedEventArgs e)
        {
            LinkItem[] selectedItems = LinkItems.Where(item => item.IsSelected).ToArray();
            foreach (LinkItem item in selectedItems)
            {
                item.AttachmentType = new AttachmentTypeItem { Text = "Наложение", Value = AttachmentType.Overlay };
            }
        }

        public void ListView_AttachmentSelected(object sender, RoutedEventArgs e)
        {
            LinkItem[] selectedItems = LinkItems.Where(item => item.IsSelected).ToArray();
            foreach (LinkItem item in selectedItems)
            {
                item.AttachmentType = new AttachmentTypeItem { Text = "Прикрепление", Value = AttachmentType.Attachment };
            }
        }
    }
}
