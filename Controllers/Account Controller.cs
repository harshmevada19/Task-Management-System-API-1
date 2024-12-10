using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Task_Management_System_API_1.Entity_Models;
using Task_Management_System_API_1.Services;

namespace Task_Management_System_API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ClaimsAuthorize]

    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Role Endpoints

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            var result = await _accountService.AddRoleAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role added successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var result = await _accountService.AssignRoleAsync(model.Username, model.Role);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole(string oldRoleName, string newRoleName)
        {
            if (string.IsNullOrWhiteSpace(oldRoleName) || string.IsNullOrWhiteSpace(newRoleName))
            {
                return BadRequest(new { message = "Both oldRoleName and newRoleName are required." });
            }

            var result = await _accountService.UpdateRoleAsync(oldRoleName, newRoleName);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role updated successfully." });
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete("delete-role/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest(new { message = "Role name is required." });
            }

            var result = await _accountService.DeleteRoleAsync(roleName);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role '{roleName}' deleted successfully." });
            }

            return BadRequest(result.Errors);
        }

        // Add Claims to a User

        [HttpPost("AddClaim")]
        public async Task<IActionResult> AddClaimToUser([FromBody] AddClaimRequest request)
        {
            var result = await _accountService.AddClaimToUserAsync(request.UserId, request.ClaimType, request.ClaimValue);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Claim added successfully." });
            }

            return BadRequest(new { Message = "Failed to add claim.", Errors = result.Errors });
        }

        [HttpGet("GetClaims")]
        public IActionResult GetUserClaims()
        {
            var claims = HttpContext.User.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });

            return Ok(new { Claims = claims });
        }

        [HttpGet("GetAllClaims/{userId}")]
        public async Task<IActionResult> GetAllClaims(string userId)
        {
            var claims = await _accountService.GetAllClaimsAsync(userId);
            if (claims == null)
            {
                return NotFound(new { Message = "User not found or no claims available." });   
            }

            return Ok(new { Claims = claims });
        }

        [HttpPut("UpdateClaim")]
        public async Task<IActionResult> UpdateUserClaim([FromBody] UpdateClaimRequest request)
        {
            var result = await _accountService.UpdateClaimAsync(request.UserId, request.OldClaimType, request.OldClaimValue, request.NewClaimType, request.NewClaimValue);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Claim updated successfully." });
            }

            return BadRequest(new { Message = "Failed to update claim.", Errors = result.Errors });
        }

        [HttpDelete("DeleteClaim")]
        public async Task<IActionResult> DeleteUserClaim([FromBody] DeleteClaimRequest request)
        {
            var result = await _accountService.DeleteClaimAsync(request.UserId, request.ClaimType, request.ClaimValue);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Claim deleted successfully." });
            }

            return BadRequest(new { Message = "Failed to delete claim.", Errors = result.Errors });
        }

        // Add a Claim to a Role

        [HttpPost("add-role-claim")]
        public async Task<IActionResult> AddClaimToRole([FromBody] RoleClaimModel model)
        {
            var result = await _accountService.AddClaimToRoleAsync(model.RoleId, model.ClaimType, model.ClaimValue);

            if (result)
            {
                return Ok($"Claim '{model.ClaimType}: {model.ClaimValue}' added to role with ID '{model.RoleId}'.");
            }

            return BadRequest($"Failed to add claim to role with ID '{model.RoleId}'.");
        }

        [HttpGet("get-all-role-claims/{roleId}")]
        public async Task<IActionResult> GetAllClaimsForRole(string roleId)
        {
            var claims = await _accountService.GetAllClaimsForRoleAsync(roleId);

            if (claims == null)
            {
                return NotFound($"Role with ID '{roleId}' not found.");
            }

            var response = claims.Select(c => new { Type = c.Type, Value = c.Value });
            return Ok(new { RoleId = roleId, Claims = response });
        }

        [HttpGet("get-role-claim/{roleId}/{claimType}")]
        public async Task<IActionResult> GetClaimByType(string roleId, string claimType)
        {
            var claims = await _accountService.GetClaimsByTypeAsync(roleId, claimType);

            if (claims == null || !claims.Any())
            {
                return NotFound($"No claims of type '{claimType}' found for role with ID '{roleId}'.");
            }

            var response = claims.Select(c => new { Type = c.Type, Value = c.Value });
            return Ok(new { RoleId = roleId, Claims = response });
        }

        [HttpPut("update-role-claim")]
        public async Task<IActionResult> UpdateRoleClaim([FromBody] UpdateRoleClaimModel model)
        {
            var result = await _accountService.UpdateRoleClaimAsync(model.RoleId, model.OldClaimType, model.OldClaimValue, model.NewClaimType, model.NewClaimValue);

            if (result)
            {
                return Ok($"Claim updated successfully for role with ID '{model.RoleId}'.");
            }

            return BadRequest($"Failed to update claim for role with ID '{model.RoleId}'.");
        }

        [HttpDelete("delete-role-claim")]
        public async Task<IActionResult> DeleteRoleClaim([FromBody] RoleClaimModel model)
        {
            var result = await _accountService.DeleteClaimFromRoleAsync(model.RoleId, model.ClaimType, model.ClaimValue);

            if (result)
            {
                return Ok($"Claim '{model.ClaimType}: {model.ClaimValue}' deleted from role with ID '{model.RoleId}'.");
            }

            return BadRequest($"Failed to delete claim from role with ID '{model.RoleId}'.");
        }
    }
}





       