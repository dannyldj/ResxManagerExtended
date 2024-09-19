using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using ResxManagerExtended.Shared.Constants;
using ResxManagerExtended.Shared.Properties;

namespace ResxManagerExtended.Shared.Components.Pages;

public partial class Home
{
    [Inject] public required IStringLocalizer<Resources> Loc { private get; init; }

    [Inject] public required IMessageService MessageService { private get; init; }

    protected override void OnInitialized()
    {
        if (OperatingSystem.IsBrowser() is false) return;

        MessageService.ShowMessageBar(options =>
        {
            options.Section = MessageBarSection.SectionTop;
            options.Body = Loc["NoticeWeb"];
            options.Intent = MessageIntent.Warning;
            options.Link = new ActionLink<Message>
            {
                Text = Loc["LearnMore"],
                Href = "https://github.com/dannyldj/ResxManagerExtended/releases"
            };
        });
    }
}