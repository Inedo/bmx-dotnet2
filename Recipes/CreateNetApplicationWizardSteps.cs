﻿using Inedo.BuildMaster.Web.Controls.Extensions;

namespace Inedo.BuildMasterExtensions.DotNet2.Recipes
{
    public sealed class CreateNetApplicationWizardSteps : RecipeWizardSteps
    {
        public readonly RecipeWizardStep SelectProviderAndFile = new RecipeWizardStep("Select Provider/File");
        public readonly RecipeWizardStep SelectProjectsInSolution = new RecipeWizardStep("Projects");
        public readonly RecipeWizardStep SelectConfigFiles = new RecipeWizardStep("Config Files");
        public readonly RecipeWizardStep SelectDeploymentPaths = new RecipeWizardStep("Deployment Path");
        public readonly RecipeWizardStep Confirmation = new RecipeWizardStep("Summary");

        public override RecipeWizardStep[] WizardStepOrder
        {
            get
            {
                return new[] 
                { 
                    this.SpecifyApplicationProperties, this.SpecifyWorkflowOrder, this.SelectProviderAndFile, 
                    this.SelectProjectsInSolution, this.SelectConfigFiles, this.SelectDeploymentPaths, 
                    this.Confirmation 
                };
            }
        }
    }
}
