﻿using System.Collections.Immutable;
using System.Globalization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Data;

namespace ResxManagerExtended.Shared.Services;

public interface IResourceService
{
    Task<IEnumerable<ITreeViewItem>?> SetNodes();

    IAsyncEnumerable<ResourceView>? ImportResources();

    Task ExportResources(ImmutableArray<CultureInfo> cultures, IEnumerable<ResourceView> resources,
        CancellationToken token = default);
}