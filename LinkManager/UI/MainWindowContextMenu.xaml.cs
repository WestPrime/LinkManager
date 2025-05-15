// Функции для контекстного меню

using System.Windows;

namespace LinkManager
{
    partial class MainWindow
    {
        public void ListView_CheckSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.IsSelected = true;
            }
        }

        public void ListView_UncheckSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.IsSelected = false;
            }
        }

        public void ListView_OverlaySelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.AttachmentType = AttachmentTypes[0];
            }
        }

        public void ListView_AttachmentSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.AttachmentType = AttachmentTypes[1];
            }
        }
    }
}
