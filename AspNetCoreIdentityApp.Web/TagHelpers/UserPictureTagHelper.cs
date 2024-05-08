﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspNetCoreIdentityApp.Web.TagHelpers
{
    public class UserPictureTagHelper:TagHelper
    {
        public string PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if (string.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/userPictures/DefaultImage.png");
            }
            else

            {
                output.Attributes.SetAttribute("src", $"/userPictures/{PictureUrl}");
            }
        }
    }
}
