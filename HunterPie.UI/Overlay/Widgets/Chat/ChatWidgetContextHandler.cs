using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using HunterPie.UI.Overlay.Widgets.Chat.Views;
using System;

namespace HunterPie.UI.Overlay.Widgets.Chat;

public class ChatWidgetContextHandler : IContextHandler
{
    private readonly ChatView _view;
    private readonly ChatViewModel _viewModel;
    private readonly Context _context;
    public ChatCategoryViewModel General { get; } = new()
    {
        Name = "General",
        Description = "General chat",
        Icon = Resources.Icon("ICON_STAR")
    };

    public ChatWidgetContextHandler(Context context)
    {
        _context = context;
        _view = new ChatView(context.Process.Type switch
        {
            GameProcessType.MonsterHunterRise => ClientConfig.Config.Rise.Overlay.ChatWidget,
            _ => throw new NotImplementedException()
        });
        _viewModel = _view.ViewModel;

        _ = WidgetManager.Register<ChatView, ChatWidgetConfig>(_view);

        UpdateData();
        HookEvents();
    }
    private void UpdateData()
    {
        _viewModel.IsChatOpen = _context.Game.Chat.IsChatOpen;
        _view.Type = _viewModel.IsChatOpen
            ? WidgetType.Window
            : WidgetType.ClickThrough;

        _viewModel.Categories.Add(General);

        foreach (IChatMessage message in _context.Game.Chat.Messages)
            _view.Dispatcher.Invoke(() =>
            {
                if (message.Type != AuthorType.Player)
                    return;

                General.Elements.Add(new ChatElementViewModel()
                {
                    Author = message.Author,
                    Text = message.Message,
                    Index = message.PlayerSlot
                });
            });
    }

    public void HookEvents()
    {
        _context.Game.Chat.OnNewChatMessage += OnNewChatMessage;
        _context.Game.Chat.OnChatOpen += OnChatOpen;
    }

    private void OnChatOpen(object sender, IChat e)
    {
        _viewModel.IsChatOpen = e.IsChatOpen;
        _view.Type = e.IsChatOpen
            ? WidgetType.Window
            : WidgetType.ClickThrough;
    }

    public void UnhookEvents()
    {
        _context.Game.Chat.OnNewChatMessage -= OnNewChatMessage;
        _context.Game.Chat.OnChatOpen -= OnChatOpen;

        _ = WidgetManager.Unregister<ChatView, ChatWidgetConfig>(_view);
    }
    private void OnNewChatMessage(object sender, IChatMessage e)
    {
        _view.Dispatcher.Invoke(() =>
        {
            if (e.Type != AuthorType.Player)
                return;

            General.Elements.Add(new ChatElementViewModel()
            {
                Author = e.Author,
                Text = e.Message,
                Index = e.PlayerSlot
            });
        });
    }
}