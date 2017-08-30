using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsoft.WizardControl.WPF
{
    /// <summary>
    /// Base View Model class
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Fields
        private BaseViewModel mParentViewModel;
        private Boolean mModelHasChanged = false;

        #endregion

        #region Properties

        /// <summary>
        /// References the Parent View Model, an example would be the Entity Record list View Model as a Parent of the Entity View Model.
        /// If the Entity is changed, the Parent View Model must refresh.
        /// </summary>
        public BaseViewModel ParentViewModel
        {
            get { return mParentViewModel; }
            set
            {
                if (mParentViewModel != value)
                {
                    mParentViewModel = value;
                    PropertyDidChange("ParentViewModel");
                }
            }
        }

        /// <summary>
        /// Indicates if the Model has changed, to faciliate Save buttons being enabled.
        /// </summary>
        public Boolean ModelHasChanged
        {
            get { return mModelHasChanged; }
            set
            {
                mModelHasChanged = value;
                //Bubble the ModelHasChanged Property.
                if (ParentViewModel != null)
                {
                    ParentViewModel.ModelHasChanged = value;
                }
            }
        }

        /// <summary>
        /// Name to display for the workspace for Tabs etc, to be implemented by inheriting classes.
        /// </summary>
        public virtual String Title { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Properties that did change.
        /// </summary>
        /// <param name="Name">The name.</param>
        public void PropertyDidChange(String Name)
        {
            VerifyPropertyName(Name);
            PropertyChanged(this, new PropertyChangedEventArgs(Name));
        }

        /// <summary>
        /// All Properties Changed (ie the Model changed).
        /// </summary>
        /// <param name="Name">The name.</param>
        public void AllPropertiesDidChange()
        {
            PropertyChanged(this, new PropertyChangedEventArgs(String.Empty));
        }

        /// <summary>
        /// Method to verify that a property name exists. as the string is hard coded for each .Net property.
        /// </summary>
        /// <param name="propertyName"></param>
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                throw new ArgumentOutOfRangeException(msg);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        #endregion
    }
}
