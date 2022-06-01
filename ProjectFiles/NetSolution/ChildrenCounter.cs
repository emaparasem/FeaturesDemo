using UAManagedCore;

public class ChildrenCounter : FTOptix.NetLogic.BaseNetLogic {
    public override void Start() {
        var nodePointerVariable = LogicObject.Children.Get<IUAVariable>("Node");

        if (nodePointerVariable == null) {
            Log.Error("ChildrenCounter", "Missing NodepathToMonitorVariable on ChildrenCounter");
            return;
        }

        if ((NodeId)nodePointerVariable.Value.Value == NodeId.Empty) {
            Log.Warning("ChildrenCounter", "Node variable not set");
            return;
        }

        var resolvedResult = LogicObject.Context.GetNode((NodeId)nodePointerVariable.Value.Value);

        var countVariable = LogicObject.Children.Get<IUAVariable>("Count");
        countVariable.Value = resolvedResult.Children.Count;
        if (countVariable == null) {
            Log.Error("ChildrenCounter", "Missing variable Count on ChildrenCounter");
            return;
        }

        if (referencesEventRegistration != null) {
            referencesEventRegistration.Dispose();
            referencesEventRegistration = null;
        }

        referencesObserver = new ReferencesObserver(resolvedResult, countVariable);
        referencesObserver.Initialize();

        referencesEventRegistration = resolvedResult.RegisterEventObserver(
            referencesObserver, EventType.ForwardReferenceAdded | EventType.ForwardReferenceRemoved);
    }

    public override void Stop() {
        // Insert code to be executed when the user-defined logic is stopped
        if (referencesEventRegistration != null)
            referencesEventRegistration.Dispose();

        referencesEventRegistration = null;
        referencesObserver = null;
    }

    ReferencesObserver referencesObserver;
    IEventRegistration referencesEventRegistration;
}

public class ReferencesObserver : IReferenceObserver {
    public ReferencesObserver(IUANode nodeToMonitor, IUAVariable countVariable) {
        this.nodeToMonitor = nodeToMonitor;
        this.countVariable = countVariable;
    }

    public void Initialize() {
        countVariable.Value = nodeToMonitor.Children.Count;
    }

    public void OnReferenceAdded(IUANode sourceNode, IUANode targetNode, NodeId referenceTypeId, ulong senderId) {
        if (IsReferenceAllowed(referenceTypeId)) {
            ++countVariable.Value;
        }
    }

    public void OnReferenceRemoved(IUANode sourceNode, IUANode targetNode, NodeId referenceTypeId, ulong senderId) {
        if (IsReferenceAllowed(referenceTypeId) && countVariable.Value > 0) {
            --countVariable.Value;
        }
    }

    public bool IsReferenceAllowed(NodeId referenceTypeId) {
        return referenceTypeId == UAManagedCore.OpcUa.ReferenceTypes.HasComponent ||
               referenceTypeId == UAManagedCore.OpcUa.ReferenceTypes.HasOrderedComponent;
    }

    private IUANode nodeToMonitor;
    private IUAVariable countVariable;
}
