﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Identity_membership.web.TagHelpers
{
    public class UserPictureThumbnailTagHelper:TagHelper
    {

        public string? PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if(String.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/userpictures/Default_picture.png");
            }
            else 
            {
                output.Attributes.SetAttribute("src", $"/userpictures/{PictureUrl}");
            }
        }

    }
}
