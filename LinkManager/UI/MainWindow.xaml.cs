using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Windows.Controls;
using System.IO;

namespace LinkManager
{
    public partial class MainWindow : Window
    {
        struct Options // RomodanEA: советую привязывать функции к этой штуке
        {
            string       ChosenFilePath;
            bool         SearchInSubdirectories;
            PositionType PositionType;
            bool         SavePositions;
            bool         TransferCoordinates;
            bool         ShowOnlyProblems;
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

        public ICommand ActionCommand { get; }
        public Document doc;
        public List<RevitLinkType> LinkTypes;
        public string[] paths;
        public MainWindow(Document document)
        {
            doc = document;
            LinkTypes = Link_Methods.GetLinks(doc);
            InitializeComponent();
            string dirName = NameSearchField.Text;
            paths = Directory.GetFiles(dirName, "*.rvt", SearchOption.AllDirectories);

            ActionCommand = new RelayCommand(ExecuteAction);

            // Инициализация коллекций
            FileItems = new ObservableCollection<FileItem>();
            LinkItems = new ObservableCollection<LinkItem>();

            // Заполнение текущими данными
            UpdateData();

            DataContext = this;
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
    }

    public class FileItem
    {
        public string FileName { get; set; }
        public string Path { get; set; }
    }

    public class LinkItem
    {
        public string LinkName { get; set; }
        public string Status { get; set; }
        public Brush StatusColor { get; set; }
        public string ActionText { get; set; }
        public Brush ActionColor { get; set; }
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
}