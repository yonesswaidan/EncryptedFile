using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;

namespace EncryptedFileApp.Data
{
    public class AuthState : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Returner den rigtige bruger, eller anonym hvis man ikke er logget ind
            return Task.FromResult(new AuthenticationState(_currentUser));
        }

        public Task MarkUserAsAuthenticated(List<Claim> claims)
        {
            // Opretter brugerens identitet med claims ved login
            var identity = new ClaimsIdentity(claims, "apiauth_type");
            _currentUser = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return Task.CompletedTask;
        }

        public void MarkUserAsLoggedOut()
        {
            //Fjerner brugerens identitet ved logout for at forhindre adgang for andre 
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
