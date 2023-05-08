using Income_and_Expense.Data;
using Income_and_Expense.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Income_and_Expense.Services
{
    public class GroupService
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailblazorService _emailblazorService;

        private readonly ApplicationDbContext groupDbContext;
        public GroupService(ApplicationDbContext applicationDbContext,EmailblazorService emailblazorService, UserManager<ApplicationUser> userManager)
        {
            groupDbContext = applicationDbContext;
            _userManager = userManager;
            _emailblazorService = emailblazorService;    

        }

        public async Task<bool> InsertGroup(Groups groups)
        {
            try
            {
                if (groups.Group_Id == 0)
                {
                    
                    await groupDbContext.Groupss.AddAsync(groups);
                    List<UserGroup> groupsList = new List<UserGroup>();
                    foreach (var item in groups.UserIds)
                    {
                        UserGroup ug = new();
                        ug.Groups = groups;
                        ug.User_Id = item;
                        var userEmail = GetEmail(item);
                        EmailDTO emailDto = new EmailDTO();
                        emailDto.To = userEmail.Result.ToString();
                        _emailblazorService.SendEmail(emailDto);
                        groupsList.Add(ug);
                    }
                    await groupDbContext.UserGroups.AddRangeAsync(groupsList);
                }
                else
                {
                    var group = groupDbContext.Groupss.Where(x => x.Group_Id == groups.Group_Id).FirstOrDefault();
                }
                await groupDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Groups>> GetAllGroups()
        {
            return await groupDbContext.Groupss.ToListAsync();
        }

        public async Task<Groups> GetGroups(int Id)
        {
            Groups group = await groupDbContext.Groupss.FirstOrDefaultAsync(x => x.Group_Id.Equals(Id));
            return group;
        }

        public async Task<bool> UpdateGroupsAsync(Groups groups)
        {
            try
            {
                await groupDbContext.Groupss.AddAsync(groups);
                await groupDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteGroupsAsync(int Groupid)
        {
            var checkgrouprecord = await groupDbContext.Groupss.Where(x => x.Group_Id == Groupid).FirstOrDefaultAsync();
            if (checkgrouprecord != null)
            {
                groupDbContext.Groupss.Remove(checkgrouprecord);
                await groupDbContext.SaveChangesAsync();

            }
            return true;
        }



        public async Task<List<SelectListItem>> GetAllUsers()
        {
            var userslist = await _userManager.Users.Select(x =>
                 new SelectListItem()
                 {
                     Value = x.Id,
                     Text = x.UserName
                 }).ToListAsync();
            return userslist;

        }
        public async Task<List<SelectListItem>> GetAllGroupsList()
        {
            var grouplist = await groupDbContext.Groupss.Select(x =>
                 new SelectListItem()
                 {
                     Value = x.Group_Id.ToString(),
                     Text = x.Group_Name
                 }).ToListAsync();
            return grouplist;
        }
        public async Task<List<UserGroup>> GetAllUserGroups()
        {
            var grouplist = await groupDbContext.UserGroups.ToListAsync();
            foreach (var item in grouplist)
            {
                item.UserName = await GetUserName(item.User_Id);
            }
            return grouplist;
        }
        public async Task<string> GetUserName(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var username = (user == null ? "" : user.FirstName + " " + user.LastName);
            return username;
        }
        public  async Task<string> GetEmail(string userId)
        {
            var user =  await _userManager.FindByIdAsync(userId);
            var userEmail = user.Email;
            return userEmail;

        }
    }
}