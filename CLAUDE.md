# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

A reusable wizard control distributed as NuGet packages for two XAML UI stacks: **WPF** (`DSoft.WizardControl.WPF`) and **WinUI 3** (`DSoft.WizardControl.WinUI`). Both share a UI-agnostic `netstandard2.0` core (`DSoft.WizardControl.Core`).

## Solution layout

Use `Dsoft.WizardControl.slnx` (new XML solution format) — it is the current solution. `Dsoft.WizardControl.sln` is the legacy MSBuild solution. The `slnx` contains:

- `DSoft.WizardControl.Core` — interfaces, enums, event args, page configuration. No UI dependency.
- `DSoft.WizardControl.WPF` — WPF NuGet package (targets net10 `-windows7.0` and `-windows10.0.18362`).
- `DSoft.WizardControl.WinUI` — WinUI 3 NuGet package (targets net10 `-windows10.0.19041.0`).
- `WpfAppNetCore`, `WinUISample` — runnable sample apps under the `Samples/` folder.

`Dsoft.WizardControl/Dsoft.WizardControl.WPFOld.csproj` is a legacy .NET Framework 4.6.1 project, **not** part of `slnx`. Do not modify it for new work.

## Shared-source architecture (important)

The WPF and WinUI controls are **one set of source files**, physically located in `DSoft.WizardControl.WinUI/` and named `*.uwp.winui.wpf.cs`:

- `WizardControl.uwp.winui.wpf.cs`, `DefaultCompleteView/ErrorView/ProgressView.uwp.winui.wpf.cs`, `DelegateCommand.shared.cs`.

The WPF project (`DSoft.WizardControl.WPF.csproj`) does **not** copy these — it `<Compile Include>`s them as linked files from the WinUI project. The two platforms diverge via preprocessor symbols:

- WPF sets `WPF` (see `<DefineConstants>...;WPF</DefineConstants>`), WinUI sets `WINUI`.
- Code branches with `#if WPF` (e.g. `System.Windows.Controls` vs `Microsoft.UI.Xaml.Controls`).

**Consequence:** editing a `*.uwp.winui.wpf.cs` file changes both packages. Any change must compile under both `WPF` and `WINUI` define paths.

## Core contracts

- `IWizardPage` — a page is a `UserControl` exposing `WizardPageConfiguration PageConfig` and `Task<bool> ValidateAsync()`.
- `IWizardControl` — navigation surface (`Navigate(NavigationDirection)`, `UpdateButtonVisibility`, `UpdateStage`, `RecalculateNavigation`).
- `WizardPageConfiguration` — per-page title, `CanGoBack`, `IsHidden`, `HideButtons`, plus `NavigationHandler` / `OnPageShownHandler` callbacks.
- `WizardControl` is a templated `Control` (default template in `DSoft.WizardControl.WPF/Themes/Generic.xaml`); host apps bind `Pages` (`ObservableCollection<IWizardPage>`), `Title`, `CancelCommand`, `FinishCommand`.

## Build & run

```sh
# Build the whole solution (Release builds NuGet packages — GeneratePackageOnBuild=true)
dotnet build Dsoft.WizardControl.slnx -c Release

# Build one target framework only (faster while iterating)
dotnet build DSoft.WizardControl.WPF/DSoft.WizardControl.WPF.csproj -f net10.0-windows7.0

# Run the WPF sample
dotnet run --project WpfAppNetCore/WpfAppNetCore.csproj
```

WinUI builds require a Windows RID/platform (x64/x86/ARM64), not `Any CPU` — see the per-project platform mappings in the `slnx`.

There is **no test project** in the solution.

## Build conventions (Directory.Build.props)

Applies to all projects: documentation XML generation, `GeneratePackageOnBuild`, MIT license, SourceLink (Release), and **strong-name signing** against `DSoft.snk` (`SignAssembly=true`). New code must compile clean under signing and XML-doc generation.

## Versioning & CI

- Azure Pipelines (`azure-pipelines-release.yml`) builds `master` on `windows-latest` with the .NET 10 SDK, stamps version from build number (`3.0.$(date...)`), and publishes `DSoft.*.nupkg` artifacts. `azure-pipelines-mergetest.yml` validates merges.
- Package versions are set per-project in the `.csproj` (`<Version>`); CI overrides via `/p:Version=`.

## Git

`master` is the release/PR target; active work happens on `dev`.
