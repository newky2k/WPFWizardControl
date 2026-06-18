# DSoft.WizardControl.WPF
DSoft.WizardControl.WPF is a simple, customisable wizard `UserControl` for WPF, targeting .NET 10 (`net10.0-windows7.0` and `net10.0-windows10.0.18362`).

It supports

 - Databinding
 - Multiple pages
 - Validation
 - Theming

The shared, UI-agnostic types (`IWizardPage`, `WizardPageConfiguration`, enums, event args) live in the [DSoft.WizardControl.Core](https://www.nuget.org/packages/DSoft.WizardControl.Core) package, which this package depends on.

## Getting Started

The WPF Wizard control is a `UserControl` based element, so it can be used inside other `UserControl` objects or directly in a `Window`.

Install the NuGet package into your project via the Package Manager Console

    Install-Package Dsoft.WizardControl.WPF

Or install it via the Visual Studio NuGet Manager

In your `Window` or `UserControl` add a new namespace

    xmlns:wizard="clr-namespace:Dsoft.WizardControl.WPF;assembly=DSoft.WizardControl.WPF"

Then you can add the `WizardControl` to the xaml

    <Grid>
        <wizard:WizardControl 
            Title="{Binding Title}"  
            Pages="{Binding Pages}"  
            CancelCommand="{Binding CancelCommand}" 
            FinishCommand="{Binding FinishCommand}"/>
    </Grid

## Pages

The `WizardControl` hosts `UserControl` pages that implement the `IWizardPage` interface. `IWizardPage` exposes a `WizardPageConfiguration PageConfig` (per-page title, `CanGoBack`, `IsHidden`, `HideButtons`, plus `NavigationHandler` / `OnPageShownHandler` callbacks) and a `Task<bool> ValidateAsync()` method.

The `Pages` property of the `WizardControl` is expecting an `ObservableCollection<IWizardPage>` object which can be databound to a viewmodel or provided explicitly.

The `Title`, `CancelCommand` and `FinishCommand` can also be databound or provided explicitly.

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

The appearance of the `WizardControl` header can be modified by overriding the theme

You can also change the `ButtonStyle` in the same way. Whilst `ButtonStyle` can be used to assign one button style to all visible buttons,
individual button styles can be set as well. For individual button styles use code-behind, 
e.g. `wizard.UpdateButtonStyle(ButtonType.Next, "WizardRedButtonStyle");` or via property binding, e.g. `NextButtonStyle="{StaticResource WizardRedButtonStyle}"`.

Example styling xaml that can be added to the App.xaml or other xaml resource file

    xmlns:wizcont="clr-namespace:Dsoft.WizardControl.WPF;assembly=DSoft.WizardControl.WPF"

    <Style TargetType="{x:Type wizcont:WizardControl}">
        <Setter Property="ButtonStyle" Value="{StaticResource AccentedSquareButtonStyle}" />
    </Style>
    
    <Style TargetType="{x:Type wizcont:WizardControl}">
        <Setter Property="HeaderTemplate">
            <Setter.Value>
                <DataTemplate>
                    <StackPanel VerticalAlignment="Center" Orientation="Vertical" Margin="5">
                        <TextBlock Text="{Binding Title,FallbackValue=Heading}" FontSize="36"  Foreground="{StaticResource AccentColorBrush}"/>
                        <TextBlock Text="{Binding SubTitle,FallbackValue=SubHeading}" FontSize="12" Foreground="Gray"/>
                    </StackPanel>
                </DataTemplate>
            </Setter.Value>
        </Setter>
    </Style>
