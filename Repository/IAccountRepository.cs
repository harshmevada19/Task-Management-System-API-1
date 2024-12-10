﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Task_Management_System_API_1.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> AddRoleAsync(string role);
        Task<IdentityResult> AssignRoleAsync(string username, string role);
        Task<IdentityResult> UpdateRoleAsync(string oldRoleName, string newRoleName);
        Task<IdentityResult> DeleteRoleAsync(string roleName);


        // Add a Claim to a User

        Task<IdentityResult> AddClaimToUserAsync(string userId, string claimType, string claimValue);
        Task<IEnumerable<Claim>> GetAllClaimsAsync(string userId);
        Task<IdentityResult> UpdateClaimAsync(string userId, string oldClaimType, string oldClaimValue, string newClaimType, string newClaimValue);
        Task<IdentityResult> DeleteClaimAsync(string userId, string claimType, string claimValue);


        // Add a Claim to a Role

        Task<bool> AddClaimToRoleAsync(string roleId, string claimType, string claimValue);
        Task<IEnumerable<System.Security.Claims.Claim>> GetAllClaimsForRoleAsync(string roleId);
        Task<IEnumerable<System.Security.Claims.Claim>> GetClaimsByTypeAsync(string roleId, string claimType);
        Task<bool> UpdateRoleClaimAsync(string roleId, string oldClaimType, string oldClaimValue, string newClaimType, string newClaimValue);
        Task<bool> DeleteClaimFromRoleAsync(string roleId, string claimType, string claimValue);
    }
}