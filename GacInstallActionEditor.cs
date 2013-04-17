using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web.Controls;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    public sealed class GacInstallActionEditor : OldActionEditorBase
    {
        private TextBox txtGacFiles;
        private CheckBox chkForceRefresh;

        public override bool DisplaySourceDirectory
        {
            get { return true; }
        }

        protected override void OnInit(EventArgs e)
        {
            txtGacFiles = new TextBox();
            txtGacFiles.TextMode = TextBoxMode.MultiLine;
            txtGacFiles.Rows = 4;
            txtGacFiles.Columns = 30;
            txtGacFiles.Width = Unit.Pixel(300);

            chkForceRefresh = new CheckBox();
            chkForceRefresh.Text = "Force refresh";
            chkForceRefresh.Checked = true;

            FormFieldGroup GacFilesFieldGroup =
                new FormFieldGroup("Files",
                    "Files listed (entered one per line) will be added to the GAC.",
                    true,
                    new StandardFormField("Files:", txtGacFiles),
                    new StandardFormField("", chkForceRefresh)
                );

            CUtil.Add(this, GacFilesFieldGroup);
        }

        public override void BindActionToForm(ActionBase action)
        {
            var gac = (GacInstallAction)action;

            txtGacFiles.Text = string.Join(Environment.NewLine, gac.FileMasks);
            chkForceRefresh.Checked = gac.ForceRefresh;
        }

        public override ActionBase CreateActionFromForm()
        {
            return new GacInstallAction()
            {
                FileMasks = Regex.Split(txtGacFiles.Text, Environment.NewLine),
                ForceRefresh = chkForceRefresh.Checked
            };
        }
    }
}
