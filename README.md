# DSoft.WizardControl

A reusable wizard `UserControl` for XAML apps, distributed as NuGet packages for two UI stacks:

| Package | UI stack | Target frameworks |
| --- | --- | --- |
| `DSoft.WizardControl.WPF` | WPF | `net10.0-windows7.0`, `net10.0-windows10.0.18362` |
| `DSoft.WizardControl.WinUI` | WinUI 3 | `net10.0-windows10.0.19041.0` |
| `DSoft.WizardControl.Core` | UI-agnostic core (interfaces, enums, page config) | `netstandard2.0` |

Both controls are built from one shared set of source files and a common `Core` library, so the WPF and WinUI APIs match.

It supports

 - Databinding
 - Multiple pages
 - Validation
 - Theming

## Getting Started

The Wizard control is a `UserControl` based element, so it can be used inside other `UserControl` objects or directly in a `Window`.

Install the NuGet package for your platform via the Package Manager Console

    # WPF
    Install-Package DSoft.WizardControl.WPF

    # WinUI 3
    Install-Package DSoft.WizardControl.WinUI

Or install it via the Visual Studio NuGet Manager.

Add the namespace to your `Window` or `UserControl`

    <!-- WPF -->
    xmlns:wizard="clr-namespace:Dsoft.WizardControl.WPF;assembly=DSoft.WizardControl.WPF"

    <!-- WinUI -->
    xmlns:wizard="using:DSoft.WizardControl"

Then add the `WizardControl` to the XAML

    <Grid>
        <wizard:WizardControl
            Title="{Binding Title}"
            Pages="{Binding Pages}"
            CancelCommand="{Binding CancelCommand}"
            FinishCommand="{Binding FinishCommand}"/>
    </Grid>

## Pages

The `WizardControl` hosts `UserControl` pages that implement the `IWizardPage` interface.

`IWizardPage` exposes a `WizardPageConfiguration PageConfig` (per-page title, `CanGoBack`, `IsHidden`, `HideButtons`, plus `NavigationHandler` / `OnPageShownHandler` callbacks) and a `Task<bool> ValidateAsync()` method.

The `Pages` property expects an `ObservableCollection<IWizardPage>` which can be databound to a viewmodel or set explicitly. `Title`, `CancelCommand` and `FinishCommand` can likewise be databound or set explicitly.

Below is an example ViewModel using `System.Mvvm`

    public class MainWindowViewModel : ViewModel
    {

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        public ObservableCollection<IWizardPage> Pages
        {
            get { return _pages; }
            set { _pages = value; NotifyPropertyChanged("Pages"); }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBox.Show("Bye!");

                    OnRequestCloseWindow?.Invoke(this, false);
                });
            }
        }

        public ICommand FinishCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    MessageBox.Show("Fin!");

                    OnRequestCloseWindow?.Invoke(this, false);
                });
            }
        }
    }

## Theming

The appearance of the `WizardControl` header can be modified by overriding the theme. You can also change the `ButtonStyle` in the same way.

Example WPF styling that can be added to App.xaml or another XAML resource file

    xmlns:wizcont="clr-namespace:Dsoft.WizardControl.WPF;assembly=DSoft.WizardControl.WPF"

    <Style TargetType="{x:Type wizcont:WizardControl}">
        <Setter Property="ButtonStyle" Value="{StaticResource AccentedSquareButtonStyle}" />
    </Style>

    <Style TargetType="{x:Type wizcont:WizardControl}">
        <Setter Property="HeaderTemplate">
            <Setter.Value>
                <DataTemplate>
                    <StackPanel VerticalAlignment="Center" Orientation="Vertical" Margin="5">
                        <TextBlock Text="{Binding Title,FallbackValue=Heading}" FontSize="36" Foreground="{StaticResource AccentColorBrush}"/>
                        <TextBlock Text="{Binding SubTitle,FallbackValue=SubHeading}" FontSize="12" Foreground="Gray"/>
                    </StackPanel>
                </DataTemplate>
            </Setter.Value>
        </Setter>
    </Style>

For WinUI theming examples, see [DSoft.WizardControl.WinUI/README.md](DSoft.WizardControl.WinUI/README.md).

## Samples

Runnable sample apps (grouped under the `Samples` solution folder):

 - `WpfAppNetCore` — WPF sample
 - `WinUISample` — WinUI 3 sample

## Building

Use the `Dsoft.WizardControl.slnx` solution.

    # Build everything (Release produces the NuGet packages)
    dotnet build Dsoft.WizardControl.slnx -c Release

    # Run the WPF sample
    dotnet run --project WpfAppNetCore/WpfAppNetCore.csproj

WinUI builds require a Windows RID/platform (x64/x86/ARM64), not `Any CPU`.

## License

MIT
