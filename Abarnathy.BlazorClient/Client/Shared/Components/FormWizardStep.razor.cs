using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;

namespace Abarnathy.BlazorClient.Client.Shared.Components
{
    public partial class FormWizardStep
    {
        [CascadingParameter] protected internal FormWizard Parent { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter] public string Name { get; set; }

      
        protected override void OnInitialized()
        {
            Parent.AddStep(this);
        }
    }
}