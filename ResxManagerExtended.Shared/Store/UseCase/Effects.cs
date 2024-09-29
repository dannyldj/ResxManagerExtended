using Fluxor;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Services;

namespace ResxManagerExtended.Shared.Store.UseCase;

public class Effects(
    IResourceService resourceService,
    ISettingService settingService,
    IState<ResourceState> resourceState)
{
    [EffectMethod(typeof(GetRootAction))]
    public async Task HandleGetRootAction(IDispatcher dispatcher)
    {
        var nodes = await resourceService.SetNodes();
        dispatcher.Dispatch(new RootResultAction(nodes));
    }

    [EffectMethod(typeof(GetRegexAction))]
    public async Task HandleGetRegexAction(IDispatcher dispatcher)
    {
        var regex = await settingService.GetOptionAsStringAsync(SettingKeys.ResourceRegexKey);
        dispatcher.Dispatch(new RegexResultAction(regex ?? DefaultSettings.DefaultResxRegex));
    }

    [EffectMethod]
    public async Task HandleSetRegexAction(SetRegexAction action, IDispatcher dispatcher)
    {
        if (action.Regex is null) return;

        await settingService.SetOptionAsStringAsync(SettingKeys.ResourceRegexKey, action.Regex);
        dispatcher.Dispatch(new RegexResultAction(action.Regex));
    }

    [EffectMethod(typeof(ImportAction))]
    public async Task HandleImportAction(IDispatcher dispatcher)
    {
        var imported = resourceService.ImportResources();
        var resources = resourceState.Value.Resources?.ToDictionary(e => e.GetResourcePath(), e => e);

        if (imported is not null)
            await foreach (var resource in imported)
            {
                if (resources?.TryGetValue(resource.Path, out var file) is true)
                    await file.SetValue(resource.Key, resource.Columns);
            }

        dispatcher.Dispatch(new ProcessDoneAction());
    }

    [EffectMethod]
    public async Task HandleExportAction(ExportAction action, IDispatcher dispatcher)
    {
        await resourceService.ExportResources(action.Cultures, action.Resources);
        dispatcher.Dispatch(new ProcessDoneAction());
    }
}