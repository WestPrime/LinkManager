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
                if (item.AttachmentType.Value != item.LinkType.AttachmentType)
                {
                    Link_Methods.ChangeType(doc, item.LinkType, item.AttachmentType.Value);
                }
            }
            UpdateData();
        }
        public void ListView_AttachmentSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.AttachmentType = AttachmentTypes[1];
                if (item.AttachmentType.Value != item.LinkType.AttachmentType)
                {
                    Link_Methods.ChangeType(doc, item.LinkType, item.AttachmentType.Value);
                }
            }
            UpdateData();
        }
        public void ListView_PublishCoordinates(object sender, RoutedEventArgs e)
        {
            if (LinksListView.SelectedItems.Count >= 1)
            {
                foreach (LinkItem item in LinksListView.SelectedItems)
                {
                    Link_Methods.PublishCoordinates(doc, item.LinkType);
                }
            }
            else
            {
                MessageBox.Show("Выделите связи, в которые необходимо опубликовать общую систему координат", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public void ListView_AcquierCoordinates(object sender, RoutedEventArgs e)
        {
            if (LinksListView.SelectedItems.Count == 1)
            {
                LinkItem item = LinksListView.SelectedItem as LinkItem;
                Link_Methods.AcquierCoordinates(doc, item.LinkType);
            }
            else
            {
                MessageBox.Show("Выделите одну связь, из которой нужно получить общую систему координат", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
