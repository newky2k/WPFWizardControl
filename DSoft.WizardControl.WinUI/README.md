# Dsoft.WizardControl.WinUI
Dsoft.WizardControl.WinUI is a simple user control for WinUI 3.x and above

It supports

 - Databiding
 - Multiple pages
 - Validation
 - Themeing

## Getting Started

The WinUI Wizard control is a `UserControl` based element and so it be used in other `UserControl` objects or directly in a `Window`

Install the Nuget package into you project via the Package Management Console

    Install-Package Dsoft.WizardControl.WinUI

Or install it via the Visual Studio Nuget Manager

In your `Window` or `UserControl` add a new namespace

     xmlns:wizard="using:DSoft.WizardControl"

Then you can add the `WizardControl` to the xaml

    <Grid>
        <wizard:WizardControl 
            Title="{Binding Title}"  
            Pages="{Binding Pages}"  
            CancelCommand="{Binding CancelCommand}" 
            FinishCommand="{Binding FinishCommand}"/>
    </Grid

## Pages

The `WizardControl` uses `UserControl` that implements the `IWizardPage` interface.

The `Pages` property of the `WizardControl` is expecting a `ObservableCollection<IWizardPage>` object which can be databound to a viewmodel or provided explicitly.

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

You can also change the `ButtonStyle` in the same way

Example styling xaml that can be added to the App.xaml or other xaml resource file

    xmlns:wizard="using:DSoft.WizardControl"

    <Style x:Key="WizardButtons" TargetType="Button" BasedOn="{StaticResource MahApps.Styles.Button.MetroSquare.Accent}">

    </Style>

    <Style TargetType="wizard:WizardControl">
        <Setter Property="ButtonStyle" Value="{DynamicResource WizardButtons}" />
        <Setter Property="TitleTextStyle">
            <Setter.Value>
                <Style TargetType="TextBlock">
                    <Setter Property="FontSize" Value="36"/>
                    <Setter Property="Margin" Value="5,0,0,0" />
                    <Setter Property="FontFamily" Value="Segoe UI" />
                    <Setter Property="FontWeight" Value="Light" />
                    <Setter Property="Foreground" Value="LightBlue" />
                </Style>
            </Setter.Value>
        </Setter>
        <Setter Property="SubTitleTextStyle">
            <Setter.Value>
                <Style TargetType="TextBlock">
                    <Setter Property="FontSize" Value="24"/>
                    <Setter Property="FontFamily" Value="Segoe UI" />
                    <Setter Property="Foreground" Value="Gray" />
                    <Setter Property="Margin" Value="5,0,0,5" />
                </Style>
            </Setter.Value>
        </Setter>
    </Style>
