# Dsoft.WizardControl.WPF
Dsoft.WizardControl.WPF is a simple user control for WPF

It supports

 - Databiding
 - Multiple pages
 - Validation
 - Themeing

## Getting Started

The WPF Wizard control is a `UserControl` based element and so it be used in other `UserControl` objects or directly in a `Window`

Install the Nuget package into you project via the Package Management Console

    Install-Package Dsoft.WizardControl.WPF

Or install it via the Visual Studio Nuget Manager

In your `Window` or `UserControl` add a new namespace

    xmlns:wizard="clr-namespace:Dsoft.WizardControl.WPF;assembly=Dsoft.WizardControl.WPF"

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

    xmlns:wizcont="clr-namespace:Dsoft.WizardControl.WPF;assembly=Dsoft.WizardControl.WPF"

    <Style TargetType="{x:Type wizcont:WizardControl}">
        <Setter Property="ButtonStyle" Value="{StaticResource AccentedSquareButtonStyle}" />
    </Style>
    
    <Style TargetType="{x:Type wizcont:WizardCont}">
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
