using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HighBoard.Core.Constants;

public class JsonConstants
{
    /// <summary>
    /// Json serileştirme ve deserileştirme işlemlerinin seçenekleri
    /// </summary>
    public static JsonSerializerOptions DefaultSerializerOptions => new()
    {
        // Döngüye neden olan gezinti özelliklerini yoksay
        ReferenceHandler = ReferenceHandler.IgnoreCycles,

        // girintile
        WriteIndented = true,

        // özellik adları büyük/küçük harfe duyarsız 
        PropertyNameCaseInsensitive = false,

        // özellik adları camelCaseFormatinda
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,

        // Kodlanan kod konusunda daha az katı olan yerleşik bir JavaScript kodlayıcı örneği. , , >& gibi <HTML duyarlı karakterlerden kaçmıyor.
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };
}