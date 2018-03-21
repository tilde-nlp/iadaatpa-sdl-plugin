using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace iADAATPA.MTProvider.Helpers
{
    public static class DialogCloser
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                // For some reason only bool values (true or false) and not null get picked up by the dependency property.
                // This does not matter though since setting the DialogResult to null has no effect anyway.
                // After Window.Close() is called if the value pf DialogResult was null, it get's set to false
                typeof(bool?),
                typeof(DialogCloser),
                new PropertyMetadata(DialogResultChanged));

        private static void DialogResultChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
            {
                if (!window.IsLoaded)
                {
                    window.Loaded += (sender, eventArgs) =>
                    {
                        if (window != null)
                        {
                            window.DialogResult = e.NewValue as bool?;
                        }
                    };
                    return;
                }
                window.DialogResult = e.NewValue as bool?;
            }
        }
        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }
    }
}
