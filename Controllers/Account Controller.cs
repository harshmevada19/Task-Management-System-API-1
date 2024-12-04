using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_Management_System_API_1.Data;
using Task_Management_System_API_1.Entity_Models;

namespace Task_Management_System_API_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _applicationDbContext;


        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        //Role
        [Authorize]
        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if (await _roleManager.RoleExistsAsync(role))
            {
                return BadRequest("Role already exists");
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(role));
            if (result.Succeeded)
            {
                return Ok(new { message = "Role added successfully" });
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRole model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);
            if (result.Succeeded)
            {
                return Ok(new { message = "Role assigned successfully" });
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpPut("update-role")]
        public async Task<IActionResult> UpdateRole(string oldRoleName, string newRoleName)
        {
            if (string.IsNullOrWhiteSpace(oldRoleName) || string.IsNullOrWhiteSpace(newRoleName))
            {
                return BadRequest(new { message = "Both oldRoleName and newRoleName are required." });
            }

            var role = await _roleManager.FindByNameAsync(oldRoleName);
            if (role == null)
            {
                return NotFound(new { message = $"Role '{oldRoleName}' not found." });
            }

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Role updated successfully." });
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpDelete("delete-role/{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest(new { message = "Role name is required." });
            }

            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound(new { message = $"Role '{roleName}' not found." });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { message = $"Role '{roleName}' deleted successfully." });
            }

            return BadRequest(result.Errors);
        }




        // Add Claims to a User
        [Authorize]
        [HttpPost("AddClaim")]
        public async Task<IActionResult> AddClaimToUser([FromBody] AddClaimRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var claims = new[]
            {
                new Claim(request.ClaimType, request.ClaimValue)
            };

            var result = await _userManager.AddClaimsAsync(user, claims);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Claims added successfully." });
            }

            return BadRequest(new { Message = "Failed to add claims.", Errors = result.Errors });
        }

        [Authorize]
        [HttpGet("GetClaims")]
        public IActionResult GetUserClaims()
        {
            var user = HttpContext.User;

            if (user?.Identity?.IsAuthenticated != true)
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }

            var claims = user.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });

            return Ok(new { Claims = claims });
        }

        [Authorize]
        [HttpGet("GetAllClaims/{userId}")]
        public async Task<IActionResult> GetAllClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var response = claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });

            return Ok(new { Claims = response });
        }

        [Authorize]
        [HttpPut("UpdateClaim")]
        public async Task<IActionResult> UpdateUserClaim([FromBody] UpdateClaimRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var existingClaims = await _userManager.GetClaimsAsync(user);
            var oldClaim = existingClaims.FirstOrDefault(c => c.Type == request.OldClaimType && c.Value == request.OldClaimValue);

            if (oldClaim == null)
            {
                return NotFound(new { Message = "Claim not found." });
            }

            var removeResult = await _userManager.RemoveClaimAsync(user, oldClaim);
            if (!removeResult.Succeeded)
            {
                return BadRequest(new { Message = "Failed to remove the old claim.", Errors = removeResult.Errors });
            }

            var addResult = await _userManager.AddClaimAsync(user, new Claim(request.NewClaimType, request.NewClaimValue));
            if (addResult.Succeeded)
            {
                return Ok(new { Message = "Claim updated successfully." });
            }

            return BadRequest(new { Message = "Failed to update claim.", Errors = addResult.Errors });
        }

        [Authorize]
        [HttpDelete("DeleteClaim")]
        public async Task<IActionResult> DeleteUserClaim([FromBody] DeleteClaimRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var existingClaims = await _userManager.GetClaimsAsync(user);
            var claimToDelete = existingClaims.FirstOrDefault(c => c.Type == request.ClaimType && c.Value == request.ClaimValue);

            if (claimToDelete == null)
            {
                return NotFound(new { Message = "Claim not found." });
            }

            var result = await _userManager.RemoveClaimAsync(user, claimToDelete);

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
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound($"Role with ID '{model.RoleId}' not found.");
            }

            var claim = new Claim(model.ClaimType, model.ClaimValue);
            var result = await _roleManager.AddClaimAsync(role, claim);

            if (result.Succeeded)
            {
                return Ok($"Claim '{model.ClaimType}: {model.ClaimValue}' added to role with ID '{model.RoleId}'.");
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("get-all-role-claims/{roleId}")]
        public async Task<IActionResult> GetAllClaimsForRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound($"Role with ID '{roleId}' not found.");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            var response = claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });

            return Ok(new { RoleId = roleId, Claims = response });
        }

        [HttpGet("get-role-claim/{roleId}/{claimType}")]
        public async Task<IActionResult> GetClaimByType(string roleId, string claimType)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return NotFound($"Role with ID '{roleId}' not found.");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            var matchingClaims = claims.Where(c => c.Type == claimType).Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });

            if (!matchingClaims.Any())
            {
                return NotFound($"No claims of type '{claimType}' found for role with ID '{roleId}'.");
            }

            return Ok(new { RoleId = roleId, Claims = matchingClaims });
        }

        [HttpPut("update-role-claim")]
        public async Task<IActionResult> UpdateRoleClaim([FromBody] UpdateRoleClaimModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound($"Role with ID '{model.RoleId}' not found.");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            var oldClaim = claims.FirstOrDefault(c => c.Type == model.OldClaimType && c.Value == model.OldClaimValue);

            if (oldClaim == null)
            {
                return NotFound($"Claim '{model.OldClaimType}: {model.OldClaimValue}' not found in role with ID '{model.RoleId}'.");
            }

            var removeResult = await _roleManager.RemoveClaimAsync(role, oldClaim);
            if (!removeResult.Succeeded)
            {
                return BadRequest(new { Message = "Failed to remove the old claim.", Errors = removeResult.Errors });
            }

            var newClaim = new Claim(model.NewClaimType, model.NewClaimValue);
            var addResult = await _roleManager.AddClaimAsync(role, newClaim);

            if (addResult.Succeeded)
            {
                return Ok($"Claim updated successfully for role with ID '{model.RoleId}'.");
            }

            return BadRequest(new { Message = "Failed to add the new claim.", Errors = addResult.Errors });
        }

        [HttpDelete("delete-role-claim")]
        public async Task<IActionResult> DeleteRoleClaim([FromBody] RoleClaimModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound($"Role with ID '{model.RoleId}' not found.");
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            var claimToDelete = claims.FirstOrDefault(c => c.Type == model.ClaimType && c.Value == model.ClaimValue);

            if (claimToDelete == null)
            {
                return NotFound($"Claim '{model.ClaimType}: {model.ClaimValue}' not found for role with ID '{model.RoleId}'.");
            }

            var result = await _roleManager.RemoveClaimAsync(role, claimToDelete);

            if (result.Succeeded)
            {
                return Ok($"Claim '{model.ClaimType}: {model.ClaimValue}' deleted from role with ID '{model.RoleId}'.");
            }

            return BadRequest(new { Message = "Failed to delete claim.", Errors = result.Errors });
        }
    }
}