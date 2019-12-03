using System;
using System.Collections.Generic;
using System.Linq;
using System.Mvvm;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppNetCore.TestData
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

        private bool _hidePage2;

        public bool HidePage2
        {
            get { return _hidePage2; }
            set { _hidePage2 = value; NotifyPropertyChanged(nameof(HidePage2)); }
        }


        public SharedViewModel()
        {
            Code = "Test Code";
            BankAccountName = "STUFF LTD";
            DatabaseName = "Database1";

            AddValidator(nameof(Code), "You must enter a code", () =>
            {
                return !string.IsNullOrWhiteSpace(Code);
            });

            //AddValidator(nameof(BankAccountName), "You must enter a bank account name", () =>
            //{
            //    return string.IsNullOrWhiteSpace(BankAccountName);
            //});
        }
    }
}
