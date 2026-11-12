using Avalonia.Controls;
using Avalonia.Media;

namespace TaskManager.Controls;

public partial class TaskStatusBadge : UserControl
{
    public TaskStatusBadge()
    {
        InitializeComponent();
    }

    public void SetStatus(bool isCompleted)
    {
        if (isCompleted)
        {
            StatusText.Text = "Виконано";
            StatusBorder.Background = new SolidColorBrush(
                Color.Parse("#D5F5DF"));
            StatusText.Foreground = new SolidColorBrush(
                Color.Parse("#176B35"));
        }
        else
        {
            StatusText.Text = "Не виконано";
            StatusBorder.Background = new SolidColorBrush(
                Color.Parse("#FFF0D0"));
            StatusText.Foreground = new SolidColorBrush(
                Color.Parse("#805000"));
        }
    }
}