using Microsoft.AspNetCore.Components;

namespace Abarnathy.BlazorClient.Client.Shared.Components.FormWizard
{
    public partial class FormWizardStep
    {
        [CascadingParameter] protected internal Components.FormWizard.FormWizard Parent { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Name { get; set; }

      
        protected override void OnInitialized()
        {
            Parent.AddStep(this);
        }
    }
}