using System;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web.Controls;
using Inedo.Web.Controls;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    /// <summary>
    /// Provides the UI elements for configuring a convert project library action.
    /// </summary>
    public sealed class ConvertProjectLibraryActionEditor : OldActionEditorBase
    {
        private SourceControlFileFolderPicker libPath;
        private ValidatingTextBox searchMask;
        private CheckBox recursive;

        /// <summary>
        /// Initializes a new instance of the ConvertProjectLibraryActionEditor class.
        /// </summary>
        public ConvertProjectLibraryActionEditor()
        {
        }

        public override bool DisplaySourceDirectory
        {
            get { return true; }
        }

        protected override void CreateChildControls()
        {
            this.libPath = new SourceControlFileFolderPicker() { Required = true, DisplayMode = SourceControlBrowser.DisplayModes.Folders, ServerId = this.ServerId };
            this.searchMask = new ValidatingTextBox() { Required = true, Text = "*.csproj", TextMode = TextBoxMode.MultiLine,  Rows = 4, Columns = 30, Width = 300 };
            this.recursive = new CheckBox() { Text = "Recursive" };

            CUtil.Add(this,
                new FormFieldGroup(
                    "Library",
                    "The library directory which contains referenced assemblies.",
                    false,
                    new StandardFormField(string.Empty, this.libPath)
                    ),
                new FormFieldGroup(
                    "Project Files",
                    "Determines which project files are converted.",
                    true,
                    new StandardFormField("File Masks:", this.searchMask),
                    new StandardFormField(string.Empty, this.recursive)
                    )
                );
        }

        public override void BindActionToForm(ActionBase action)
        {
            EnsureChildControls();

            var convert = (ConvertProjectLibraryAction)action;
            this.libPath.Text = convert.LibraryPath;
            this.searchMask.Text = string.Join(Environment.NewLine, convert.SearchMasks);
            this.recursive.Checked = convert.Recursive;
        }

        public override ActionBase CreateActionFromForm()
        {
            EnsureChildControls();

            return new ConvertProjectLibraryAction()
            {
                LibraryPath = this.libPath.Text,
                SearchMasks = Regex.Split(this.searchMask.Text, "\r?\n"),
                Recursive = this.recursive.Checked
            };
        }
    }
}
