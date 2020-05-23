using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.WizardControl.Core
{
    public class WizardPageConfiguration
    {
        public string Title { get; set; }

        public bool IsHidden { get; set; }

        public bool CanGoBack { get; set; }

        public bool HideButtons { get; set; }

        /// <summary>
        /// Called when the page navigation is occuring
        /// </summary>
        public Action<WizardNavigationEventArgs> NavigationHandler { get; set; }

        public Action<IWizardControl> OnPageShownHandler { get; set; }

        public WizardPageConfiguration()
        {
            CanGoBack = true;
            IsHidden = false;
        }

        public WizardPageConfiguration(string title) : this()
        {
            Title = title;
        }

    }
}
