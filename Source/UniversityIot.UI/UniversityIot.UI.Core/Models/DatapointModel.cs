﻿using UniversityIot.UI.Core.MVVM;

namespace UniversityIot.UI.Core.Models
{
    public class DatapointModel : BaseModel
    {
        private long id;
        private string description;
        private string hexAddress;
        private object datapointValue;
        private bool isReadOnly;

        public long Id
        {
            get { return id; }
            set
            {
                if (value == id) return;
                id = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (value == description) return;
                description = value;
                OnPropertyChanged();
            }
        }

        public string HexAddress
        {
            get { return hexAddress; }
            set
            {
                if (value == hexAddress) return;
                hexAddress = value;
                OnPropertyChanged();
            }
        }

        public object DatapointValue
        {
            get { return datapointValue; }
            set
            {
                if (Equals(value, datapointValue)) return;
                datapointValue = value;
                OnPropertyChanged();
            }
        }

        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                if (value == isReadOnly) return;
                isReadOnly = value;
                OnPropertyChanged();
            }
        }
    }
}