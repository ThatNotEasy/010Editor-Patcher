using System.Windows;
using System.Windows.Input;
using EditorLicenseGenerator.Services;

namespace EditorLicenseGenerator.ViewModels;

/// <summary>
/// ViewModel for Version License (0x9C) generation
/// </summary>
public class VersionLicenseViewModel : BaseViewModel
{
    private readonly ILicenseGenerator _generator;
    
    private string _userName = string.Empty;
    private int _numberOfUsers = 1;
    private int _version = 14;
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

    public int Version
    {
        get => _version;
        set => SetProperty(ref _version, value);
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

    public VersionLicenseViewModel(ILicenseGenerator generator)
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
            GeneratedLicense = _generator.GenerateVersionLicense(UserName, NumberOfUsers, Version);
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
