using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shiny.Api.Push.Providers
{
    public static class Extensions
    {
        public static void AssertValid(this GoogleNotification notification)
        {
            
            //if (copy.Color != null && !Regex.Match(copy.Color, "^#[0-9a-fA-F]{6}$").Success)
            //{
            //    throw new ArgumentException("Color must be in the form #RRGGBB.");
            //}

            //if (copy.TitleLocArgs?.Any() == true && string.IsNullOrEmpty(copy.TitleLocKey))
            //{
            //    throw new ArgumentException("TitleLocKey is required when specifying TitleLocArgs.");
            //}

            //if (copy.BodyLocArgs?.Any() == true && string.IsNullOrEmpty(copy.BodyLocKey))
            //{
            //    throw new ArgumentException("BodyLocKey is required when specifying BodyLocArgs.");
            //}

            //if (copy.ImageUrl != null && !Uri.IsWellFormedUriString(copy.ImageUrl, UriKind.Absolute))
            //{
            //    throw new ArgumentException($"Malformed image URL string: {copy.ImageUrl}.");
            //}
        }
    }
}
