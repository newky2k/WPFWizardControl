using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dsoft.WizardControl.WPF
{
	/// <summary>
	/// Class WizardCont.
	/// Implements the <see cref="HeaderedContentControl" />
	/// </summary>
	/// <seealso cref="HeaderedContentControl" />
	/// <font color="red">Badly formed XML comment.</font>
	public class WizardCont : HeaderedContentControl
    {
		/// <summary>
		/// The title property
		/// </summary>
		public readonly static DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(WizardCont));

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

		/// <summary>
		/// Initializes static members of the <see cref="WizardCont"/> class.
		/// </summary>
		static WizardCont()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WizardCont), new FrameworkPropertyMetadata(typeof(WizardCont)));

            
        }
    }
}
