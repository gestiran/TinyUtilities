using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TinyUtilities.Serialization {
    public sealed class FlexibleNameResolver : DefaultContractResolver {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization member) {
            IList<JsonProperty> properties = base.CreateProperties(type, member);
            int count = properties.Count;
            
            for (int propertyId = 0; propertyId < count; propertyId++) {
                JsonProperty property = properties[propertyId];
                
                string underscoreName = $"_{property.PropertyName}";
                
                JsonProperty underscoreProperty = new JsonProperty();
                
                underscoreProperty.PropertyName = underscoreName;
                underscoreProperty.PropertyType = property.PropertyType;
                underscoreProperty.DeclaringType = property.DeclaringType;
                underscoreProperty.ValueProvider = property.ValueProvider;
                underscoreProperty.Converter = property.Converter;
                underscoreProperty.Readable = property.Readable;
                underscoreProperty.Writable = property.Writable;
                
                properties.Add(underscoreProperty);
            }
            
            return properties;
        }
    }
}