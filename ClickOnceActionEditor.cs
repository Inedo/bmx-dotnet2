using System;
using System.Web.UI.WebControls;
using Inedo.BuildMaster.Extensibility.Actions;
using Inedo.BuildMaster.Web.Controls;
using Inedo.Web.Controls;

namespace Inedo.BuildMasterExtensions.DotNet2
{
    public sealed class ClickOnceActionEditor : OldActionEditorBase
    {
        // Fields
        ValidatingTextBox txtApplicationName;
        ValidatingTextBox txtProviderUrl;
        SourceControlFileFolderPicker txtCertificatePath;
        PasswordTextBox txtCertificatePassword;
        ValidatingTextBox txtCertificateHash;
        ValidatingTextBox txtVersion;
        CheckBox chkMapFileExtensions, chkInstallApplication;

        #region Configuration
        public override bool DisplaySourceDirectory { get { return true; } }
        #endregion

        public ClickOnceActionEditor()
        {
            Init += new EventHandler(ClickOnceActionEditor_Init);
            ValidateBeforeCreate += new EventHandler<ActionEditorValidationEventArgs>(ClickOnceActionEditor_ValidateBeforeCreate);
        }

        void ClickOnceActionEditor_ValidateBeforeCreate(object sender, ActionEditorValidationEventArgs e)
        {
            //Validate .NET2 is installed on server
            DotNet2Helper dn2 = DotNet2Helper.Proxy(ServerId);
            if (!dn2.IsMageAvailable(((DotNetConfigurer)GetExtensionConfigurer()).SdkPath))
            {
                e.ValidLevel = ActionEditorValidLevels.Warning;
                e.Message += "mage.exe is not installed on the selected server, which may result in build errors.";
            }
        }

        void ClickOnceActionEditor_Init(object sender, EventArgs e)
        {
            // txtApplicationName
            txtApplicationName = new ValidatingTextBox();
            txtApplicationName.Required = true;
            txtApplicationName.Width = 300;

            // txtProviderUrl
            txtProviderUrl = new ValidatingTextBox();
            txtProviderUrl.Required = true;
            txtProviderUrl.Width = 300;

            // txtCertificatePath
            txtCertificatePath = new SourceControlFileFolderPicker();
            txtCertificatePath.ServerId = ServerId;
            txtCertificatePath.Width = 300;

            // txtCertificatePassword
            txtCertificatePassword = new PasswordTextBox();
            txtCertificatePassword.Width = 250;

            // txtCertificateHash
            txtCertificateHash = new ValidatingTextBox();
            txtCertificateHash.Width = 300;

            // txtVersion
            txtVersion = new ValidatingTextBox();
            txtVersion.Required = true;
            txtVersion.Width = 300;

            // chkMapFileExtensions
            chkMapFileExtensions = new CheckBox();
            chkMapFileExtensions.Text = "Rename files to .deploy";

            // chkInstallApplication
            chkInstallApplication = new CheckBox();
            chkInstallApplication.Text = "Install application onto local machine";

            CUtil.Add(this,
                new FormFieldGroup(
                    "Application Settings",
                    "Configuration for the application. Note that the version number "
                    + "must be of the form 0.0.0.0 and the provider URL should be where "
                    + "the application is deployed to (e.g. http://example.com/MyApp/)",
                    false,
                    new StandardFormField(
                        "Application Name:",
                        txtApplicationName),
                    new StandardFormField(
                        "Version Number:",
                        txtVersion),
                    new StandardFormField(
                        "Provider URL:",
                        txtProviderUrl)
                    ),
                new FormFieldGroup(
                    "File Extension Mapping",
                    "Determines whether files in the deployment will have a .deploy extension. "
                    + "ClickOnce will strip this extension off these files as soon as it downloads them "
                    + "from the Web server. This parameter allows all the files within a ClickOnce deployment "
                    + "to be downloaded from a Web server that blocks transmission of files ending in \"unsafe\" "
                    + "extensions such as .exe. ",
                    false,
                    new StandardFormField(string.Empty, chkMapFileExtensions)),
                new FormFieldGroup(
                    "Install Application",
                    "Indicates whether or not the ClickOnce application should install onto the local machine, "
                    + "or whether it should run from the Web. Installing an application gives that application a " 
                    + "presence in the Windows Start menu.",
                    false,
                    new StandardFormField(string.Empty, chkInstallApplication)),
                new FormFieldGroup(
                    "Certificate Settings",
                    "ClickOnce applications must be signed with an X509 certificate, "
                    + "which may be stored on disk or in the local cert store. "
                    + "<br /><br />Note that either a Certificate Path or Certificate Hash "
                    + "must be selected, but not both",
                    true,
                    new StandardFormField(
                        "Certificate Path:",
                        txtCertificatePath),
                    new StandardFormField(
                        "Certificate Hash:",
                        txtCertificateHash),
                    new StandardFormField(
                        "Certificate Password:",
                        txtCertificatePassword)));
        }

        public override void BindActionToForm(ActionBase action)
        {
            var c1action = (ClickOnceAction)action;

            txtApplicationName.Text = c1action.ApplicationName;
            txtProviderUrl.Text = c1action.ProviderUrl;
            txtCertificatePath.Text = c1action.CertificatePath;
            txtCertificatePassword.Text = c1action.CertificatePassword;
            txtCertificateHash.Text = c1action.CertificateHash;
            txtVersion.Text = c1action.Version;
            chkMapFileExtensions.Checked = c1action.MapFileExtensions;
            chkInstallApplication.Checked = c1action.InstallApplication;
        }

        public override ActionBase CreateActionFromForm()
        {
            var c1action = new ClickOnceAction();

            c1action.ApplicationName = txtApplicationName.Text;
            c1action.ProviderUrl = txtProviderUrl.Text;
            c1action.CertificatePath = txtCertificatePath.Text;
            c1action.CertificatePassword     = txtCertificatePassword.Text;
            c1action.CertificateHash = txtCertificateHash.Text;
            c1action.Version = txtVersion.Text;
            c1action.MapFileExtensions = chkMapFileExtensions.Checked; ;
            c1action.InstallApplication = chkInstallApplication.Checked;
            return c1action;
        }
    }
}
