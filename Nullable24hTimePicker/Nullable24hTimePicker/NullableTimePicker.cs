using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Nullable24hTimePicker
{
    public class NullableTimePicker : TimePicker
    {
        public NullableTimePicker()
        {
            Time = DateTime.Now.TimeOfDay;
            NullableTime = null;

            Format = @"HH\:mm";
        }
        public string _originalFormat = null;

        public static readonly BindableProperty PlaceHolderProperty =
            BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(NullableTimePicker), "  :  ");

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set
            {
                SetValue(PlaceHolderProperty, value);
            }
        }


        public static readonly BindableProperty NullableTimeProperty =
        BindableProperty.Create(nameof(NullableTime), typeof(TimeSpan?), typeof(NullableTimePicker), null, defaultBindingMode: BindingMode.TwoWay);

        public TimeSpan? NullableTime
        {
            get { return (TimeSpan?)GetValue(NullableTimeProperty); }
            set { SetValue(NullableTimeProperty, value); UpdateTime(); }
        }

        private void UpdateTime()
        {
            if (NullableTime != null)
            {
                if (_originalFormat != null)
                {
                    Format = _originalFormat;
                }
            }
            else
            {
                Format = PlaceHolder;
            }

        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                _originalFormat = Format;
                UpdateTime();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == TimeProperty.PropertyName ||
                (
                    propertyName == IsFocusedProperty.PropertyName &&
                    !IsFocused &&
                    (Time == DateTime.Now.TimeOfDay)))
            {
                AssignValue();
            }

            if (propertyName == NullableTimeProperty.PropertyName && NullableTime.HasValue)
            {
                Time = NullableTime.Value;
                if (Time == DateTime.Now.TimeOfDay)
                {
                    //this code was done because when date selected is the actual date the"DateProperty" does not raise  
                    UpdateTime();
                }
            }
        }

        public void CleanTime()
        {
            NullableTime = null;
            UpdateTime();
        }
        public void AssignValue()
        {
            NullableTime = Time;
            UpdateTime();

        }
    }
}
