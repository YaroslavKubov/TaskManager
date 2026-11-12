using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TaskManager.Controls;

public partial class TaskItemControl : UserControl
{
    private bool _isUpdating;

    public event Action<TaskItemControl, bool>? CompletionChanged;
    public event Action<TaskItemControl>? DeleteRequested;

    public TaskItemControl()
    {
        InitializeComponent();
    }

    public void Configure(string title, bool isCompleted)
    {
        _isUpdating = true;

        TitleText.Text = title;
        CompletedCheckBox.IsChecked = isCompleted;

        _isUpdating = false;
    }

    private void CompletedCheckBox_Changed(
        object? sender,
        RoutedEventArgs e)
    {
        if (_isUpdating)
            return;

        CompletionChanged?.Invoke(
            this,
            CompletedCheckBox.IsChecked == true);
    }

    private void DeleteButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        DeleteRequested?.Invoke(this);
    }
}