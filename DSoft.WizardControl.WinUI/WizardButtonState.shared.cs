using DSoft.WizardControl.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WPF
using System.Windows.Controls;
using System.Windows;
using NativeVisibility = System.Windows.Visibility;
#else
using Windows.UI.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NativeVisibility = Microsoft.UI.Xaml.Visibility;
#endif

namespace DSoft.WizardControl
{
    /// <summary>
    /// Button state for the wizard buttons: visibility, current, and default style.
    /// </summary>
    public class WizardButtonState
    {
        /// <summary>
        /// Gets or sets the button or combination of buttons displayed in the wizard interface.
        /// </summary>
        /// <remarks>Use this property to specify which navigation or action buttons (such as Next, Back,
        /// Finish, or Cancel) the state relates to</remarks>
        public WizardButtons Button { get; set; }

        /// <summary>
        /// Gets or sets the visibility state of the wizard button.
        /// </summary>
        public WizardButtonVisibility Visibility { get; set; }

        /// <summary>
        /// Gets or sets the style currently applied to the button.
        /// </summary>
        public Style ButtonStyleCurrent { get; set; }

        /// <summary>
        /// Default button style; not used at the moment but for future reference, 
        /// e.g., if we want to reset the button style to default after changing it.
        /// </summary>
        public Style ButtonStyleDefault { get; set; }

        /// <summary>
        /// Initializes a new instance of the WizardButtonState class with the specified button, visibility, and styles.
        /// </summary>
        /// <param name="button">The wizard button to which this state applies.</param>
        /// <param name="visibility">The visibility setting for the wizard button.</param>
        /// <param name="buttonStyleCurrent">The style to apply to the button in its current state. Can be null to use the default style.</param>
        /// <param name="buttonStyleDefault">The default style to apply to the button when no specific style is set. Can be null.</param>
        public WizardButtonState(WizardButtons button, WizardButtonVisibility visibility, Style buttonStyleCurrent, Style buttonStyleDefault)
        {
            Button = button;
            Visibility = visibility;
            ButtonStyleCurrent = buttonStyleCurrent;
            ButtonStyleDefault = buttonStyleDefault;
        }

        /// <summary>
        /// Convert WizardButtonVisibility to the native UI Framework Visibility
        /// </summary>
        public static NativeVisibility WizardVisibilityToVisibility(WizardButtonVisibility visibility)
            => visibility == WizardButtonVisibility.Visible ?
            NativeVisibility.Visible : NativeVisibility.Collapsed;

        /// <summary>
        /// Converts a NativeVisibility value to the corresponding WizardButtonVisibility value.
        /// </summary>
        /// <param name="visibility">The visibility state to convert.</param>
        /// <returns>A WizardButtonVisibility value that represents the equivalent visibility state. Returns
        /// WizardButtonVisibility.Visible if the input is NativeVisibility.Visible; otherwise, returns
        /// WizardButtonVisibility.Collapsed.</returns>
        public static WizardButtonVisibility VisibilityToWizardVisibility(NativeVisibility visibility)
            => visibility == NativeVisibility.Visible ?
            WizardButtonVisibility.Visible : WizardButtonVisibility.Collapsed;

        /// <summary>
        /// Convert WizardButtonVisibility to the native UI Framework Visibility
        /// </summary>
        public NativeVisibility GUIVisibility
        {
            get => WizardVisibilityToVisibility(Visibility);
            set => Visibility = VisibilityToWizardVisibility(value);
        }
    }
}
