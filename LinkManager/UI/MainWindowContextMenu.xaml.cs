// Функции для контекстного меню

using System.Windows;

namespace LinkManager
{
    partial class MainWindow
    {
        public void ListView_InvertSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.IsSelected = !item.IsSelected; // заменить на тру после тестирования
            }
        }

        public void ListView_OverlaySelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                
            }
        }

        public void ListView_AttachmentSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                
            }
        }
    }
}
