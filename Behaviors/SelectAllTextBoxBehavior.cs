using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Glushkov89.DDPicker
{
    /// <summary>
    /// This behavior/attached property causes all of the text in the textbox to be automatically selected when the user tabs into the control.
    /// Optionally, it also selects the text when the user clicks on it with the mouse.
    /// </summary>
    /// <remarks>
    ///
    /// Attached Propety Syntax:
    ///
    /// theme:SelectAllTextBoxBehavior.IsEnabled="True"
    /// theme:SelectAllTextBoxBehavior.SelectOnMouseClick="True"
    ///
    /// Behaviors syntax
    ///
    /// &lt;i:Interaction.Behaviors&gt;
    ///     &lt;s:SelectAllTextBoxBehavior OnMouseClick = "True" /&gt;
    /// &lt;/ i:Interaction.Behaviors&gt;
    ///
    /// From https://stackoverflow.com/questions/4161531/automatically-select-all-text-on-focus-in-wpf-textbox
    ///
    /// </remarks>
    ///

    public class SelectAllTextBoxBehavior : Behavior<TextBox>
    {
        private bool m_AlreadyFocused;
        public bool OnMouseClick { get; set; }

        /// <summary>
        /// Called after the behavior is attached to an AssociatedObject.
        /// </summary>
        /// <remarks>Override this to hook up functionality to the AssociatedObject.</remarks>
        protected override void OnAttached()
        {
            if (!DesignerProperties.GetIsInDesignMode(AssociatedObject))
            {
                AssociatedObject.GotFocus += Target_GotFocus;
                AssociatedObject.PreviewMouseUp += Target_MouseUp; //Need PreviewMouseUp or it can be swallowed by a child control
                AssociatedObject.LostFocus += Target_LostFocus;
            }
            base.OnAttached();
        }

        /// <summary>
        /// Called when the behavior is being detached from its AssociatedObject, but before it has actually occurred.
        /// </summary>
        /// <remarks>Override this to unhook functionality from the AssociatedObject.</remarks>
        protected override void OnDetaching()
        {
            if (!DesignerProperties.GetIsInDesignMode(AssociatedObject))
            {
                AssociatedObject.GotFocus -= Target_GotFocus;
                AssociatedObject.PreviewMouseUp -= Target_MouseUp;
                AssociatedObject.LostFocus -= Target_LostFocus;
            }
            base.OnDetaching();
        }

        private void Target_GotFocus(object sender, RoutedEventArgs e)
        {
            //Debug.WriteLine("LeftButton: " + Mouse.LeftButton);
            //Debug.WriteLine("MiddleButton: " + Mouse.MiddleButton);
            //Debug.WriteLine("RightButton: " + Mouse.RightButton);
            //Debug.WriteLine("XButton1: " + Mouse.XButton1);
            //Debug.WriteLine("XButton2: " + Mouse.XButton2);
            if (Mouse.LeftButton == MouseButtonState.Released & Mouse.MiddleButton == MouseButtonState.Released & Mouse.RightButton == MouseButtonState.Released & Mouse.XButton1 == MouseButtonState.Released & Mouse.XButton2 == MouseButtonState.Released)
            {
                AssociatedObject.SelectAll();
                m_AlreadyFocused = true;
                //Debug.WriteLine("GotFocus --> Select All");
            }
            else
            {
                //Debug.WriteLine("GotFocus " + m_AlreadyFocused);
            }
        }

        private void Target_LostFocus(object sender, RoutedEventArgs e)
        {
            m_AlreadyFocused = false;
            //Debug.WriteLine("LostFocus " + m_AlreadyFocused);
        }

        private void Target_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!m_AlreadyFocused & AssociatedObject.SelectionLength == 0 && OnMouseClick)
            {
                m_AlreadyFocused = true;
                AssociatedObject.SelectAll();
                //Debug.WriteLine("MouseUp --> Select All");
            }
            else
            {
                //Debug.WriteLine("MouseUp " + m_AlreadyFocused + " OnMouse: " + OnMouseClick);
            }
        }

        #region "Boilerplate for XAML Attached Properties"

        public static DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SelectAllTextBoxBehavior), new FrameworkPropertyMetadata(false, OnIsEnabledChanged));

        public static DependencyProperty SelectOnMouseClickProperty = DependencyProperty.RegisterAttached("SelectOnMouseClick", typeof(bool), typeof(SelectAllTextBoxBehavior), new FrameworkPropertyMetadata(false, OnSelectOnMouseClickChanged));

        public static bool GetIsEnabled(DependencyObject uie)
        {
            return (bool)(uie.GetValue(IsEnabledProperty));
        }

        public static bool GetSelectOnMouseClick(DependencyObject uie)
        {
            return (bool)(uie.GetValue(SelectOnMouseClickProperty));
        }

        public static void SetIsEnabled(DependencyObject uie, bool value)
        {
            uie.SetValue(IsEnabledProperty, value);
        }
        public static void SetSelectOnMouseClick(DependencyObject uie, bool value)
        {
            uie.SetValue(SelectOnMouseClickProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as UIElement;
            if (uie == null)
                return;

            var behaviors = Interaction.GetBehaviors(uie);
            var existingBehavior = behaviors.FirstOrDefault(b => b.GetType() == typeof(SelectAllTextBoxBehavior)) as SelectAllTextBoxBehavior;

            if ((bool)e.NewValue == false && existingBehavior != null)
            {
                behaviors.Remove(existingBehavior);
            }
            else if ((bool)e.NewValue == true && existingBehavior == null)
            {
                behaviors.Add(new SelectAllTextBoxBehavior() { OnMouseClick = GetSelectOnMouseClick(dpo) });
            }
        }

        private static void OnSelectOnMouseClickChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as UIElement;
            if (uie == null)
                return;

            var behaviors = Interaction.GetBehaviors(uie);
            var existingBehavior = behaviors.FirstOrDefault(b => b.GetType() == typeof(SelectAllTextBoxBehavior)) as SelectAllTextBoxBehavior;

            if (existingBehavior != null)
            {
                existingBehavior.OnMouseClick = (bool)e.NewValue;
            }
            else if ((bool)e.NewValue == true && existingBehavior == null)
            {
                SetIsEnabled(dpo, true);
                behaviors.Add(new SelectAllTextBoxBehavior() { OnMouseClick = true });
            }
        }

        #endregion "Boilerplate for XAML Attached Properties"
    }
}