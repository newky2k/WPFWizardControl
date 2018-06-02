using Dsoft.WizardControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Dsoft.WizardControl.WPF
{
    /// <summary>
    /// Base Wizard ViewModel class
    /// </summary>
    internal class WizardControlViewModel : ViewModel
    {
        #region Fields

        private bool mPreviousEnabled;
        private bool mNextEnabled;
        private bool mFinishEnabled;
        private bool mCancelEnabled;
        private int mSelectedIndex;
        private List<KeyValuePair<String, Object>> mParameters;
        private String mHeading;
        private ObservableCollection<IWizardPage> mPages;

        #endregion

        #region Properties

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; NotifyPropertyChanged("Title"); }
        }

        /// <summary>
        /// Whats is the current selected index
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return mSelectedIndex;
            }
            set
            {
                mSelectedIndex = value;

                NotifyPropertyChanged("SelectedIndex");

            }
        }

        /// <summary>
        /// Is previous button enabled
        /// </summary>
        public Boolean PreviousEnabled
        {
            get { return mPreviousEnabled; }
            set
            {
                mPreviousEnabled = value;

                NotifyPropertyChanged("PreviousEnabled");
            }
        }

        /// <summary>
        /// Is next button enabled
        /// </summary>
        public Boolean NextEnabled
        {
            get { return mNextEnabled; }
            set
            {
                mNextEnabled = value;

                NotifyPropertyChanged("NextEnabled");
            }
        }

        /// <summary>
        /// Is finish button enabled
        /// </summary>
        public Boolean FinishEnabled
        {
            get { return mFinishEnabled; }
            set
            {
                mFinishEnabled = value;

                NotifyPropertyChanged("FinishEnabled");
            }
        }

        /// <summary>
        /// Is previous button enabled
        /// </summary>
        public Boolean CancelEnabled
        {
            get { return mCancelEnabled; }
            set
            {
                mCancelEnabled = value;

                NotifyPropertyChanged("CancelEnabled");
            }
        }

        /// <summary>
        /// Gets the pages for the wizard
        /// </summary>
        public ObservableCollection<IWizardPage> Pages
        {
            get { return mPages; }
            set
            {
                
                mPages = value;
                //mPages.CollectionChanged += mPages_CollectionChanged;
                NotifyPropertyChanged("Pages");

                if (mPages != null)
                {
                    foreach (IWizardPage NewPage in mPages)
                    {
                        NewPage.ViewModel.Parameters = mParameters;
                    }
                }


                RecalculateNavigation();
            }
        }

        /// <summary>
        /// Parameter list for communicating parameters / variables between pages.
        /// </summary>
        public List<KeyValuePair<String, Object>> Parameters
        {
            get { return mParameters; }
            set { mParameters = value; }
        }

        /// <summary>
        /// Current heading of the wizard
        /// </summary>
        public String SubTitle
        {
            get
            {
                if (mHeading == null)
                {
                    if (Pages != null && Pages.Count != 0)
                    {
                        mHeading = Pages[0].ViewModel.Title;
                    }
                    else
                    {
                        mHeading = String.Empty;
                    }

                }

                return mHeading;
            }
            set
            {
                if (mHeading != value)
                {
                    mHeading = value;

                    NotifyPropertyChanged("Heading");
                }
            }
        }

        #region Commands

        /// <summary>
        /// Gets the previous command.
        /// </summary>
        /// <value>
        /// The previous command.
        /// </value>
        public ICommand PreviousCommand
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the next command.
        /// </summary>
        /// <value>
        /// The next command.
        /// </value>
        public ICommand NextCommand
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the finish command.
        /// </summary>
        /// <value>
        /// The finish command.
        /// </value>
        public ICommand FinishCommand
        {
            get;
            protected set;
        }

        /// <summary>
        /// Command to be called when the wizard completes.  Should be set in the View to be called by the view model
        /// </summary>
        public ICommand CompleteCommand
        {
            get;
            set;
        }

        private ICommand cancelCommand;

        public ICommand CancelCommand
        {
            get { return cancelCommand; }
            set { cancelCommand = value; NotifyPropertyChanged("CancelCommand"); }
        }

        #endregion

        #endregion

        #region Contructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardControlViewModel"/> class.
        /// </summary>
        public WizardControlViewModel()
        {
            
            this.CancelEnabled = false;
            this.FinishEnabled = false;
            this.PreviousEnabled = false;
            this.NextEnabled = false;

            this.Pages = new ObservableCollection<IWizardPage>();
            this.Parameters = new List<KeyValuePair<string, object>>();

            PreviousCommand = new DelegateCommand(() =>
            {
                this.SelectedIndex = this.SelectedIndex - 1;

                this.SubTitle = Pages[this.SelectedIndex].ViewModel.Title;

                this.FinishEnabled = false;
                this.PreviousEnabled = false;
                this.NextEnabled = true;
            });

            NextCommand = new DelegateCommand(() =>
            {
                var cuItem = this.Pages[SelectedIndex];

                if (cuItem.ViewModel.Validate())
                {
                    this.SelectedIndex = this.SelectedIndex + 1;
                    this.SubTitle = Pages[this.SelectedIndex].ViewModel.Title;
                    this.PreviousEnabled = true;

                    if (this.SelectedIndex == mPages.Count - 1)
                    {
                        this.NextEnabled = false;
                        this.FinishEnabled = true;
                    }
                }

            });

            FinishCommand = new DelegateCommand(() =>
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(() => { DidFinish(); }));

            });

            CompleteCommand = new DelegateCommand(() => { });
        }
        #endregion

        #region Methods

        public virtual void DidFinish()
        {

        }

        public void RecalculateNavigation()
        {
            if (Pages.Count == 1)
            {
                this.CancelEnabled = false;
                this.FinishEnabled = true;
                this.PreviousEnabled = false;
                this.NextEnabled = false;
            }
            else
            {
                this.CancelEnabled = true;
                this.FinishEnabled = false;
                this.PreviousEnabled = false;
                this.NextEnabled = true;
            }
        }
        /// <summary>
        /// Set the Parameters property of the Pages added to be that of this class. ie centralise them to point here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mPages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (IWizardPage NewPage in e.NewItems)
            {
                NewPage.ViewModel.Parameters = mParameters;
            }

            if (Pages.Count == 1)
            {
                this.CancelEnabled = false;
                this.FinishEnabled = true;
                this.PreviousEnabled = false;
                this.NextEnabled = false;
            }
            else
            {
                this.CancelEnabled = true;
                this.FinishEnabled = false;
                this.PreviousEnabled = false;
                this.NextEnabled = true;
            }

        }

        #endregion
    }
}
