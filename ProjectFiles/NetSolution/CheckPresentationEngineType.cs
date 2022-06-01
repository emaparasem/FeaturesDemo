#region Using directives
using FTOptix.Core;
using FTOptix.HMIProject;
using FTOptix.NetLogic;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
#endregion

public class CheckPresentationEngineType : BaseNetLogic {
    public override void Start() {
        var isNativeUI = Session.GetVariable("IsNativeUI");
        if (isNativeUI == null) {
            isNativeUI = InformationModel.MakeVariable("IsNativeUI", OpcUa.DataTypes.Boolean);
            Session.Add(isNativeUI);
        }

        var presentationEngine = FindPresentationEngine();
        if (presentationEngine != null)
            isNativeUI.Value = presentationEngine.IsInstanceOf(FTOptix.NativeUI.ObjectTypes.NativeUIPresentationEngine);
    }

    IUAObject FindPresentationEngine() {
        IUANode currentNode = Session;

        while (true) {
            if (currentNode == null)
                return null;

            var currentObject = (IUAObject)currentNode;
            if (currentObject != null && currentObject.IsInstanceOf(FTOptix.UI.ObjectTypes.PresentationEngine))
                return currentObject;

            currentNode = currentNode.Owner;
        }
    }
}
