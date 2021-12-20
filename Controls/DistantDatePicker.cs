using System;
using System.Windows;
using System.Windows.Controls;

namespace Glushkov89.DDPicker
{
    public class DistantDatePicker : DatePicker
    {
        private TextBox _dayBox;
        private TextBox _monthBox;
        private TextBox _yearBox;

        static DistantDatePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
               typeof(DistantDatePicker),
               new FrameworkPropertyMetadata(typeof(DistantDatePicker)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _dayBox = GetTemplateChild("dayInput") as TextBox;
            _monthBox = GetTemplateChild("monthInput") as TextBox;
            _yearBox = GetTemplateChild("yearInput") as TextBox;
            if (_dayBox != null)
            {
                _dayBox.TextChanged += new TextChangedEventHandler(Input_Changed);
            }
            if (_monthBox != null)
            {
                _monthBox.TextChanged += new TextChangedEventHandler(Input_Changed);
            }
            if (_yearBox != null)
            {
                _yearBox.TextChanged += new TextChangedEventHandler(Input_Changed);
            }
            base.SelectedDateChanged += DistantDatePicker_SelectedDateChanged;
        }

        private void DistantDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowDate();
        }

        private void Input_Changed(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(_dayBox.Text) |
                string.IsNullOrEmpty(_monthBox.Text) |
                string.IsNullOrEmpty(_yearBox.Text))
            {
                return;
            }
            else
            {
                try
                {
                    int year = int.Parse(_yearBox.Text);
                    int month = int.Parse(_monthBox.Text);
                    int day = int.Parse(_dayBox.Text);
                    DateTime NewDate = new DateTime(year, month, day);
                    SelectedDate = NewDate;
                }
                catch (Exception)
                {
                }
            }
        }

        private void ShowDate()
        {
            if (SelectedDate != null)
            {
                _dayBox.Text = SelectedDate.Value.Day.ToString();
                _monthBox.Text = SelectedDate.Value.Month.ToString();
                _yearBox.Text = SelectedDate.Value.Year.ToString();
            }
        }

        #region Dependecy properties

        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof(string), typeof(DistantDatePicker), new PropertyMetadata(""));

        public string YearTitle
        {
            get { return (string)GetValue(YearTitleProperty); }
            set { SetValue(YearTitleProperty, value); }
        }
        public static readonly DependencyProperty YearTitleProperty =
            DependencyProperty.Register("YearTitle", typeof(string), typeof(DistantDatePicker), new PropertyMetadata("Year"));

        public string MonthTitle
        {
            get { return (string)GetValue(MonthTitleProperty); }
            set { SetValue(MonthTitleProperty, value); }
        }
        public static readonly DependencyProperty MonthTitleProperty =
            DependencyProperty.Register("MonthTitle", typeof(string), typeof(DistantDatePicker), new PropertyMetadata("Month"));

        public string DayTitle
        {
            get { return (string)GetValue(DayTitleProperty); }
            set { SetValue(DayTitleProperty, value); }
        }
        public static readonly DependencyProperty DayTitleProperty =
            DependencyProperty.Register("DayTitle", typeof(string), typeof(DistantDatePicker), new PropertyMetadata("Day"));

        #endregion Dependecy properties
    }
}