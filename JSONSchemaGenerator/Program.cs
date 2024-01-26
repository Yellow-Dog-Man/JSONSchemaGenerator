using SkyFrost.Base;
using FrooxEngine.Headless;
using NJsonSchema.Generation;
using NJsonSchema.NewtonsoftJson.Generation;
using System;
using System.IO;
using System.Text.Json;

namespace JSONSchemaGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;


            //TODO: If we unify System.Text.Json and Newtonsoft here we can get better schemas
            string outputPath = projectDirectory + "/output/";
            Directory.CreateDirectory(outputPath);

            Type[] systemJsonTypes = { typeof(AppConfig) };
            var systemJsonSettings = new SystemTextJsonSchemaGeneratorSettings
            {
                SerializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                },
                // VSCode hates me?
                FlattenInheritanceHierarchy = true
            };

            var systemGenerator = new JsonSchemaGenerator(systemJsonSettings);

            foreach (Type type in systemJsonTypes)
            {
                File.WriteAllText($"{outputPath}{type.Name}.schema.json", systemGenerator.Generate(type).ToJson());
            }

            var newtonJsonSettings = new NewtonsoftJsonSchemaGeneratorSettings
            {
                FlattenInheritanceHierarchy = true
            };

            // VSCode hates me?
            newtonJsonSettings.FlattenInheritanceHierarchy = true;
            var newtonGenerator = new JsonSchemaGenerator(newtonJsonSettings);

            Type[] newtonJsonTypes = { typeof(HeadlessConfig) };

            foreach (Type type in newtonJsonTypes)
            {
                File.WriteAllText($"{outputPath}{type.Name}.schema.json", newtonGenerator.Generate(type).ToJson());
            }
        }
    }
}
