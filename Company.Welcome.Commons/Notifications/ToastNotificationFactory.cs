using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace Company.Welcome.Commons.Notifications
{
    public class ToastNotificationFactory : IToastNotificationFactory
    {
        public ToastNotification CreateNotificationFor(List<string> texts, List<string> images, ToastTemplateType toastTemplateType)
        {
            var notificationXml = ToastNotificationManager.GetTemplateContent(toastTemplateType);

            var textElements = notificationXml.GetElementsByTagName("text");
            for (var i = 0; i < textElements.Length; i++)
            {
                textElements[i].AppendChild(notificationXml.CreateTextNode(texts[i]));
            }
            if (images != null)
            {
                var imageElements = notificationXml.GetElementsByTagName("image");
                for (var i = 0; i < textElements.Length; i++)
                {
                    imageElements[i].Attributes[1].NodeValue = images[i];
                }
            }

            return new ToastNotification(notificationXml);
        }
    }

    public interface IToastNotificationFactory
    {
        ToastNotification CreateNotificationFor(List<string> texts, List<string> images,
            ToastTemplateType toastTemplateType);
    }
}
