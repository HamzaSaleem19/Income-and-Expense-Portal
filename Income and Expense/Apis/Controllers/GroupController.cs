﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Income_and_Expense.Data;
using Microsoft.AspNetCore.Identity;
using Income_and_Expense.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Income_and_Expense.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly ApplicationDbContext groupDbContext;
        public GroupController(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager)
        {

            groupDbContext = applicationDbContext;
            this.userManager = userManager;
        }

        // GET api/<GroupController>/5
        [HttpGet]
        [Route("GetAllUserGroups")]
        public async Task<List<UserGroup>> GetAllUserGroups()
        {
            var grouplist = await groupDbContext.UserGroups.ToListAsync();
            foreach (var item in grouplist)
            {
                item.UserName = await GetUserName(item.UserName);
            }
            return grouplist;
        }

        // GET api/<GroupController>/5
        [HttpGet]
        [Route("GetUserName")]
        public async Task<String> GetUserName(String UserId)
        {
            var user = await userManager.FindByIdAsync(UserId);
            var username = (user == null ? "" : user.FirstName + " " + user.LastName);
            return username;
        }


        // GET api/<GroupController>/5
        [HttpGet("{id}")]
        [Route("GetGroups")]

        public async Task<Groups> GetGroups(int Id)
        {
            Groups group = await groupDbContext.Groupss.FirstOrDefaultAsync(x => x.Group_Id.Equals(Id));
            return group;
        }

        // GET: api/<GroupController>
        [HttpGet]
        [Route("GetAllUsers")]

        public async Task<List<SelectListItem>> GetAllUsers()
        {
            var userlist = await userManager.Users.Select(x =>
            new SelectListItem()
            {
                Value = x.Id,
                Text = x.UserName
            }).ToListAsync();
            return userlist;
        }


        // POST api/<GroupController>
        [HttpPost]
        [Route("InsertGroup")]

        public async Task<bool> InsertGroup(Groups groups)
        {
            try
            {
                if (groups.Group_Id == 0)
                {
                    await groupDbContext.Groupss.AddAsync(groups);
                    List<UserGroup> groupslist = new List<UserGroup>();
                    foreach (var item in groups.UserIds)
                    {
                        UserGroup ug = new();
                        ug.Groups = groups;
                        ug.User_Id = item;
                        groupslist.Add(ug);
                    }
                    await groupDbContext.UserGroups.AddRangeAsync(groupslist);
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


        // PUT api/<GroupController>/5
        [HttpPut("{id}")]
        [Route("UpdateGroups")]


        public async Task<bool> UpdateGroups(Groups groups)
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

        // DELETE api/<GroupController>/5
        [HttpDelete("{id}")]
        [Route("DeleteGroupsAsync")]

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

        [HttpGet]
        [Route("TestAPI")]
        public async Task<IActionResult> TestAPI() //For Apis testing
        {
            try
            {
                return Ok("Apis working fine");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }
    }
}
