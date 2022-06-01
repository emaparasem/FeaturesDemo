#region StandardUsing
using FTOptix.NetLogic;
using FTOptix.UI;
using UAManagedCore;
#endregion

public class UserEditorPanelLoaderLogic : FTOptix.NetLogic.BaseNetLogic {
    public override void Start() {
        // Insert code to be executed when the user-defined logic is started
    }

    public override void Stop() {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void GoToUserDetailsPanel() {
        var userCountVariable = LogicObject.Get<IUAVariable>("UserCount");
        if (userCountVariable == null)
            return;

        var noUsersPanelVariable = LogicObject.Get<IUAVariable>("NoUsersPanel");
        if (noUsersPanelVariable == null)
            return;

        var userDetailPanelVariable = LogicObject.Get<IUAVariable>("UserDetailPanel");
        if (userDetailPanelVariable == null)
            return;

        var panelLoader = (PanelLoader)Owner;

        NodeId newPanelNode = userCountVariable.Value > 0 ? userDetailPanelVariable.Value : noUsersPanelVariable.Value;
        NodeId userAlias = userCountVariable.Value > 0 ? Owner.Owner.Get<ListBox>("UsersList").SelectedItem : NodeId.Empty;

        panelLoader.ChangePanel(newPanelNode, userAlias);
    }
}
