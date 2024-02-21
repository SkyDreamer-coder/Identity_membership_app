using Identity_membership.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Text;
using System.Xml.Serialization;

namespace Identity_membership.web.TagHelpers
{
    public class UserRoleNamesTagHelper:TagHelper
    {
        private readonly UserManager<UserApp> _userManager;

        public UserRoleNamesTagHelper(UserManager<UserApp> userManager)
        {
            _userManager = userManager;
        }

        public string UserId { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserId);

            var userRoles = await _userManager.GetRolesAsync(user!);

            var stringBuilder = new StringBuilder();

            userRoles.ToList().ForEach(x =>
            {
                stringBuilder.Append(@$"<span class='badge bg-secondary mx-1'>{x.ToLower()}</span>");
            });

            output.Content.SetHtmlContent(stringBuilder.ToString());
        }
    }
}
