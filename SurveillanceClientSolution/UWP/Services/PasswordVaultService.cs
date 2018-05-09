using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.Services.Interfaces;
using Windows.Security.Credentials;

namespace UWP.Services
{
    public class PasswordVaultService : IPasswordVaultService
    {
        private const string DateTimeSavingFormat = "yyyy.MM.dd.HH.mm.ss.fff";
        private const string BearerTokenVaultKey = "bearer_token_vault_key";
        private const string RefreshTokenVaultKey = "refresh_token_vault_key";
        private const int ElementNotFound = unchecked((int)0x80070490);
        private PasswordVault _passwordVault;

        public PasswordVaultService()
        {
            _passwordVault = new PasswordVault();
        }

        public void SaveTokens()
        {
            if (!string.IsNullOrEmpty(App.BearerToken))
            {
                try
                {
                    EmptyVault();

                    var bearer = new PasswordCredential(BearerTokenVaultKey, App.BearerExpires.ToString(DateTimeSavingFormat), App.BearerToken);
                    var refresh = new PasswordCredential(RefreshTokenVaultKey, App.RefreshTokenExpires.ToString(DateTimeSavingFormat), App.RefreshToken);

                    _passwordVault.Add(bearer);
                    _passwordVault.Add(refresh);
                }
                catch (Exception ex)
                {
                    Debug.Write("SaveTokensError: " + ex.ToString());
                }
            }
            else
            {
                Debug.Write("Token is empty");
            }
        }

        public void LoadExistingTokens()
        {
            try
            {
                var bearer = _passwordVault.FindAllByResource(BearerTokenVaultKey).FirstOrDefault();
                var refresh = _passwordVault.FindAllByResource(RefreshTokenVaultKey).FirstOrDefault();
                bearer.RetrievePassword();
                refresh.RetrievePassword();
                if (bearer == null || refresh == null)
                    return;

                App.BearerToken = bearer.Password;
                App.BearerExpires = SafeDatetimeParse(bearer.UserName);
                App.RefreshToken = refresh.Password;
                App.RefreshTokenExpires = SafeDatetimeParse(refresh.UserName);
            }
            catch (Exception ex)
            {
                if (ex.HResult != ElementNotFound)
                {
                    Debug.Write("LoadExistingTokenError: " + ex.ToString());
                    RemoveAllCredentials();
                }
            }
        }

        public void RemoveExistingTokens()
        {
            RemoveAllCredentials();
        }

        private void EmptyVault()
        {
            var creds = _passwordVault.RetrieveAll();
            foreach (var cred in creds)
            {
                _passwordVault.Remove(cred);
            }
        }

        private void RemoveAllCredentials()
        {
            try
            {
                EmptyVault();
                // Clear app saved values
                App.BearerToken = string.Empty;
                App.BearerExpires = DateTime.MinValue;
                App.RefreshToken = string.Empty;
                App.RefreshTokenExpires = DateTime.MinValue;
            }
            catch (Exception ex)
            {
                Debug.Write("RemoveAllCredentialsError: " + ex.ToString());
            }
        }

        private DateTime SafeDatetimeParse(string input)
        {
            var separated = input.Split('.');
            List<int> timeParts = new List<int>();
            for (int i = 0; i < separated.Length; i++)
            {
                int.TryParse(separated[i], out int res);
                timeParts.Add(res);
            }
            var results = timeParts.ToArray();
            return new DateTime(results[0], results[1], results[2], results[3], results[4], results[5], results[6]);
        }
    }
}
