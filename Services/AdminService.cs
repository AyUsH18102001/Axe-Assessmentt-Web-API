namespace AxeAssessmentToolWebAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly DataContext _dataContext;

        public AdminService(DataContext context)
        {
            _dataContext = context;
        }
        
        public async Task<bool> AddAdminProfile(int userId)
        {
            // find the user
            User? user = await _dataContext.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }
            // make admin
            user.isAdmin = true;
            await _dataContext.SaveChangesAsync();
            return true;
        }
    }
}
