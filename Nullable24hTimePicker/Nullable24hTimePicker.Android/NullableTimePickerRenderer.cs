using Android.App;
using Android.Content;
using Android.Widget;
using Nullable24hTimePicker;
using Nullable24hTimePicker.Droid;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NullableTimePicker), typeof(NullableTimePickerRenderer))]
namespace Nullable24hTimePicker.Droid
{
    public class NullableTimePickerRenderer : ViewRenderer<NullableTimePicker, EditText>
    {
        public NullableTimePickerRenderer(Context context) : base(context)
        {
        }

        TimePickerDialog _dialog;
        protected override void OnElementChanged(ElementChangedEventArgs<NullableTimePicker> e)
        {
            base.OnElementChanged(e);

            this.SetNativeControl(new Android.Widget.EditText(Context));
            if (Control == null || e.NewElement == null)
                return;

            this.Control.Click += OnPickerClick;

            if (Element.NullableTime.HasValue)
                Control.Text = DateTime.Today.Add(Element.Time).ToString(Element.Format);
            else
                this.Control.Text = Element.PlaceHolder;

            this.Control.KeyListener = null;
            this.Control.FocusChange += OnPickerFocusChange;
            this.Control.Enabled = Element.IsEnabled;

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Xamarin.Forms.TimePicker.TimeProperty.PropertyName ||
                e.PropertyName == Xamarin.Forms.TimePicker.FormatProperty.PropertyName)
                SetTime(Element.Time);
        }

        void OnPickerFocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
            {
                ShowTimePicker();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (Control != null)
            {
                this.Control.Click -= OnPickerClick;
                this.Control.FocusChange -= OnPickerFocusChange;

                if (_dialog != null)
                {
                    _dialog.Hide();
                    _dialog.Dispose();
                    _dialog = null;
                }
            }

            base.Dispose(disposing);
        }

        void OnPickerClick(object sender, EventArgs e)
        {
            ShowTimePicker();
        }

        void SetTime(TimeSpan time)
        {
            Control.Text = DateTime.Today.Add(time).ToString(Element.Format);
            Element.Time = time;
        }

        private void ShowTimePicker()
        {
            CreateTimePickerDialog(this.Element.Time.Hours, this.Element.Time.Minutes);
            _dialog.Show();
        }

        void CreateTimePickerDialog(int hours, int minutes)
        {
            NullableTimePicker view = Element;
            _dialog = new TimePickerDialog(Context, (o, e) =>
            {
                view.Time = new TimeSpan(hours: e.HourOfDay, minutes: e.Minute, seconds: 0);
                view.AssignValue();
                ((IElementController)view).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                Control.ClearFocus();

                _dialog = null;
            }, hours, minutes, true);

            _dialog.SetButton2("clear", (sender, e) =>
            {
                this.Element.CleanTime();
                Control.Text = this.Element.Format;
            });
        }
    }
}