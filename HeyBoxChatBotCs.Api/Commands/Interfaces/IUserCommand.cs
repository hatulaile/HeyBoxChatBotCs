using HeyBoxChatBotCs.Api.Enums;

namespace HeyBoxChatBotCs.Api.Commands.Interfaces;

public interface IUserCommand : ICommand<Dictionary<string, List<KeyValuePair<CommandArgsTypeId, string>>>>;