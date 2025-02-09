using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoneRight.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        private string title;
        private string? description;
        private bool isCompleted;

        // Gdy wartość zostanie zmieniona, powiadamia UI o aktualizacji
        public string Title
        {
            get => title;
            set
            {
                if (title != value)
                {
                    title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }

        }
        // Gdy wartość zostanie zmieniona, powiadamia UI o aktualizacji
        public string? Description
        {
            get => description;
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        // Gdy wartość zostanie zmieniona, powiadamia UI o aktualizacji
        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }
        // Zdarzenie powiadamiające UI o zmianie wartości właściwości
        public event PropertyChangedEventHandler? PropertyChanged;
    
        //  Metoda wywoływana, gdy wartość właściwości się zmienia
        // Informuje UI, że powinien odświeżyć wyświetlane dane
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskItem() { }

        public TaskItem(string title, string? description, bool isCompleted)
        {
            Title = title;
            Description = description;
            IsCompleted = isCompleted;
        }
    }
}