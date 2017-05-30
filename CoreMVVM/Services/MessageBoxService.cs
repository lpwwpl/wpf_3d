using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CoreMVVM.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public MessageBoxResult Show
            (
            string messageBoxText, 
            string caption, 
            MessageBoxButton button
            )
        {
            return MessageBox.Show(messageBoxText, caption, button);
        }
    }
}
