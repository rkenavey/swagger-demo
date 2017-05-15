using System;
using SwaggerDemo.Models;
using Swashbuckle.Swagger;

namespace SwaggerDemo.Filters
{
    public class RequiredSchemaFilter : ISchemaFilter
    {
        public void Apply(Schema schema, SchemaRegistry schemaRegistry, Type type)
        {
            if (type == typeof(Player))
            {
                schema.required = new[]
                {
                    "firstName",
                    "lastName",
                };
            }
        }
    }
}
