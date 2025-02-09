using DoneRight.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace DoneRight.Pages;

public partial class TasksPage : ContentPage
{
    public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

    public TasksPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadTasksFromFile();
    }

    // Dodawanie nowego zadania
    private void OnAddTaskClicked(object sender, EventArgs e)
    {
        string taskTitle = TaskEntry.Text;
        string taskDescription = TaskDescriptionEntry.Text;

        if (!string.IsNullOrWhiteSpace(taskTitle))
        {
            Tasks.Add(new TaskItem(taskTitle, taskDescription, false));
            TaskEntry.Text = "";
            TaskDescriptionEntry.Text = "";
            SaveTasksToFile();
        }
    }

    // Oznaczanie zadania jako uko�czone
    private void OnTaskCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var task = checkBox?.BindingContext as TaskItem;

        if (task != null)
        {
            task.IsCompleted = e.Value;
            SaveTasksToFile();
        }
    }

    // Usuwanie zadania
    private void DeleteItem(object sender, EventArgs e)
    {
        var button = sender as Button;
        var task = button?.BindingContext as TaskItem;

        if (task != null)
        {
            Tasks.Remove(task);
            SaveTasksToFile();
        }
    }

    // Edycja zadania
    private async void EditItem(object sender, EventArgs e)
    {
        var button = sender as Button;
        var task = button?.BindingContext as TaskItem;

        if (task != null)
        {
            // Pobranie nowej nazwy i opisu
            string newTitle = await DisplayPromptAsync("Edytuj zadanie", "Nowa nazwa:", initialValue: task.Title);
            string newDescription = await DisplayPromptAsync("Edytuj opis", "Nowy opis (pozostaw puste, aby usun��):", initialValue: task.Description);

            // Je�li u�ytkownik wpisa� co� nowego - aktualizujemy tytu�
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                task.Title = newTitle;
            }

            // Je�li u�ytkownik wpisa� pusty opis, ustawiamy `null` (lub pusty string)
            task.Description = string.IsNullOrWhiteSpace(newDescription) ? "" : newDescription;

            SaveTasksToFile();
        }
    }

    // Zapisywanie zada� do pliku JSON
    private async void SaveTasksToFile()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
        string json = JsonSerializer.Serialize(Tasks);
        await File.WriteAllTextAsync(filePath, json);
    }

    // Wczytywanie zada� z pliku JSON
    private async void LoadTasksFromFile()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
        string json = await File.ReadAllTextAsync(filePath);
        var loadTasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();

            foreach (var task in loadTasks)
            {
                Tasks.Add(task);
            }

    }
}
