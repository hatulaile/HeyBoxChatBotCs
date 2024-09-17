namespace HeyBoxChatBotCs.Api.Features;

public static class MessageAck
{
    private static ushort Serial { get; set; } = ushort.MinValue;
    private static readonly object GetLock = new();

    public static string GetAckId()
    {
        string serial;
        lock (GetLock)
        {
            if (Serial == ushort.MaxValue)
            {
                Serial = ushort.MinValue;
            }

            serial = Serial.ToString();
            Serial++;
        }

        return serial;
    }
}