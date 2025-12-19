using System.ComponentModel;

namespace BasisJaar2.Models;

public class Level : INotifyPropertyChanged
{
    //  Id, Naam, Beschrijving, Tekst, IsUnlocked, IsCompleted

    public int Id { get; set; }

    private string _naam = "";
    public string Naam
    {
        get => _naam;
        set
        {
            if (_naam == value) return;
            _naam = value ?? "";
            OnPropertyChanged(nameof(Naam));
        }
    }

    private string _beschrijving = "";
    public string Beschrijving
    {
        get => _beschrijving;
        set
        {
            if (_beschrijving == value) return;
            _beschrijving = value ?? "";
            OnPropertyChanged(nameof(Beschrijving));
        }
    }

    private string _tekst = "";
    public string Tekst
    {
        get => _tekst;
        set
        {
            if (_tekst == value) return;
            _tekst = value ?? "";
            OnPropertyChanged(nameof(Tekst));
        }
    }

    // (Bestond al in code 2, mag blijven)
    private string _nummer = "";
    public string Nummer
    {
        get => _nummer;
        set
        {
            if (_nummer == value) return;
            _nummer = value ?? "";
            OnPropertyChanged(nameof(Nummer));
        }
    }

    // Voor UI (code 2 had notify + IconSource, dat houden we)
    private bool _isUnlocked;
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

    private bool _isCompleted;
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

    public string IconSource =>
        IsCompleted ? "completed.png"
        : IsUnlocked ? "unlocked.png"
        : "locked.png";

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
