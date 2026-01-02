using EditorLicenseGenerator.Services;

namespace EditorLicenseGenerator.ViewModels;

/// <summary>
/// Main ViewModel containing all license type ViewModels
/// </summary>
public class MainViewModel : BaseViewModel
{
    public TimeLicenseViewModel TimeLicense { get; }
    public VersionLicenseViewModel VersionLicense { get; }
    public TrialLicenseViewModel TrialLicense { get; }

    public MainViewModel()
    {
        var generator = new LicenseGenerator();
        
        TimeLicense = new TimeLicenseViewModel(generator);
        VersionLicense = new VersionLicenseViewModel(generator);
        TrialLicense = new TrialLicenseViewModel(generator);
    }
}
