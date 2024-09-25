namespace HeyBoxChatBotCs.Api.Enums;

public enum MessageType
{
    Image = 3,

    [Obsolete($"建议直接使用 {nameof(MarkdownPing)}")]
    Markdown = 4,
    MarkdownPing = 10
}