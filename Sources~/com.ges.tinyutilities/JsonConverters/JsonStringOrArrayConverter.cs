#if EXTERNAL_DEPENDENCIES
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TinyUtilities.JsonConverters {
    public sealed class JsonStringOrArrayConverter : JsonConverter<string[]> {
        public override string[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (reader.TokenType == JsonTokenType.String) {
                return new string[] { reader.GetString() };
            }
            
            if (reader.TokenType == JsonTokenType.StartArray) {
                return JsonSerializer.Deserialize<string[]>(ref reader, options);
            }
            
            if (reader.TokenType == JsonTokenType.Null) {
                return null;
            }
            
            throw new JsonException($"Unexpected token '{reader.TokenType}' for JSON Schema type field.");
        }
        
        public override void Write(Utf8JsonWriter writer, string[] value, JsonSerializerOptions options) {
            if (value == null) {
                writer.WriteNullValue();
                return;
            }
            
            if (value.Length == 1) {
                writer.WriteStringValue(value[0]);
                return;
            }
            
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
#endif