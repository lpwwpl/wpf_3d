using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CoreMVVM.Services
{
    public interface IMessageBoxService
    {
        MessageBoxResult Show
            (
            string messageBoxText,
            string caption,
            MessageBoxButton button
            );
    }
}
