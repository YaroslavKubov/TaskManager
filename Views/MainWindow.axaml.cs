using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TaskManager.Views;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<TaskItem> _tasks = new();

    public MainWindow()
    {
        InitializeComponent();

        TasksList.ItemsSource = _tasks;

        _tasks.CollectionChanged += Tasks_CollectionChanged;

        UpdateCounter();
    }

    private void Tasks_CollectionChanged(
        object? sender,
        NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (TaskItem task in e.NewItems)
            {
                task.PropertyChanged += Task_PropertyChanged;
            }
        }

        if (e.OldItems != null)
        {
            foreach (TaskItem task in e.OldItems)
            {
                task.PropertyChanged -= Task_PropertyChanged;
            }
        }

        UpdateCounter();
    }

    private void Task_PropertyChanged(
        object? sender,
        PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TaskItem.IsCompleted))
        {
            UpdateCounter();
        }
    }

    private void AddTask_Click(object? sender, RoutedEventArgs e)
    {
        string title = TaskInput.Text?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(title))
        {
            ErrorText.Text = "Введіть назву завдання!";
            ErrorText.IsVisible = true;
            return;
        }

        ErrorText.IsVisible = false;

        _tasks.Add(new TaskItem(title));

        TaskInput.Text = "";
    }

    private void DeleteTask_Click(object? sender, RoutedEventArgs e)
    {
        if (TasksList.SelectedItem is TaskItem selectedTask)
        {
            _tasks.Remove(selectedTask);
        }
    }

    private void UpdateCounter()
    {
        int count = _tasks.Count(task => !task.IsCompleted);

        CounterText.Text = $"Невиконаних завдань: {count}";
    }
}

public class TaskItem : INotifyPropertyChanged
{
    private bool _isCompleted;

    public string Title { get; }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            if (_isCompleted != value)
            {
                _isCompleted = value;
                OnPropertyChanged();
            }
        }
    }

    public TaskItem(string title)
    {
        Title = title;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}