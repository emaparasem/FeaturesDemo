#region StandardUsing
using FTOptix.Core;
using FTOptix.NetLogic;
using System;
using UAManagedCore;
#endregion

public class LoginButtonLogic : FTOptix.NetLogic.BaseNetLogic {
    public override void Start() {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop() {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void PerformLogin(string username, string password, out bool loginResult) {
        var usersAlias = LogicObject.GetAlias("Users");
        if (usersAlias == null || usersAlias.NodeId == NodeId.Empty) {
            Log.Error("LoginForm", "Missing Users alias");
            loginResult = false;
            return;
        }

        var user = usersAlias.Get<User>(username);
        if (user == null) {
            Log.Error("LoginForm", "Could not find user " + username);
            loginResult = false;
            return;
        }

        try {
            user.PasswordVariable.RemoteRead();
            loginResult = Session.ChangeUser(username, password);
        } catch (Exception e) {
            Log.Error("LoginForm", e.Message);
            loginResult = false;
        }
    }

}
