using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class NotificationControlViewModel : BaseViewModel
    {
        /// <summary>
        /// The message to display
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// The background color of the Box in ARGB value
        /// </summary>
        public string? PrimaryBackground { get; set; }

        /// <summary>
        /// The Line color of the Box in ARGB value
        /// </summary>
        public string? SecendaryBackground { get; set; }

        /// <summary>
        /// determines the type of the notification
        /// </summary>
        public NotificationType NotificationType { get; set; }

        /// <summary>
        /// The Icon type of the notification
        /// </summary>
        public IconType IconType { get; set; }

        public NotificationControlViewModel(NotificationType notificationType, string message)
        {
            SetType(notificationType);
            Message = message;
        }

        public void SetType(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.Info:
                    IconType = IconType.Info;
                    PrimaryBackground = "E0EEFB";
                    SecendaryBackground = "62ACED";
                    
                    break;
                case NotificationType.Warning:
                    IconType = IconType.Warning;
                    PrimaryBackground = "FFF1DB";
                    SecendaryBackground = "FFB74D";
                    

                    break;
                case NotificationType.Error:
                    IconType = IconType.Error;
                    PrimaryBackground = "F6CFD3";
                    SecendaryBackground = "E62737";
                    

                    break;
                case NotificationType.Success:
                    IconType = IconType.Succes;
                    PrimaryBackground = "E6F4E7";
                    SecendaryBackground = "82C785";
                    
                    break;
                default:
                    break;
            }
        }

    }

    public enum NotificationType
    {
        Info,
        Warning,
        Error,
        Success
    }
}
