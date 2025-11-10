using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceTracker.API.Controllers
{
    /// <summary>
    /// Base controller with helper methods for JWT authentication
    /// </summary>
    public class AuthenticatedControllerBase : ControllerBase
    {
        /// <summary>
        /// Get the authenticated user's ID from JWT claims
        /// </summary>
        protected string GetAuthenticatedUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                         ?? User.FindFirst("sub")?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token claims");
            }
            
            return userId;
        }

        /// <summary>
        /// Check if the authenticated user matches the requested userId
        /// </summary>
        protected bool IsAuthorizedUser(string requestedUserId)
        {
            var authenticatedUserId = GetAuthenticatedUserId();
            return authenticatedUserId == requestedUserId;
        }
    }
}
