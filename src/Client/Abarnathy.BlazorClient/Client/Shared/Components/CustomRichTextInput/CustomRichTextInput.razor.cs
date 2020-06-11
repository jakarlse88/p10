using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

// ReSharper disable InconsistentNaming

namespace Abarnathy.BlazorClient.Client.Shared.Components.CustomRichTextInput
{
    // Source: https://cpratt.co/blazor-rich-text-editor-using-ckeditor/
    public partial class CustomRichTextInput : InputTextArea
    {
        [Inject] public IJSRuntime JSRuntime { get; set; }
        [Parameter] public string Id 
        {
            get => _id ?? $"CKEDITOR_{uid}";
            set => _id = value; 
        }

        private string _id { get; set; }
        private readonly string uid = Guid.NewGuid().ToString().ToLower().Replace("-", "");

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("CKEditorInterop.init", Id, DotNetObjectReference.Create(this));
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public Task EditorDataChanged(string data)
        {
            CurrentValue = data;
            StateHasChanged();
            return Task.CompletedTask;
        }

        protected override void Dispose(bool disposing)
        {
            JSRuntime.InvokeVoidAsync("CKEditorInterop.destroy", Id);
            base.Dispose(disposing);
        }
    }
}
