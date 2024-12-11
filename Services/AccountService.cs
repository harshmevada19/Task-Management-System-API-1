using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Task_Management_System_API_1.Repositories;
using Task_Management_System_API_1.Repository;

namespace Task_Management_System_API_1.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IdentityResult> AddRoleAsync(string role)
        {
            try
            {
                return await _accountRepository.AddRoleAsync(role);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> AssignRoleAsync(string username, string role)
        {
            try
            {
                return await _accountRepository.AssignRoleAsync(username, role);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> UpdateRoleAsync(string oldRoleName, string newRoleName)
        {
            try
            {
                return await _accountRepository.UpdateRoleAsync(oldRoleName, newRoleName);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            try
            {
                return await _accountRepository.DeleteRoleAsync(roleName);
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"An error occurred: {ex.Message}" });
            }
        }


        // Add Claims to a User

        public async Task<IdentityResult> AddClaimToUserAsync(string userId, string claimType, string claimValue)
        {
            try
            {
                return await _accountRepository.AddClaimToUserAsync(userId, claimType, claimValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddClaimToUserAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<System.Security.Claims.Claim>> GetAllClaimsAsync(string userId)
        {
            try
            {
                return await _accountRepository.GetAllClaimsAsync(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllClaimsAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IdentityResult> UpdateClaimAsync(string userId, string oldClaimType, string oldClaimValue, string newClaimType, string newClaimValue)
        {
            try
            {
                return await _accountRepository.UpdateClaimAsync(userId, oldClaimType, oldClaimValue, newClaimType, newClaimValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateClaimAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<IdentityResult> DeleteClaimAsync(string userId, string claimType, string claimValue)
        {
            try
            {
                return await _accountRepository.DeleteClaimAsync(userId, claimType, claimValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteClaimAsync: {ex.Message}");
                throw;
            }
        }

        // Add a Claim to a Role

        public async Task<bool> AddClaimToRoleAsync(string roleId, string claimType, string claimValue)
        {
            try
            {
                var result = await _accountRepository.AddClaimToRoleAsync(roleId, claimType, claimValue);
                return result;
            }
            catch (Exception ex)
            {
                return false; 
            }
        }

        public async Task<IEnumerable<Claim>> GetAllClaimsForRoleAsync(string roleId)
        {
            try
            {
                return await _accountRepository.GetAllClaimsForRoleAsync(roleId);
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        public async Task<IEnumerable<Claim>> GetClaimsByTypeAsync(string roleId, string claimType)
        {
            try
            {
                return await _accountRepository.GetClaimsByTypeAsync(roleId, claimType);
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        public async Task<bool> UpdateRoleClaimAsync(string roleId, string oldClaimType, string oldClaimValue, string newClaimType, string newClaimValue)
        {
            try
            {
                return await _accountRepository.UpdateRoleClaimAsync(
                    roleId, oldClaimType, oldClaimValue, newClaimType, newClaimValue);
            }
            catch (Exception ex)
            {
                return false; 
            }
        }

        public async Task<bool> DeleteClaimFromRoleAsync(string roleId, string claimType, string claimValue)
        {
            try
            {
                return await _accountRepository.DeleteClaimFromRoleAsync(roleId, claimType, claimValue);
            }
            catch (Exception ex)
            {
                return false; 
            }
        }
    }
}
