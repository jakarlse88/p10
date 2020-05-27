using Abarnathy.BlazorClient.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Abarnathy.BlazorClient.Client.Shared.Components.NoteEditForm
{
    public partial class NoteEditForm
    {
        [Parameter] public NoteInputModel InputModel { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> SubmitCallback { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> CancelCallback { get; set; }
        [Parameter] public APIOperationStatus OperationStatus { get; set; }
    }
}