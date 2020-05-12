using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Abarnathy.BlazorClient.Client.Shared.Components.FormWizard
{
    public partial class FormWizard
    {
        private readonly List<FormWizardStep> _steps = new List<FormWizardStep>();

        [Parameter] public string Id { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public FormWizardStep ActiveStep { get; set; }
        [Parameter] public int ActiveStepIndex { get; set; }
        [Parameter] public EventCallback OnClickSubmit { get; set; }
        [Parameter] public EventCallback OnClickCancel { get; set; }
        [Parameter] public bool EnableSubmit { get; set; }

        private bool IsLastStep { get; set; }

        public int StepsIndex(FormWizardStep step) => StepsIndexInternal(step);

        private void GoBack()
        {
            if (ActiveStepIndex > 0)
                SetActive(_steps[ActiveStepIndex - 1]);
        }

        private void GoNext()
        {
            if (ActiveStepIndex < _steps.Count - 1)
                SetActive(_steps[(_steps.IndexOf(ActiveStep) + 1)]);
        }

        private void SetActive(FormWizardStep step)
        {
            ActiveStep = step ?? throw new ArgumentNullException(nameof(step));

            ActiveStepIndex = StepsIndex(step);
            if (ActiveStepIndex == _steps.Count - 1)
                IsLastStep = true;
            else
                IsLastStep = false;
        }

        private int StepsIndexInternal(FormWizardStep step)
        {
            if (step == null)
                throw new ArgumentNullException(nameof(step));

            return _steps.IndexOf(step);
        }

        protected internal void AddStep(FormWizardStep step)
        {
            _steps.Add(step);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                SetActive(_steps[0]);
                StateHasChanged();
            }
        }
    }
}