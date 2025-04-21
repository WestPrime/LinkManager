using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using System;

namespace LinkManager
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<FileItem> FileItems { get; set; }
        public ObservableCollection<LinkItem> LinkItems { get; set; }

        public ICommand ActionCommand { get; }

        public MainWindow()
        {
            InitializeComponent();

            ActionCommand = new RelayCommand(ExecuteAction);

            // Инициализация коллекций
            FileItems = new ObservableCollection<FileItem>();
            LinkItems = new ObservableCollection<LinkItem>();

            // Заполнение тестовыми данными
            FileItems.Add(new FileItem
            {
                FileName = "Проект_А.rvt",
                Path = @"C:\RevitProjects\Main"
            });

            LinkItems.Add(new LinkItem
            {
                LinkName = "Инженерные системы.rvt",
                Status = "Актуально",
                StatusColor = Brushes.Green,
                ActionText = "Обновить",
                ActionColor = Brushes.DodgerBlue
            });

            DataContext = this;
        }

        private void ExecuteAction(object parameter)
        {
            if (parameter is LinkItem link)
            {
                // Логика обработки действия
                MessageBox.Show($"Выполнено действие: {link.ActionText} для {link.LinkName}");
            }
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