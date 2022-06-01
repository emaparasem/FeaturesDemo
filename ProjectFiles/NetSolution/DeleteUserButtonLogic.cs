#region StandardUsing
using FTOptix.NetLogic;
using UAManagedCore;
#endregion

public class DeleteUserButtonLogic : FTOptix.NetLogic.BaseNetLogic {
    [ExportMethod]
    public void DeleteUser(NodeId userToDelete) {
        var userObjectToRemove = Owner.Context.GetNode(userToDelete);
        if (userObjectToRemove == null) {
            Log.Error("UserEditor", "Cannot obtain the selected user.");
            return;
        }

        var userVariable = Owner.Owner.Owner.Owner.GetVariable("Users");
        if (userVariable == null) {
            Log.Error("UserEditor", "Missing user variable in UserEditor Panel.");
            return;
        }

        if (userVariable.Value == null || (NodeId)userVariable.Value == NodeId.Empty) {
            Log.Error("UserEditor", "Fill User variable in UserEditor.");
            return;
        }
        var userFolder = Owner.Context.GetNode(userVariable.Value);
        if (userFolder == null) {
            Log.Error("UserEditor", "Cannot obtain Users folder.");
            return;
        }


        userFolder.Remove(userObjectToRemove);
    }
}
