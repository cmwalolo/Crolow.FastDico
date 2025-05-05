using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MauiBlazorWeb.Shared.ComponentBases.Base
{
    public class ModalComponent : ComponentBase
    {
        public Guid Guid = Guid.NewGuid();
        public string ModalDisplay { get; set; } = "none;";
        public string ModalClass { get; set; } = "fade";

        protected ElementReference ModalDiv { get; set; }  // set the @ref for attribute

        [Inject]
        IJSRuntime jsRuntime { get; set; }

        protected bool needsFocus = false;
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (needsFocus)
            {
                needsFocus = false;
                await jsRuntime.InvokeVoidAsync("SetFocusToElement", ModalDiv);
            }
        }
        public void Open()
        {
            needsFocus = true;
            ModalDisplay = "block;";
            ModalClass = "Show";
            StateHasChanged();
        }

        public virtual void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            StateHasChanged();
        }
    }
}
