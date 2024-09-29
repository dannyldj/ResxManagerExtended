using Microsoft.Win32;

namespace ResxManagerExtended.Desktop.Extensions;

public static class DialogExtension
{
    public static async Task<bool?> ShowDialogAsync(this CommonItemDialog dialog)
    {
        await Task.Yield();
        return dialog.ShowDialog();
    }
}