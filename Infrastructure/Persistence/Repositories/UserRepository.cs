namespace Infrastructure.Persistence.Repositories;

public class UserRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    : GenericRepository<ApplicationUser, string>(context), IUserRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<ApplicationUser> _userManager = userManager;


}
