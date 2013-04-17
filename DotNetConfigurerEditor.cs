using Inedo.BuildMaster.Extensibility.Configurers.Extension;
using Inedo.BuildMaster.Web.Controls;
using Inedo.BuildMaster.Web.Controls.Extensions;
using Inedo.Web.Controls;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    /// <summary>
    /// Custom editor for the .NET configurer class.
    /// </summary>
    public sealed class DotNetConfigurerEditor : ExtensionConfigurerEditorBase
    {
        private ValidatingTextBox txtSdkPath;
        private ValidatingTextBox txtFrameworkPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetConfigurerEditor"/> class.
        /// </summary>
        public DotNetConfigurerEditor()
        {
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.txtSdkPath = new ValidatingTextBox() { Required = true, Width = 300 };
            this.txtFrameworkPath = new ValidatingTextBox() { Required = true, Width = 300 };

            CUtil.Add(this,
                new FormFieldGroup(
                    "Environment",
                    "Specifies the locations of .NET framework components.",
                    true,
                    new StandardFormField(
                        "Windows SDK/.NET Framework SDK path:",
                        this.txtSdkPath),
                    new StandardFormField(
                        ".NET Framework install path:",
                        this.txtFrameworkPath)
                    )
                );
        }

        public override void BindToForm(ExtensionConfigurerBase extension)
        {
            EnsureChildControls();

            var cfg = (DotNetConfigurer)extension;
            this.txtSdkPath.Text = cfg.SdkPath;
            this.txtFrameworkPath.Text = cfg.FrameworkRuntimePath;
        }

        public override ExtensionConfigurerBase CreateFromForm()
        {
            EnsureChildControls();

            return new DotNetConfigurer()
            {
                SdkPath = this.txtSdkPath.Text,
                FrameworkRuntimePath = this.txtFrameworkPath.Text
            };
        }

        /// <summary>
        /// Populates the fields within the control with the appropriate default values.
        /// </summary>
        /// <remarks>
        /// This is only called when creating a new extension.
        /// </remarks>
        public override void InitializeDefaultValues()
        {
            BindToForm(new DotNetConfigurer());
        }
    }
}
