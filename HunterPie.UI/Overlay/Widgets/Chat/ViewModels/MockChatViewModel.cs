using HunterPie.UI.Architecture.Test;
using HunterPie.UI.Assets.Application;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

internal class MockChatViewModel : ChatViewModel
{
    public MockChatViewModel()
    {
        SolidColorBrush[] playerColors =
        {
            new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0x64, 0x91)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x64, 0xB6, 0xED)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0xED, 0xAD, 0x64)),
            new SolidColorBrush(Color.FromArgb(0xFF, 0x64, 0xED, 0x99))
        };

        string[] playerNames = { "Lyss", "Pia", "夜令", "MashiLo" };
        string[] possibleChats =
        {
            "poggies poggies wooo!",
            "uwu hi",
            "this is a test",
            "hi can someone help me",
            "deez nuts",
            "very cool",
            "subscribe to my onlyfans"
        };

        const int chatSize = 900;
        var chatElements = new ChatElementViewModel[chatSize];
        for (int i = 0; i < chatSize; i++)
        {
            var rng = new Random();
            int playerIndex = rng.Next(0, playerNames.Length);
            int chatIndex = rng.Next(0, possibleChats.Length);
            chatElements[i] = new ChatElementViewModel()
            {
                Author = playerNames[playerIndex],
                Index = playerIndex,
                Text = possibleChats[chatIndex],
            };
        }

        var general = new ChatCategoryViewModel()
        {
            Name = "General",
            Description = "General chat",
            Icon = Resources.Icon("ICON_STAR")
        };

        Categories.Add(general);

        IEnumerator enumerator = chatElements.GetEnumerator();
        MockBehavior.Run(() =>
        {
            bool success = enumerator.MoveNext();

            if (!success)
                return;

            var vm = (ChatElementViewModel)enumerator.Current;

            Application.Current.Dispatcher.Invoke(() => general.Elements.Add(vm));

        }, 1);
    }
}