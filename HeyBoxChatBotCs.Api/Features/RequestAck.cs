namespace HeyBoxChatBotCs.Api.Features;

public static class RequestAck
{
    private static readonly object GetLock = new();
    private static uint Serial { get; set; } = uint.MinValue;

    public static string GetAckId()
    {
        string serial;
        lock (GetLock)
        {
            if (Serial == int.MaxValue)
            {
                Serial = uint.MinValue;
            }

            serial = Serial.ToString();
            Serial++;
        }

        return serial;
    }
}