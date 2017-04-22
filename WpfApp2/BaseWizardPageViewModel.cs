﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    public abstract class BaseWizardPageViewModel : BaseViewModel
    {
        #region Fields

        private String mTitle = String.Empty;
        private List<KeyValuePair<String, Object>> mParameters;

        #endregion

        #region Properties

        public List<KeyValuePair<String, Object>> Parameters
        {
            get { return mParameters; }
            set { mParameters = value; }
        }

        #endregion

        #region Constructors

        public BaseWizardPageViewModel()
        {

        }

        #endregion

        #region Methods

        //Validates the page before the Next button is pressed.
        public abstract bool Validate();

        /// <summary>
        /// Retrieves the value of a parameter based on its name.
        /// </summary>
        /// <param name="ParameterName">the parameter name</param>
        /// <returns></returns>
        public Object GetParameter(string ParameterName)
        {
            foreach (KeyValuePair<String, Object> Param in mParameters)
            {
                if (Param.Key == ParameterName)
                {
                    return Param.Value;
                }
            }

            //No Parameter found. Raise error.
            throw new Exception(String.Format("No parameter found for %1", ParameterName));
        }

        /// <summary>
        /// Sets the value of a paramter based on its name.
        /// </summary>
        /// <param name="ParameterName">the parameter name</param>
        /// <param name="ParameterValue">the parameter value</param>
        /// <returns></returns>
        public void SetParameter(string ParameterName, Object ParameterValue)
        {
            //Find the existing parameter
            foreach (KeyValuePair<String, Object> Param in mParameters)
            {
                if (Param.Key == ParameterName)
                {
                    //Remove the read only parameter
                    mParameters.Remove(Param);
                    break;
                }
            }

            //Add the new parameter.
            mParameters.Add(new KeyValuePair<string, object>(ParameterName, ParameterValue));
        }

        #endregion
    }
}
