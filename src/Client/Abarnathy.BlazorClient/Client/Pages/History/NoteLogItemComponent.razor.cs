using System;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace Abarnathy.BlazorClient.Client.Pages.History
{
    public class NoteLogItemComponentBase : ComponentBase
    {
        [CascadingParameter] BlazoredModalInstance BlazoredModal { get; set; }
        [Parameter] public DateTime OriginallyCreated { get; set; }
        [Parameter] public DateTime Archived { get; set; }
        [Parameter] public string Content { get; set; }
    }
}