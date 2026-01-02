using System.Windows;
using System.Windows.Input;
using EditorLicenseGenerator.Services;

namespace EditorLicenseGenerator.ViewModels;

/// <summary>
/// ViewModel for Trial License (0xFC) generation
/// </summary>
public class TrialLicenseViewModel : BaseViewModel
{
    private readonly ILicenseGenerator _generator;
    
    private string _userName = string.Empty;
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

    public TrialLicenseViewModel(ILicenseGenerator generator)
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
            GeneratedLicense = _generator.GenerateTrialLicense(UserName);
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
