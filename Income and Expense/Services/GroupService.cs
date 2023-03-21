using Income_and_Expense.Data;

namespace Income_and_Expense.Services
{
    public class GroupService
    {
        private readonly ApplicationDbContext context;
        public GroupService(ApplicationDbContext _context)
        {
            _context = context;
        }

    }
}
