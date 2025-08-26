using PxlFund.Shared.Services.Interfaces;

namespace PxlFund.Shared.Services
{
    public class SeedDataRepository : ISeedDataRepository
    {
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly ApplicationDbContext _context;
        //public SeedDataRepository(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        //{
        //    _context = context;
        //    _userManager = userManager;
        //}
        public void Initialise()
        {
            //Data models
            //AddBankInfo();
            //AddFundInfo();
            //Identity models
            //AddRoles();
            //AddAdminUser();
        }
        //private void AddBankInfo()
        //{
        //    if (_context.Bank.Any())
        //    { }
        //}
        //private void AddFundInfo()
        //{
        //    if (_context.Bank.Any())
        //    { }
        //}
        //private void AddRoles()
        //{
        //    if (_context.Roles.Any())
        //    { }
        //}
        //private void AddAdminUser()
        //{
        //    if (_context.Users.Any())
        //    { }
        //}
    }
}
