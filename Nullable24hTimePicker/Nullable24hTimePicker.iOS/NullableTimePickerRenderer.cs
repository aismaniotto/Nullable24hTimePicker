using Foundation;
using Nullable24hTimePicker;
using Nullable24hTimePicker.iOS;
using System;
using System.Collections.Generic;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NullableTimePicker), typeof(NullableTimePickerRenderer))]
namespace Nullable24hTimePicker.iOS
{
    public class NullableTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            var timePicker = (UIDatePicker)Control.InputView;
            timePicker.Locale = new NSLocale("no_nb");

            if (e.NewElement != null && this.Control != null)
            {
                this.UpdateDoneButton();
                this.AddClearButton();
                this.Control.BorderStyle = UITextBorderStyle.Line;
                Control.Layer.BorderColor = UIColor.LightGray.CGColor;
                Control.Layer.BorderWidth = 1;

                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    this.Control.Font = UIFont.SystemFontOfSize(25);
                }
            }

        }

        private void UpdateDoneButton()
        {
            var toolbar = (UIToolbar)Control.InputAccessoryView;
            var doneBtn = toolbar.Items[1];

            doneBtn.Clicked += (sender, args) =>
            {
                NullableTimePicker baseTimePicker = this.Element as NullableTimePicker;
                if (!baseTimePicker.NullableTime.HasValue)
                {
                    baseTimePicker.AssignValue();
                }
            };
        }

        private void AddClearButton()
        {
            var originalToolbar = this.Control.InputAccessoryView as UIToolbar;

            if (originalToolbar != null && originalToolbar.Items.Length <= 2)
            {
                var clearButton = new UIBarButtonItem("clear", UIBarButtonItemStyle.Plain, ((sender, ev) =>
                {
                    NullableTimePicker baseTimePicker = this.Element as NullableTimePicker;
                    this.Element.Unfocus();
                    this.Element.Time = DateTime.Now.TimeOfDay;
                    baseTimePicker.CleanTime();

                }));

                var newItems = new List<UIBarButtonItem>();
                foreach (var item in originalToolbar.Items)
                {
                    newItems.Add(item);
                }

                newItems.Insert(0, clearButton);

                originalToolbar.Items = newItems.ToArray();
                originalToolbar.SetNeedsDisplay();
            }
        }
    }
}