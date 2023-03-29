using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Income_and_Expense.Components
{
    public partial class MultiSelect2<TValue> : InputBase<TValue>
    {
        [Parameter]
        public string DefaultOption { get; set; }

        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string[] Multiselection { get; set; }
        [Parameter]
        public string ModalId { get; set; }

        [Parameter]
        public IEnumerable<SelectListItem> Datasource
        {
            get
            {
                return _datasource;
            }
            set
            {
                _datasource = value;
                //if (_datasource != null && _datasource.Count() == 1)
                //{
                //    TValue temp;
                //    if (Nullable.GetUnderlyingType(typeof(TValue)) != null)
                //    {
                //        temp = (TValue)Convert.ChangeType(_datasource.First().Value, Nullable.GetUnderlyingType(typeof(TValue)));
                //    }
                //    else
                //    {
                //        temp = (TValue)Convert.ChangeType(_datasource.First().Value, typeof(TValue));
                //    }
                //    if (Value?.ToString() != temp?.ToString())
                //    {
                //        Value = temp;
                //        ValueChanged.InvokeAsync(Value);
                //    }
                //} 
            }
        }
        private IEnumerable<SelectListItem> _datasource;

        [Inject]
        ILogger<MultiSelect2<TValue>> logger { get; set; }



        public DotNetObjectReference<MultiSelect2<TValue>> DotNetRef;
        protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
        {
            try
            {
                if (Nullable.GetUnderlyingType(typeof(TValue)) != null)
                {
                    result = (TValue)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TValue)));
                }
                else
                {
                    result = (TValue)Convert.ChangeType(value, typeof(TValue));
                }
                validationErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                validationErrorMessage = ex.ToString();
            }

            throw new InvalidOperationException($"{GetType()} does not support the type '{typeof(TValue)}'.");
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            //if(Datasource.Count() == 1)
            //{
            //    CurrentValue = (TValue)Convert.ChangeType(Datasource.First()?.Value, typeof(TValue));
            //}
            DotNetRef = DotNetObjectReference.Create(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("multiSelect2Component.init", Id, ModalId);
                await JSRuntime.InvokeVoidAsync("multiSelect2Component.onChange", Id, DotNetRef, "Change_SelectWithFilterBase");
            }
        }

        [JSInvokable("Change_SelectWithFilterBase")]
        public void Change(string value, string key = null)
        {
            logger.LogWarning($"{Id}-{value}");
            try
            {
                if (string.IsNullOrWhiteSpace(value) || value.Contains("Select a"))
                {
                    Value = default;
                    ValueChanged.InvokeAsync(Value);
                }
                else
                {
                    TValue temp;
                    if (Nullable.GetUnderlyingType(typeof(TValue)) != null)
                    {
                        temp = (TValue)Convert.ChangeType(value, Nullable.GetUnderlyingType(typeof(TValue)));
                    }
                    else
                    {
                        temp = (TValue)Convert.ChangeType(value, typeof(TValue));
                    }
                    if (!(Value?.Equals(temp)).GetValueOrDefault())
                    {
                        Value = temp;
                        ValueChanged.InvokeAsync(Value);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
            }
        }
    }
}
