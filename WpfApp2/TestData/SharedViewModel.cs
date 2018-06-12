﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.TestData
{
    public class SharedViewModel : ViewModel
    {
        private string _code;
        private string _bankAccountName;
        private string datbaseName;

        public string Code
        {
            get { return _code; }
            set { _code = value; NotifyPropertyChanged(nameof(Code)); ValidateProperty(); }
        }

        public string BankAccountName
        {
            get { return _bankAccountName; }
            set { _bankAccountName = value; NotifyPropertyChanged(nameof(BankAccountName)); ValidateProperty(); }
        }

        public string DatabaseName
        {
            get { return datbaseName; }
            set { datbaseName = value; NotifyPropertyChanged(nameof(DatabaseName)); ValidateProperty(); }
        }

        public SharedViewModel()
        {
            Code = "Test Code";
            BankAccountName = "STUFF LTD";
            DatabaseName = "Database1";

            AddValidator(nameof(Code), () =>
            {
                if (string.IsNullOrWhiteSpace(Code))
                    return "You must enter a code";

                return null;
            });

            AddValidator(nameof(BankAccountName), () =>
            {
                if (string.IsNullOrWhiteSpace(BankAccountName))
                    return "You must enter a bank account name";

                return null;
            });
        }
    }
}