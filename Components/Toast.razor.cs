using BlazorWasmAwsAmplify.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWasmAwsAmplify.Components
{
    public class ToastBase : ComponentBase, IDisposable
    {
        [Inject] NotificationService NotificationService { get; set; }

        [Parameter]
        public string Message { get; set; }

        [Parameter]
        public string Time { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }
        protected string BackgroundCssClass { get; set; }
        protected string IconCssClass { get; set; }
        protected bool IsInfinite { get; set; }
        

        protected override void OnInitialized()
        {
            NotificationService.OnShow += ShowToast;
            NotificationService.OnHide += HideToast;
        }

        public void ShowToast(string message, ToastLevel level, bool isInfinite)
        {
            BuildToastSettings(message, level, isInfinite);
            IsVisible = true;
            StateHasChanged();
        }

        public void HideToast()
        {
            IsVisible = false;
            StateHasChanged();
        }

        private void BuildToastSettings(string message, ToastLevel level, bool isInfinite)
        {
            switch (level)
            {
                case ToastLevel.Info:
                    BackgroundCssClass = "bg-info";
                    IconCssClass = "info";
                    break;
                case ToastLevel.Success:
                    BackgroundCssClass = "bg-success";
                    IconCssClass = "check";
                    break;
                case ToastLevel.Warning:
                    BackgroundCssClass = "bg-warning";
                    IconCssClass = "exclamation";
                    break;
                case ToastLevel.Error:
                    BackgroundCssClass = "bg-danger";
                    IconCssClass = "times";
                    break;
            }
            this.IsInfinite = isInfinite;
            this.Message = message;
        }//BuildToastSettings

        public void Dispose()
        {
            NotificationService.OnShow -= ShowToast;
        }

    }
}
