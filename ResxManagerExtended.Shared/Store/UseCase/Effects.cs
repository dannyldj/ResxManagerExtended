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
        if (action.Regex is null)
        {
            return;
        }

        await settingService.SetOptionAsStringAsync(SettingKeys.ResourceRegexKey, action.Regex);
        dispatcher.Dispatch(new RegexResultAction(action.Regex));
    }

    [EffectMethod(typeof(ImportAction))]
    public async Task HandleImportAction(IDispatcher dispatcher)
    {
        // 리소스 파일 접근이 실패해도 처리 상태는 반드시 해제한다.
        // 해제되지 않으면 오버레이가 남고 이후 상태 변경에서 그리드가 갱신되지 않는다.
        try
        {
            var imported = resourceService.ImportResources();
            var resources = resourceState.Value.Resources?.ToDictionary(e => e.GetResourcePath(), e => e);

            if (imported is not null)
            {
                await foreach (var resource in imported)
                {
                    if (resources?.TryGetValue(resource.Path, out var file) is true)
                    {
                        await file.SetValue(resource.Key, resource.Columns);
                    }
                }
            }
        }
        finally
        {
            dispatcher.Dispatch(new ProcessDoneAction());
        }
    }

    [EffectMethod]
    public async Task HandleEditResourceAction(EditResourceAction action, IDispatcher dispatcher)
    {
        try
        {
            var resources = resourceState.Value.Resources?.ToDictionary(e => e.GetResourcePath(), e => e);

            if (resources?.TryGetValue(action.Resource.Path, out var file) is true)
            {
                await file.SetValue(action.Resource.Key, action.Culture, action.Value);
            }
        }
        finally
        {
            dispatcher.Dispatch(new ProcessDoneAction());
        }
    }

    [EffectMethod]
    public async Task HandleDeleteResourcesAction(DeleteResourcesAction action, IDispatcher dispatcher)
    {
        try
        {
            var resources = resourceState.Value.Resources?.ToDictionary(e => e.GetResourcePath(), e => e);

            foreach (var group in action.Resources.GroupBy(e => e.Path))
            {
                if (resources?.TryGetValue(group.Key, out var file) is not true)
                {
                    continue;
                }

                await file.DeleteValues([..group.Select(e => e.Key).Distinct()]);
            }
        }
        finally
        {
            dispatcher.Dispatch(new ProcessDoneAction());
        }
    }

    [EffectMethod]
    public async Task HandleExportAction(ExportAction action, IDispatcher dispatcher)
    {
        try
        {
            await resourceService.ExportResources(action.Cultures, action.Resources);
        }
        finally
        {
            dispatcher.Dispatch(new ProcessDoneAction());
        }
    }
}
