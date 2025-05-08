﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Windows.Controls;
using System.IO;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.ComponentModel;
using System.Linq;

namespace LinkManager
{
    public partial class MainWindow : Window
    {
        struct Options // RomodanEA: советую привязывать функции к этой штуке
        {
            string ChosenFilePath;
            bool SearchInSubdirectories;
            PositionType PositionType;
            bool SavePositions;
            bool TransferCoordinates;
            bool ShowOnlyProblems;
        }

        public enum PositionType
        {
            ByCommonCoordinatesRadio,
            ByBasePointRadio,
            AlignInnerBeginningsRadio,
            GetCoordinatesFromModelRadio
        }

        public PositionType GetPositionType()
        {
            if (ByCommonCoordinatesRadio.IsChecked == true)
                return PositionType.ByCommonCoordinatesRadio;
            if (ByBasePointRadio.IsChecked == true)
                return PositionType.ByBasePointRadio;
            if (AlignInnerBeginningsRadio.IsChecked == true)
                return PositionType.AlignInnerBeginningsRadio;
            if (GetCoordinatesFromModelRadio.IsChecked == true)
                return PositionType.GetCoordinatesFromModelRadio;

            // Возвращаем значение по умолчанию, если ни один не выбран
            return PositionType.ByCommonCoordinatesRadio; // или можно выбросить исключение
        }

        public ObservableCollection<FileItem> FileItems { get; set; }
        public ObservableCollection<LinkItem> LinkItems { get; set; }

        // Код для чекбоксов
        private bool _isAllFilesSelected;
        public bool? IsAllFilesSelected
        {
            get => _isAllFilesSelected;
            set
            {
                if (_isAllFilesSelected != value)
                {
                    _isAllFilesSelected = value ?? false;
                    OnPropertyChanged(nameof(IsAllFilesSelected));
                    UpdateFilesSelection(_isAllFilesSelected);
                }
            }
        }

        private bool _isAllLinksSelected;
        public bool? IsAllLinksSelected
        {
            get => _isAllLinksSelected;
            set
            {
                if (_isAllLinksSelected != value)
                {
                    _isAllLinksSelected = value ?? false;
                    OnPropertyChanged(nameof(IsAllLinksSelected));
                    UpdateLinksSelection(_isAllLinksSelected);
                }
            }
        }

        public ICommand ToggleAllFilesCommand { get; }
        public ICommand ToggleAllLinksCommand { get; }


        public ICommand ActionCommand { get; }
        public Document doc;
        public List<RevitLinkType> LinkTypes;
        public string[] paths;
        public MainWindow(Document document)
        {
            doc = document;
            LinkTypes = Link_Methods.GetLinks(doc);
            InitializeComponent();
            DataContext = this;
            string dirName = NameSearchField.Text;
            paths = Directory.GetFiles(dirName, "*.rvt", SearchOption.AllDirectories);

            ActionCommand = new RelayCommand(ExecuteAction);

            // Инициализация коллекций
            FileItems = new ObservableCollection<FileItem>();
            LinkItems = new ObservableCollection<LinkItem>();

            // Команда для чекбокса "Все"
            ToggleAllFilesCommand = new RelayCommand(param =>
            {
                IsAllFilesSelected = !IsAllFilesSelected;
            });
            ToggleAllLinksCommand = new RelayCommand(param =>
            {
                IsAllLinksSelected = !IsAllLinksSelected;
            });

            // Заполнение текущими данными
            UpdateData();
        }
        private void UpdateData() // Обновление данных
        {
            FileItems.Clear();
            LinkItems.Clear();
            if (LinkTypes.Count != 0)
            {
                foreach (RevitLinkType link in LinkTypes)
                {
                    LinkItem item = new LinkItem
                    {
                        LinkName = link.Name,
                        ActionText = "Обновить",
                        ActionColor = Brushes.DodgerBlue
                    };
                    LinkedFileStatus link_status = link.GetLinkedFileStatus();
                    if (link_status == LinkedFileStatus.Loaded)
                    {
                        item.Status = "Загружено";
                        item.StatusColor = Brushes.Green;
                    }
                    else
                    {
                        item.Status = "Не загружено";
                        item.StatusColor = Brushes.Red;
                    }
                    LinkItems.Add(item);
                }
            }
            if (paths.Length != 0)
            {
                foreach (string path in paths)
                {
                    string filename = new DirectoryInfo(path).Name;
                    FileItem item = new FileItem
                    {
                        FileName = filename,
                        Path = path,
                    };
                    FileItems.Add(item);
                }
            }
        }
        private void ExecuteAction(object parameter)
        {
            if (parameter is LinkItem link)
            {
                // Логика обработки действия
                MessageBox.Show($"Выполнено действие: {link.ActionText} для {link.LinkName}");
            }
        }

