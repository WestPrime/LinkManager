// Функции для контекстного меню

using System.Windows;

namespace LinkManager
{
    partial class MainWindow
    {
        #region Правая часть
        public void RightListView_CheckSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.IsSelected = true;
            }
        }

        public void RightListView_UncheckSelected(object sender, RoutedEventArgs e)
        {
            foreach (LinkItem item in LinksListView.SelectedItems)
            {
                item.IsSelected = false;
            }
        }

        public void RightListView_OverlaySelected(object sender, RoutedEventArgs e)
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

        public void RightListView_AttachmentSelected(object sender, RoutedEventArgs e)
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

        public void RightListView_PublishCoordinates(object sender, RoutedEventArgs e)
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

        public void RightListView_AcquierCoordinates(object sender, RoutedEventArgs e)
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
        #endregion
        #region Левая часть
        public void LeftListView_CheckSelected(object sender, RoutedEventArgs e)
        {
            foreach (FileItem item in FilesListView.SelectedItems)
            {
                item.IsSelected = true;
            }
        }

        public void LeftListView_UncheckSelected(object sender, RoutedEventArgs e)
        {
            foreach (FileItem item in FilesListView.SelectedItems)
            {
                item.IsSelected = false;
            }
        }
        #endregion
    }
}
