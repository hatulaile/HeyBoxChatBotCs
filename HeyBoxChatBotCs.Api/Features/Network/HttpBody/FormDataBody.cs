using System.Text;

namespace HeyBoxChatBotCs.Api.Features.Network.HttpBody;

public class FormDataBody : IHttpBody
{
    public FormDataBody(params FormDataBodyItemBase[] items)
    {
        Items = items;
    }

    public FormDataBodyItemBase[] Items { get; init; }

    public List<FormDataBodyItemBase> ItemList => Items.ToList();

    public HttpContent GetContent()
    {
        var formDataContent = new MultipartFormDataContent();
        foreach (FormDataBodyItemBase item in Items)
        {
            HttpContent? content = item.GetContent();
            if (content is null)
            {
                continue;
            }

            if (!string.IsNullOrEmpty(item.FileName))
            {
                formDataContent.Add(content, item.Name, item.FileName);
            }
            else
            {
                formDataContent.Add(content, item.Name);
            }
        }

        return formDataContent;
    }
}

public enum FormDataBodyItemType : byte
{
    Txt,
    File
}

public abstract class FormDataBodyItemBase
{
    public FormDataBodyItemBase(string name, string value, FormDataBodyItemType type, string fileName = "")
    {
        Name = name;
        Value = value;
        Type = type;
        FileName = fileName;
    }

    public string Name { get; init; }

    public string FileName { get; init; }
    public string Value { get; init; }
    public FormDataBodyItemType Type { get; init; }

    public abstract HttpContent? GetContent();
}

public class FormDataBodyFile : FormDataBodyItemBase
{
    public FormDataBodyFile(string name, string value, string fileName = "") : base(name, value,
        FormDataBodyItemType.File,
        fileName)
    {
    }

    public override HttpContent? GetContent()
    {
        return !File.Exists(Value) ? null : new ByteArrayContent(File.ReadAllBytes(Value));
    }
}

public class FormDataBodyTxt : FormDataBodyItemBase
{
    public FormDataBodyTxt(string name, string value, string content = "application/json", Encoding? encoding = null) :
        base(name, value,
            FormDataBodyItemType.Txt)
    {
        Content = content;
        Encoding = encoding ?? Encoding.UTF8;
    }

    public string Content { get; init; }
    public Encoding Encoding { get; init; }

    public override HttpContent GetContent()
    {
        return new StringContent(Value, Encoding, Content);
    }
}