        /// <summary>
        /// Когда нажата кнопка "Выбрать файл"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    NameSearchField.Text = dialog.SelectedPath;
                }
            }
        }

        /// <summary>
        /// Когда нажата кнопка "Обновить все"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshAllButton_Click(object sender, RoutedEventArgs e)
        {
            Link_Methods.Reload(LinkTypes);
            UpdateData();
        }

        /// <summary>
        /// Когда отмечается чекбокс "Искать в подпапках"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubdirectorySearchCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Когда изменился текст в поле "Поиск по имени"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameSearchField_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        /// <summary>
        /// Когда нажата кнопка "Выгрузить все"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnloadAllButton_Click(object sender, RoutedEventArgs e)
        {
            Link_Methods.Unload(LinkTypes);
            UpdateData();
        }

        /// <summary>
        /// Когда нажата кнопка "Удалить все"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            Link_Methods.Delete(doc, LinkTypes);
            UpdateData();
        }

        /// <summary>
        /// Когда отмечается чекбокс "Сохранить положения"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePositionsCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Когда отмечается чекбокс "Передать координаты"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TransferCoordinatesRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Когда отмечается чекбокс "Показывать только проблемы"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowOnlyProblemsCheckbox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void UpdateFilesSelection(bool isSelected)
        {
            foreach (var item in FileItems)
            {
                item.IsSelected = isSelected;
            }
        }

        private void UpdateLinksSelection(bool isSelected)
        {
            if (LinkItems == null)
            {
                LinkItems = new ObservableCollection<LinkItem>();
            }
            foreach (var item in LinkItems)
            {
                item.IsSelected = isSelected;
            }
        }

        private void FileItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FileItem.IsSelected))
            {
                bool allSelected = FileItems.All(item => item.IsSelected);
                IsAllFilesSelected = allSelected ? true : false;
            }
        }

        private void LinkItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LinkItem.IsSelected))
            {
                bool allSelected = LinkItems.All(item => item.IsSelected);
                IsAllLinksSelected = allSelected ? true : false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is System.Windows.Controls.ListViewItem listViewItem && listViewItem.DataContext is INotifyPropertyChanged item)
            {
                if (item is FileItem fileItem)
                {
                    fileItem.IsSelected = !fileItem.IsSelected;
                }
                else if (item is LinkItem linkItem)
                {
                    linkItem.IsSelected = !linkItem.IsSelected;
                }
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox && checkBox.DataContext is INotifyPropertyChanged item)
            {
                if (item is FileItem fileItem)
                {
                    fileItem.IsSelected = true;
                }
                else if (item is LinkItem linkItem)
                {
                    linkItem.IsSelected = true;
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.CheckBox checkBox && checkBox.DataContext is INotifyPropertyChanged item)
            {
                if (item is FileItem fileItem)
                {
                    fileItem.IsSelected = false;
                }
                else if (item is LinkItem linkItem)
                {
                    linkItem.IsSelected = false;
                }
            }
        }
    }

}

public class FileItem : INotifyPropertyChanged
{
    private bool _isSelected;
    public string FileName { get; set; }
    public string Path { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class LinkItem : INotifyPropertyChanged
{
    private bool _isSelected;

    public string LinkName { get; set; }
    public string Status { get; set; }
    public Brush StatusColor { get; set; }
    public string ActionText { get; set; }
    public Brush ActionColor { get; set; }

    //public RevitLinkType ToRevitLinkType
    //{
    //    get
    //    {
    //        return new RevitLinkType()
    //        {
    //            // Указать параметры возвращаемого
    //        };
    //    }
    //}

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Реализация RelayCommand
public class RelayCommand : ICommand
{
    private readonly Action<object> _execute;

    public RelayCommand(Action<object> execute) => _execute = execute;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter) => _execute(parameter);

    public event EventHandler CanExecuteChanged;
}