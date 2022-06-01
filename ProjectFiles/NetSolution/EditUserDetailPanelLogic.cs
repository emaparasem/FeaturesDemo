#region Using directives
using FTOptix.NetLogic;
using UAManagedCore;
#endregion

public class EditUserDetailPanelLogic : BaseNetLogic {
    [ExportMethod]
    public void SaveUser(string username, string password, string locale, out NodeId result) {
        result = NodeId.Empty;

        if (string.IsNullOrEmpty(username)) {
            Log.Error("EditUserDetailPanelLogic", "Cannot create user with empty username");
            return;
        }

        result = ApplyUser(username, password, locale);
    }

    private NodeId ApplyUser(string username, string password, string locale) {
        var users = GetUsers();
        if (users == null) {
            Log.Error("EditUserDetailPanelLogic", "Unable to get users");
            return NodeId.Empty;
        }

        var user = users.Get<FTOptix.Core.User>(username);
        user.Password = password;
        user.LocaleIds = new string[] { locale };

        return user.NodeId;
    }

    private IUANode GetUsers() {
        var pathResolverResult = LogicObject.Context.ResolvePath(LogicObject, "{Users}");
        if (pathResolverResult == null)
            return null;
        if (pathResolverResult.ResolvedNode == null)
            return null;

        return pathResolverResult.ResolvedNode;
    }
}
