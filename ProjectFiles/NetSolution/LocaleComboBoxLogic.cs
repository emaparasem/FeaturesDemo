#region StandardUsing
using FTOptix.HMIProject;
using FTOptix.UI;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
#endregion

public class LocaleComboBoxLogic : FTOptix.NetLogic.BaseNetLogic {
    public override void Start() {
        var localeCombo = (ComboBox)Owner;

        var projectLocales = (string[])Project.Current.GetVariable("Locales").Value;
        var modelLocales = InformationModel.MakeObject("Locales");
        modelLocales.Children.Clear();

        foreach (var locale in projectLocales) {
            var language = InformationModel.MakeVariable(locale, OpcUa.DataTypes.String);
            language.Value = locale;
            modelLocales.Children.Add(language);
        }

        LogicObject.Children.Add(modelLocales);
        localeCombo.Model = modelLocales.NodeId;
    }

    public override void Stop() {
        // Insert code to be executed when the user-defined logic is stopped
    }
}
