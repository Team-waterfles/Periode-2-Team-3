using System.ComponentModel;

namespace BasisJaar2.Models;

public class Level : INotifyPropertyChanged
{
    // Extra properties voor oude code (LevelSelectieViewModel)
    public int Id { get; set; }              // kan gebruikt worden als numeriek id
    public string Tekst { get; set; } = "";  // optioneel: eigen tekst per level

    private string _nummer;
    private string _naam;
    private string _beschrijving;
    private bool _isUnlocked;
    private bool _isCompleted;

    public string Nummer
    {
        get => _nummer;
        set
        {
            if (_nummer == value) return;
            _nummer = value;
            OnPropertyChanged(nameof(Nummer));
        }
    }

    public string Naam
    {
        get => _naam;
        set
        {
            if (_naam == value) return;
            _naam = value;
            OnPropertyChanged(nameof(Naam));
        }
    }

    public string Beschrijving
    {
        get => _beschrijving;
        set
        {
            if (_beschrijving == value) return;
            _beschrijving = value;
            OnPropertyChanged(nameof(Beschrijving));
        }
    }

    // Voor UI
    public bool IsUnlocked
    {
        get => _isUnlocked;
        set
        {
            if (_isUnlocked == value) return;
            _isUnlocked = value;
            OnPropertyChanged(nameof(IsUnlocked));
            OnPropertyChanged(nameof(IconSource));
        }
    }

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            if (_isCompleted == value) return;
            _isCompleted = value;
            OnPropertyChanged(nameof(IsCompleted));
            OnPropertyChanged(nameof(IconSource));
        }
    }

    /// <summary>
    /// Geeft het icoon voor dit level:
    /// - completed.png als level gehaald is
    /// - unlocked.png als level open is
    /// - locked.png als level nog dicht zit
    /// </summary>
    public string IconSource =>
        IsCompleted ? "completed.png"
        : IsUnlocked ? "unlocked.png"
        : "locked.png";

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
