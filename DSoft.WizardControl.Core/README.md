# DSoft.WizardControl.Core
DSoft.WizardControl.Core is a support library for [DSoft.WizardControl.WPF](https://www.nuget.org/packages/DSoft.WizardControl.WPF) and [DSoft.WizardControl.WinUI](https://www.nuget.org/packages/DSoft.WizardControl.WinUI) libraries.

These libraries provide a customisable WizardControl for both platforms.

`DSoft.WizardControl.Core` is a UI-agnostic `netstandard2.0` library containing the shared contracts used by both controls:

 - `IWizardPage` / `IWizardControl` — page and navigation interfaces
 - `WizardPageConfiguration` — per-page title, `CanGoBack`, `IsHidden`, `HideButtons`, `NavigationHandler` / `OnPageShownHandler` callbacks
 - navigation enums and event args

You normally do not install this package directly — it is pulled in as a dependency of the WPF and WinUI packages below.

Platform/Feature               | Package name                              | Stable                              | Beta
-----------------------|-------------------------------------------|------------------------------------------------|----------------
WPF             | `DSoft.WizardControl.WPF` | [![NuGet](https://img.shields.io/nuget/v/DSoft.WizardControl.WPF.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.WizardControl.WPF/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.WizardControl.WPF.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.WizardControl.WPF/) 
WinUI             | `DSoft.WizardControl.WinUI` | [![NuGet](https://img.shields.io/nuget/v/DSoft.WizardControl.WinUI.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.WizardControl.WinUI/) | [![NuGet](https://img.shields.io/nuget/vpre/DSoft.WizardControl.WinUI.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/DSoft.WizardControl.WinUI/) 

