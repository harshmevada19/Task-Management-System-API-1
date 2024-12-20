using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Task_Management_System_API_1.Controllers;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Repositories;

namespace Task_Management_System_API_1.Repository
{
    public class AccountRepository : GenericRepository<Task>, IAccountRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountRepository(ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager) : base(applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IdentityResult> AddRoleAsync(string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            await _applicationDbContext.SaveChangesAsync();
            return result;
        }

        public async Task<IdentityResult> AssignRoleAsync(string username, string role)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            await _applicationDbContext.SaveChangesAsync();
            return result;
        }
        public async Task<IdentityResult> UpdateRoleAsync(string oldRoleName, string newRoleName)
        {
            var role = await _roleManager.FindByNameAsync(oldRoleName);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{oldRoleName}' not found." });
            }

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);
            await _applicationDbContext.SaveChangesAsync();
            return result;
        }
        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role '{roleName}' not found." });
            }

            var result = await _roleManager.DeleteAsync(role);
            await _applicationDbContext.SaveChangesAsync();
            return result;
        }


        // Add Claims to a User

        public async Task<IdentityResult> AddClaimToUserAsync(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var claim = new System.Security.Claims.Claim(claimType, claimValue);
            return await _userManager.AddClaimAsync(user, claim);
        }

        public async Task<IEnumerable<System.Security.Claims.Claim>> GetAllClaimsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return await _userManager.GetClaimsAsync(user);
        }

        public async Task<IdentityResult> UpdateClaimAsync(string userId, string oldClaimType, string oldClaimValue, string newClaimType, string newClaimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var oldClaim = new System.Security.Claims.Claim(oldClaimType, oldClaimValue);
            var newClaim = new System.Security.Claims.Claim(newClaimType, newClaimValue);

            var result = await _userManager.RemoveClaimAsync(user, oldClaim);
            if (!result.Succeeded) return result;

            return await _userManager.AddClaimAsync(user, newClaim);
        }
        public async Task<IdentityResult> DeleteClaimAsync(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found." });

            var claim = new System.Security.Claims.Claim(claimType, claimValue);
            return await _userManager.RemoveClaimAsync(user, claim);
        }

        // Add a Claim to a Role

        public async Task<bool> AddClaimToRoleAsync(string roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var claim = new System.Security.Claims.Claim(claimType, claimValue);
            var result = await _roleManager.AddClaimAsync(role, claim);

            return result.Succeeded;
        }

        public async Task<IEnumerable<System.Security.Claims.Claim>> GetAllClaimsForRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return null;

            return await _roleManager.GetClaimsAsync(role);
        }

        public async Task<IEnumerable<System.Security.Claims.Claim>> GetClaimsByTypeAsync(string roleId, string claimType)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return null;

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims.Where(c => c.Type == claimType);
        }
        public async Task<bool> UpdateRoleClaimAsync(string roleId, string oldClaimType, string oldClaimValue, string newClaimType, string newClaimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var claims = await _roleManager.GetClaimsAsync(role);
            var claimToUpdate = claims.FirstOrDefault(c => c.Type == oldClaimType && c.Value == oldClaimValue);

            if (claimToUpdate == null) return false;

            // Remove old claim
            var removeResult = await _roleManager.RemoveClaimAsync(role, claimToUpdate);
            if (!removeResult.Succeeded) return false;

            // Add new claim
            var newClaim = new System.Security.Claims.Claim(newClaimType, newClaimValue);
            var addResult = await _roleManager.AddClaimAsync(role, newClaim);

            return addResult.Succeeded;
        }
        public async Task<bool> DeleteClaimFromRoleAsync(string roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var claims = await _roleManager.GetClaimsAsync(role);
            var claimToDelete = claims.FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);

            if (claimToDelete == null) return false;

            var result = await _roleManager.RemoveClaimAsync(role, claimToDelete);
            return result.Succeeded;
        }
    }
}

      