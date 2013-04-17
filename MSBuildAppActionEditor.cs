using System.Web.UI.WebControls;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web.Controls;
using Inedo.Web.Controls;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    /// <summary>
    /// Provides the editor for the MSBuild action.
    /// </summary>
    public sealed class MSBuildAppActionEditor : OldActionEditorBase
    {
        private ValidatingTextBox txtProjectFilePath,txtMSBuildTarget, txtAdditionalProperties;
        private DropDownList ddlVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildAppActionEditor"/> class.
        /// </summary>
        public MSBuildAppActionEditor()
        {
        }

        public override bool DisplaySourceDirectory
        {
            get { return true; }
        }
        public override bool DisplayTargetDirectory
        {
            get { return true; }
        }

        protected override void CreateChildControls()
        {
            ddlVersion = new DropDownList();
            ddlVersion.Items.Add(new ListItem("(auto detect)", ""));
            ddlVersion.Items.Add(new ListItem("2.0", "2.0.50727"));
            ddlVersion.Items.Add(new ListItem("3.5", "3.5"));
            ddlVersion.Items.Add(new ListItem("4.0", "4.0.30319"));

            txtProjectFilePath = new ValidatingTextBox() 
            { 
                Width = 300
            };
            
            txtMSBuildTarget = new ValidatingTextBox() 
            { 
                Width = 300,
                Required = true
            };

            txtAdditionalProperties = new ValidatingTextBox()
            {
                Width = 300,
                TextMode = TextBoxMode.MultiLine,
                Rows = 5
            };

            CUtil.Add(this,
                new FormFieldGroup(".NET Framework Version",
                    "The version of the .NET Framework to use when building this project.",
                    false,
                    new StandardFormField("Version:", ddlVersion)
                )
            );

            FormFieldGroup FilePathFieldGroup =
                new FormFieldGroup("MSBuild File Path",
                    "The path of the msbuild file.",
                    false,
                    new StandardFormField("MSBuild File Path:", txtProjectFilePath)
                );

            FormFieldGroup TargetFieldGroup =
                new FormFieldGroup("MSBuild Target",
                    "The MSBuild Target property. For example: Build",
                    false,
                    new StandardFormField("MSBuild Target:", txtMSBuildTarget)
                );

            FormFieldGroup PropertiesFieldGroup =
                new FormFieldGroup("MSBuild Properties",
                    "Additional properties, separated by newlines.  Example:<br />WarningLevel=2<br />Optimize=false",
                    true,
                    new StandardFormField("MSBuild Properties:", txtAdditionalProperties)
                );

            CUtil.Add(this, FilePathFieldGroup, TargetFieldGroup, PropertiesFieldGroup);
        }

        protected override void OnValidateBeforeSave(ActionEditorValidationEventArgs e)
        {
            if (string.IsNullOrEmpty(txtProjectFilePath.Text))
            {
                e.ValidLevel = ActionEditorValidLevels.Warning;
                e.Message = "Project path is not set. This may result in build errors.";
            }
            return;
        }

        public override void BindActionToForm(ActionBase action)
        {
            EnsureChildControls();

            var buildAction = (MSBuildAppAction)action;

            txtProjectFilePath.Text = buildAction.MSBuildPath;
            txtMSBuildTarget.Text = buildAction.ProjectBuildTarget;
            txtAdditionalProperties.Text = buildAction.MSBuildProperties;
            ddlVersion.SelectedValue = buildAction.DotNetVersion ?? "";
        }

        public override ActionBase CreateActionFromForm()
        {
            EnsureChildControls();

            string buildProperties = txtAdditionalProperties.Text;
            if (buildProperties.StartsWith("/p:"))
            {
                buildProperties = buildProperties.Replace("/p:", "");
            }

            var buildAction = new MSBuildAppAction();

            buildAction.MSBuildPath = txtProjectFilePath.Text;
            buildAction.ProjectBuildTarget = txtMSBuildTarget.Text;
            buildAction.MSBuildProperties = buildProperties;
            buildAction.DotNetVersion = ddlVersion.SelectedValue;

            return buildAction;
        }
    }
}
