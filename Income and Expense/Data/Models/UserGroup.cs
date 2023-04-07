using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Income_and_Expense.Data.Models
{
    public class UserGroup
    {
        [Key]
        public int UserGroup_Id { get; set; }
        public int Group_Id { get; set; }
        [ForeignKey("Group_Id")]
        public Groups Groups { get; set; }
        public string User_Id { get; set; }
        [ForeignKey("User_Id")]
        public ApplicationUser Application { get; set; }
        [NotMapped]
        public string UserName { get; set; }


    }
}
