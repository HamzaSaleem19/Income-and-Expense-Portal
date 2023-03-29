using Income_and_Expense.Data;
using Income_and_Expense.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Income_and_Expense.Pages
{
    public partial class AddFriend
    {
        public List<SelectListItem> UserList = new();
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager { get; set; }

        [CascadingParameter]
        public ClaimsPrincipal User { get; set; }

        public string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);
        protected override async Task OnInitializedAsync()
        {
            UserList = await friendService.GetAllFriendsAsync();
        }
    }
}
