﻿using Hellion.Core.Data.Headers;
using Hellion.Core.IO;
using Hellion.Core.Network;
using System.Linq;

namespace Hellion.Login.Client
{
    public partial class LoginClient
    {
        private void OnLoginRequest(FFPacket packet)
        {
            var buildVersion = packet.Read<string>();
            var username = packet.Read<string>();
            var password = packet.Read<string>();

            Log.Debug("Recieved from client: buildVersion: {0}, username: {1}, password: {2}", buildVersion, username, password);

            // Database request
            var user = (from x in LoginServer.DbContext.Users
                        where x.Username == username
                        select x).FirstOrDefault();

            if (user == null)
            {
                Log.Info($"User '{username}' logged in with bad credentials. (Bad username)");
                this.SendLoginError(LoginHeaders.LoginErrors.WrongID);
                this.Server.RemoveClient(this);
            }
            else
            {
                if (buildVersion.ToLower() != this.Server.LoginConfiguration.BuildVersion?.ToLower())
                {
                    Log.Info($"User '{username}' logged in with bad build version.");
                    this.SendLoginError(LoginHeaders.LoginErrors.ResourceWasFalsified);
                    this.Server.RemoveClient(this);
                    return;
                }

                if (password.ToLower() != user.Password.ToLower())
                {
                    Log.Info($"User '{username}' logged in with bad credentials. (Bad password)");
                    this.SendLoginError(LoginHeaders.LoginErrors.WrongPassword);
                    this.Server.RemoveClient(this);
                    return;
                }

                if (user.Authority <= 0)
                {
                    Log.Info($"User '{username}' account is suspended.");
                    this.SendLoginError(LoginHeaders.LoginErrors.AccountSuspended);
                    this.Server.RemoveClient(this);
                    return;
                }

                LoginClient connectedClient = null;
                if (this.IsAlreadyConnected(out connectedClient))
                {
                    this.SendLoginError(LoginHeaders.LoginErrors.AccountAlreadyOn);
                    this.Server.RemoveClient(this);
                    this.Server.RemoveClient(connectedClient);
                    return;
                }
				
                this.SendServerList();
            }
        }
    }
}