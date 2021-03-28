using MudBlazor;

namespace ComicsLibrary.Blazor.Services
{
    public class Messenger : IMessenger
    {
        private readonly ISnackbar _snackbar;

        public Messenger(ISnackbar snackbar)
        {
            _snackbar = snackbar;
        }

        public void DisplayErrorAlert(string message)
        {
            _snackbar.Add(message, Severity.Error, opt =>
            {
                opt.SnackbarVariant = Variant.Filled;
                opt.VisibleStateDuration = 3000;
            });
        }

        public void DisplaySuccessAlert(string message)
        {
            _snackbar.Add(message, Severity.Success, opt =>
            {
                opt.SnackbarVariant = Variant.Filled;
                opt.VisibleStateDuration = 3000;
            });
        }
    }
}
