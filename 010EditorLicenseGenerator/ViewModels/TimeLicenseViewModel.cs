using System.Windows;
using System.Windows.Input;
using EditorLicenseGenerator.Services;

namespace EditorLicenseGenerator.ViewModels;

/// <summary>
/// ViewModel for Time License (0xAC) generation
/// </summary>
public class TimeLicenseViewModel : BaseViewModel
{
    private readonly ILicenseGenerator _generator;
    
    private string _userName = string.Empty;
    private int _numberOfUsers = 1;
    private int _days = 365;
    private string _generatedLicense = string.Empty;
    private string _statusMessage = string.Empty;

    public string UserName
    {
        get => _userName;
        set
        {
            if (SetProperty(ref _userName, value))
            {
                ((RelayCommand)GenerateCommand).RaiseCanExecuteChanged();
            }
        }
    }

    public int NumberOfUsers
    {
        get => _numberOfUsers;
        set => SetProperty(ref _numberOfUsers, value);
    }

    public int Days
    {
        get => _days;
        set => SetProperty(ref _days, value);
    }

    public string GeneratedLicense
    {
        get => _generatedLicense;
        set
        {
            if (SetProperty(ref _generatedLicense, value))
            {
                OnPropertyChanged(nameof(HasLicense));
            }
        }
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public bool HasLicense => !string.IsNullOrEmpty(GeneratedLicense);

    public ICommand GenerateCommand { get; }
    public ICommand CopyCommand { get; }

    public TimeLicenseViewModel(ILicenseGenerator generator)
    {
        _generator = generator;
        
        GenerateCommand = new RelayCommand(Generate, CanGenerate);
        CopyCommand = new RelayCommand(CopyToClipboard, () => HasLicense);
    }

    private bool CanGenerate()
    {
        return !string.IsNullOrWhiteSpace(UserName);
    }

    private void Generate()
    {
        try
        {
            GeneratedLicense = _generator.GenerateTimeLicense(UserName, NumberOfUsers, Days);
            StatusMessage = "License generated successfully!";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error: {ex.Message}";
            GeneratedLicense = string.Empty;
        }
    }

    private void CopyToClipboard()
    {
        if (!string.IsNullOrEmpty(GeneratedLicense))
        {
            Clipboard.SetText(GeneratedLicense);
            StatusMessage = "License copied to clipboard!";
        }
    }
}
