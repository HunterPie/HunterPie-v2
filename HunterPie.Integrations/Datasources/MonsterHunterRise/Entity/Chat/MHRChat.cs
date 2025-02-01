using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Integrations.Datasources.Common.Entity.Chat;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Chat;

public class MHRChat : CommonChat
{
    private bool _isChatOpen;
    private readonly Dictionary<nint, MHRChatMessage> _messages = new();

    public override List<IChatMessage> Messages => _messages.Values.ToList<IChatMessage>();

    public override bool IsChatOpen
    {
        get => _isChatOpen;
        protected set
        {
            if (value != _isChatOpen)
            {
                _isChatOpen = value;
                this.Dispatch(_onChatOpen, this);
            }
        }
    }

    internal bool ContainsMessage(nint messageAddress) => _messages.ContainsKey(messageAddress);

    internal void AddMessage(nint messageAddress, MHRChatMessage message)
    {
        if (_messages.ContainsKey(messageAddress))
            return;

        _messages.Add(messageAddress, message);

        this.Dispatch(_onNewChatMessage, message);
    }

    internal void SetChatState(bool isOpen) => IsChatOpen = isOpen;
}