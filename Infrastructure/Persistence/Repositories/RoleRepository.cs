﻿namespace Infrastructure.Persistence.Repositories;

public class RoleRepository(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager) : IRoleRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    #region Read Operations

    public async Task<Role?> GetAsync(string id, CancellationToken cancellationToken = default)
        => await _context.Roles.ProjectToType<Role>().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<IEnumerable<Role>> GetAllAsync(bool includeDefault = false, bool includeDisabled = false, CancellationToken cancellationToken = default)
        => await _context.Roles
            .AsNoTracking()
            .Where(r => (includeDefault || !r.IsDefault) && (includeDisabled || !r.IsDisabled))
            .ProjectToType<Role>()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<string>> GetAllNamesAsync(bool includeDefault = false, bool includeDisabled = false, CancellationToken cancellationToken = default)
        => await _context.Roles
            .AsNoTracking()
            .Where(r => (includeDefault || !r.IsDefault) && (includeDisabled || !r.IsDisabled))
            .Select(r => r.Name!)
            .Distinct()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<string>> GetClaimsAsync(string roleId, CancellationToken cancellationToken = default)
        => await _context.RoleClaims
            .AsNoTracking()
            .Where(rc => rc.RoleId == roleId)
            .Select(rc => rc.ClaimValue!)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<string>> GetClaimsAsync(string roleId, string claimType, CancellationToken cancellationToken = default)
        => await _context.RoleClaims
            .AsNoTracking()
            .Where(rc => rc.RoleId == roleId && rc.ClaimType == claimType)
            .Select(rc => rc.ClaimValue!)
            .ToListAsync(cancellationToken);

    #endregion

    #region Check Operations

    public async Task<bool> NameExistsAsync(string roleName, CancellationToken cancellationToken = default)
    {
        roleName = _roleManager.NormalizeKey(roleName);

        return await _context.Roles
            .AsNoTracking()
            .AnyAsync(r => r.NormalizedName == roleName, cancellationToken);
    }

    public async Task<bool> NameExistsAsync(string roleName, string roleId, CancellationToken cancellationToken = default)
    {
        roleName = _roleManager.NormalizeKey(roleName);

        return await _context.Roles
            .AsNoTracking()
            .AnyAsync(r => r.NormalizedName == roleName && r.Id != roleId, cancellationToken);
    }

    #endregion

    #region Modify Operations

    public async Task<Result> CreateAsync(Role role)
    {
        var applicationRole = role.CreateIdentity();

        var result = await _roleManager.CreateAsync(applicationRole);

        if (result.Succeeded)
            role.Id = applicationRole.Id;

        return result.ToDomain();
    }

    public async Task<Result> UpdateAsync(Role role)
    {
        var applicationRole = await GetByIdOrThrowAsync(role.Id);

        applicationRole = role.Adapt(applicationRole);

        var result = await _roleManager.UpdateAsync(applicationRole);

        return result.ToDomain();
    }

    public async Task AddClaimAsync(string roleId, string claimType, string claimValue, CancellationToken cancellationToken = default)
    {
        var claim = new IdentityRoleClaim<string>
        {
            RoleId = roleId,
            ClaimType = claimType,
            ClaimValue = claimValue
        };

        await _context.RoleClaims.AddAsync(claim, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddClaimsAsync(string roleId, string claimType, IEnumerable<string> claimValue, CancellationToken cancellationToken = default)
    {
        var claims = claimValue.Select(value => new IdentityRoleClaim<string>
        {
            RoleId = roleId,
            ClaimType = claimType,
            ClaimValue = value
        });

        await _context.RoleClaims.AddRangeAsync(claims, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteClaimsAsync(string roleId, CancellationToken cancellationToken = default)
        => await _context.RoleClaims
            .Where(rc => rc.RoleId == roleId)
            .ExecuteDeleteAsync(cancellationToken);

    public async Task DeleteClaimsAsync(string roleId, IEnumerable<string> claimValue, CancellationToken cancellationToken = default)
        => await _context.RoleClaims
            .Where(rc => rc.RoleId == roleId && claimValue.Contains(rc.ClaimValue))
            .ExecuteDeleteAsync(cancellationToken);

    public async Task ToggleStatusAsync(Role role, CancellationToken cancellationToken = default)
        => await _context.Roles
            .Where(r => r.Id == role.Id)
            .ExecuteUpdateAsync
            (
                r => r.SetProperty(x => x.IsDefault, x => !x.IsDefault)
                , cancellationToken
            );

    #endregion

    #region Private Methods

    private async Task<ApplicationRole> GetByIdOrThrowAsync(string roleId)
    {
        var role = await _roleManager.FindByIdAsync(roleId);

        return role is not null
            ? role
            : throw new InvalidOperationException(string.Format("Role with ID '{0}' not found. This indicates a bug in the service layer validation.", roleId));
    }

    #endregion
}
