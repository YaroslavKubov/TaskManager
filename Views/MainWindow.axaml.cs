using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using TaskManager.Controls;

namespace TaskManager.Views;

public partial class MainWindow : Window
{
    private readonly List<TaskEntry> _tasks = new();

    public MainWindow()
    {
        InitializeComponent();

        UpdateCounter();
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

        var taskControl = new TaskItemControl();
        var statusBadge = new TaskStatusBadge();

        taskControl.Configure(title, false);
        statusBadge.SetStatus(false);

        var row = new StackPanel
        {
            Spacing = 4
        };

        row.Children.Add(taskControl);
        row.Children.Add(statusBadge);

        var entry = new TaskEntry(
            title,
            taskControl,
            statusBadge,
            row
        );

        taskControl.CompletionChanged += (control, isCompleted) =>
        {
            entry.IsCompleted = isCompleted;

            entry.StatusBadge.SetStatus(isCompleted);

            UpdateCounter();
        };

        taskControl.DeleteRequested += control =>
        {
            _tasks.Remove(entry);
            TasksPanel.Children.Remove(entry.Row);

            UpdateCounter();
        };

        _tasks.Add(entry);
        TasksPanel.Children.Add(row);

        TaskInput.Text = "";

        UpdateCounter();
    }

    private void UpdateCounter()
    {
        int count = _tasks.Count(task => !task.IsCompleted);

        CounterText.Text =
            $"Невиконаних завдань: {count}";
    }

    private class TaskEntry
    {
        public string Title { get; }

        public bool IsCompleted { get; set; }

        public TaskItemControl TaskControl { get; }

        public TaskStatusBadge StatusBadge { get; }

        public StackPanel Row { get; }

        public TaskEntry(
            string title,
            TaskItemControl taskControl,
            TaskStatusBadge statusBadge,
            StackPanel row)
        {
            Title = title;
            TaskControl = taskControl;
            StatusBadge = statusBadge;
            Row = row;
            IsCompleted = false;
        }
    }
}