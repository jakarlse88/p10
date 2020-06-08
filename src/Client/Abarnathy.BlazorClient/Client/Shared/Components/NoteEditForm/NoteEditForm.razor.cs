using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;

namespace Abarnathy.BlazorClient.Client.Shared.Components.NoteEditForm
{
    public partial class NoteEditForm
    {
        [Parameter] public ComponentMode Mode { get; set; }
        [Parameter] public NoteInputModel InputModel { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> SubmitCallback { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> CancelCallback { get; set; }
        [Parameter] public APIOperationStatus OperationStatus { get; set; }
        private bool _isValid;
        private EditContext _editContext;

        protected override void OnInitialized()
        {
            _editContext = new EditContext(InputModel);
            
            _editContext.OnFieldChanged += (sender, e) =>
            {
                _isValid = _editContext.Validate();
                StateHasChanged();
            };

            _isValid = Mode != ComponentMode.Create && _editContext.Validate();
        }
    }
}