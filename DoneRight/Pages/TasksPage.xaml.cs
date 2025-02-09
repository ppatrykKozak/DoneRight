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

    // Oznaczanie zadania jako ukoñczone
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
            string newDescription = await DisplayPromptAsync("Edytuj opis", "Nowy opis (pozostaw puste, aby usun¹æ):", initialValue: task.Description);

            // Jeœli u¿ytkownik wpisa³ coœ nowego - aktualizujemy tytu³
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                task.Title = newTitle;
            }

            // Jeœli u¿ytkownik wpisa³ pusty opis, ustawiamy `null` (lub pusty string)
            task.Description = string.IsNullOrWhiteSpace(newDescription) ? "" : newDescription;

            SaveTasksToFile();
        }
    }

    // Zapisywanie zadañ do pliku JSON
    private async void SaveTasksToFile()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");
        string json = JsonSerializer.Serialize(Tasks);
        await File.WriteAllTextAsync(filePath, json);
    }

    // Wczytywanie zadañ z pliku JSON
    private async void LoadTasksFromFile()
    {
        string filePath = Path.Combine(FileSystem.AppDataDirectory, "tasks.json");

        try
        {
            // Sprawdzenie, czy plik istnieje – jeœli nie, tworzymy nowy
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Plik tasks.json nie istnieje. Tworzê nowy plik.");
                await File.WriteAllTextAsync(filePath, "[]"); // Zapis pustej listy JSON
            }

            // Odczyt pliku
            string json = await File.ReadAllTextAsync(filePath);
            var loadedTasks = JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();

            // Aktualizacja listy zadañ
            Tasks.Clear();
            foreach (var task in loadedTasks)
            {
                Tasks.Add(task);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"B³¹d podczas wczytywania pliku: {ex.Message}");
        }
    }
}
