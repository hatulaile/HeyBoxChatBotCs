using System.Text.Json;
using System.Text.Json.Serialization;

namespace HeyBoxChatBotCs.Api.Converters;

/// <summary>
/// 从<see cref="bool"/>转换到数字的Json转换器
///</summary>
///<seealso href="https://github.com/gehongyan/HeyBox.Net/">参考项目</seealso>
public class NumberBooleanJsonConverter : JsonConverter<bool>
{
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetInt32() != 0;
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value ? 1 : 0);
    }
}