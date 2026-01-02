# 010Editor License Generator - C# WPF GUI

A modern Windows desktop application built with .NET 8 and WPF for generating 010Editor licenses.

## Features

- **Time License** - Generate time-based licenses with customizable expiration days
- **Version License** - Generate version-specific licenses for particular 010Editor versions
- **Trial License** - Generate trial licenses (default 365 days)
- **Copy to Clipboard** - One-click copy for generated licenses
- **Modern UI** - Clean, tabbed interface following Windows design guidelines

## Requirements

- Windows 10/11
- .NET 8.0 SDK or Runtime

## Building the Application

### Using .NET CLI

```bash
# Navigate to the project directory
cd 010EditorLicenseGenerator

# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

### Using Visual Studio

1. Open `010EditorLicenseGenerator.csproj` in Visual Studio 2022
2. Build the solution (Ctrl+Shift+B)
3. Run the application (F5)

### Creating a Release Build

```bash
# Create a self-contained executable
dotnet publish -c Release -r win-x64 --self-contained true

# Or create a framework-dependent executable
dotnet publish -c Release -r win-x64 --self-contained false
```

The output will be in `bin/Release/net8.0-windows/win-x64/publish/`

## Project Structure

```
010EditorLicenseGenerator/
├── 010EditorLicenseGenerator.csproj  # Project file
├── App.xaml                          # Application entry point
├── App.xaml.cs
├── MainWindow.xaml                   # Main window UI
├── MainWindow.xaml.cs
├── Services/
│   └── LicenseGenerator.cs           # License generation algorithms
├── ViewModels/
│   ├── BaseViewModel.cs              # MVVM base class
│   ├── RelayCommand.cs               # ICommand implementation
│   ├── MainViewModel.cs              # Main window ViewModel
│   ├── TimeLicenseViewModel.cs       # Time license tab ViewModel
│   ├── VersionLicenseViewModel.cs    # Version license tab ViewModel
│   └── TrialLicenseViewModel.cs      # Trial license tab ViewModel
└── Resources/
    └── Styles.xaml                   # Application styles
```

## Usage

### Time License
1. Select the "Time License" tab
2. Enter the user name
3. Set the number of users
4. Set the number of days for the license validity
5. Click "Generate License"
6. Click "Copy to Clipboard" to copy the license

### Version License
1. Select the "Version License" tab
2. Enter the user name
3. Set the number of users
4. Set the version number (e.g., 14)
5. Click "Generate License"
6. Click "Copy to Clipboard" to copy the license

### Trial License
1. Select the "Trial License" tab
2. Enter the user name
3. Click "Generate License"
4. Click "Copy to Clipboard" to copy the license

## License Output Format

Generated licenses follow the format:
```
XXXX-XXXX-XXXX-XXXX-XXXX    (Time License - 10 bytes)
XXXX-XXXX-XXXX-XXXX         (Version/Trial License - 8 bytes)
```

## Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern:

- **Models/Services**: Core license generation logic ported from Python
- **ViewModels**: Handle UI state, commands, and business logic
- **Views**: XAML-based user interface

## Notes

- This is a port of the Python license generator to C# WPF
- The license generation algorithms produce identical output to the Python version
- For educational purposes only

## License

This project is for educational purposes only. Use at your own risk.
