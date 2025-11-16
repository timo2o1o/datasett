using System.Text.Json;
using DataSett.Metamodel;
using DataSett.Metamodel.Serde;
using Xunit;

namespace DataSettMetamodelSerde.Tests;

/// <summary>
/// Tests to validate that JsonDefaults.Web correctly applies camelCase naming
/// and that serialization/deserialization works consistently.
/// </summary>
public class JsonDefaultsTests
{
    [Fact]
    public void JsonDefaults_Web_ShouldHaveCamelCaseNamingPolicy()
    {
        // Act
        var options = JsonDefaults.Web;

        // Assert
        Assert.NotNull(options.PropertyNamingPolicy);
        Assert.Equal(JsonNamingPolicy.CamelCase, options.PropertyNamingPolicy);
        Assert.Equal(JsonNamingPolicy.CamelCase, options.DictionaryKeyPolicy);
    }

    [Fact]
    public void SourceSystemDTO_Serialization_ShouldUseCamelCase()
    {
        // Arrange
        var sourceSystem = new SourceSystemDTO
        {
            Name = "TestSystem",
            Driver = "TestDriver",
            Server = "localhost",
            ConnectionString = "Server=localhost;Database=test;",
            Version = "1.0"
        };

        // Act
        var json = JsonSerializer.Serialize(sourceSystem, JsonDefaults.Web);

        // Assert
        Assert.Contains("\"name\":", json);
        Assert.Contains("\"driver\":", json);
        Assert.Contains("\"server\":", json);
        Assert.Contains("\"connectionString\":", json);
        Assert.Contains("\"version\":", json);
        
        // Verify that PascalCase keys are NOT present
        Assert.DoesNotContain("\"Name\":", json);
        Assert.DoesNotContain("\"Driver\":", json);
        Assert.DoesNotContain("\"Server\":", json);
        Assert.DoesNotContain("\"ConnectionString\":", json);
        Assert.DoesNotContain("\"Version\":", json);
    }

    [Fact]
    public void SourceSystemDTO_Deserialization_ShouldUseCamelCase()
    {
        // Arrange
        var json = "{\"name\":\"TestSystem\",\"driver\":\"TestDriver\",\"server\":\"localhost\",\"connectionString\":\"Server=localhost;Database=test;\",\"version\":\"1.0\"}";

        // Act
        var sourceSystem = JsonSerializer.Deserialize<SourceSystemDTO>(json, JsonDefaults.Web);

        // Assert
        Assert.NotNull(sourceSystem);
        Assert.Equal("TestSystem", sourceSystem.Name);
        Assert.Equal("TestDriver", sourceSystem.Driver);
        Assert.Equal("localhost", sourceSystem.Server);
        Assert.Equal("Server=localhost;Database=test;", sourceSystem.ConnectionString);
        Assert.Equal("1.0", sourceSystem.Version);
    }

    [Fact]
    public void SourceInterfaceDTO_Serialization_ShouldUseCamelCase()
    {
        // Arrange
        var sourceInterface = new SourceInterfaceDTO
        {
            Name = "TestTable",
            Schema = "dbo",
            Catalog = "TestCatalog",
            SourceSystemId = "TestSystem",
            SourceAttributes = new List<SourceAttribute>
            {
                new SourceAttribute
                {
                    Name = "Id",
                    IsPk = true,
                    Datatype = "int",
                    Position = 1,
                    Nullable = false
                }
            }
        };

        // Act
        var json = JsonSerializer.Serialize(sourceInterface, JsonDefaults.Web);

        // Assert
        Assert.Contains("\"name\":", json);
        Assert.Contains("\"schema\":", json);
        Assert.Contains("\"catalog\":", json);
        Assert.Contains("\"sourceSystemId\":", json);
        Assert.Contains("\"sourceAttributes\":", json);
        Assert.Contains("\"isPk\":", json);
        Assert.Contains("\"datatype\":", json);
        Assert.Contains("\"position\":", json);
        Assert.Contains("\"nullable\":", json);
        
        // Verify that PascalCase keys are NOT present
        Assert.DoesNotContain("\"Name\":", json);
        Assert.DoesNotContain("\"Schema\":", json);
        Assert.DoesNotContain("\"IsPk\":", json);
        Assert.DoesNotContain("\"Position\":", json);
    }

    [Fact]
    public void SourceInterfaceDTO_Deserialization_ShouldUseCamelCase()
    {
        // Arrange
        var json = "{\"name\":\"TestTable\",\"schema\":\"dbo\",\"catalog\":\"TestCatalog\",\"sourceSystemId\":\"TestSystem\",\"sourceAttributes\":[{\"name\":\"Id\",\"isPk\":true,\"isFk\":false,\"position\":1,\"nullable\":false,\"datatype\":\"int\"}]}";

        // Act
        var sourceInterface = JsonSerializer.Deserialize<SourceInterfaceDTO>(json, JsonDefaults.Web);

        // Assert
        Assert.NotNull(sourceInterface);
        Assert.Equal("TestTable", sourceInterface.Name);
        Assert.Equal("dbo", sourceInterface.Schema);
        Assert.Equal("TestCatalog", sourceInterface.Catalog);
        Assert.Equal("TestSystem", sourceInterface.SourceSystemId);
        Assert.NotNull(sourceInterface.SourceAttributes);
        Assert.Single(sourceInterface.SourceAttributes);
        Assert.Equal("Id", sourceInterface.SourceAttributes[0].Name);
        Assert.True(sourceInterface.SourceAttributes[0].IsPk);
        Assert.Equal("int", sourceInterface.SourceAttributes[0].Datatype);
    }

    [Fact]
    public void SourceAttribute_Serialization_ShouldUseCamelCase()
    {
        // Arrange
        var attribute = new SourceAttribute
        {
            Name = "TestColumn",
            IsPk = true,
            IsFk = false,
            Position = 1,
            Default = "0",
            Nullable = false,
            Datatype = "varchar",
            Length = 50,
            Precision = 0
        };

        // Act
        var json = JsonSerializer.Serialize(attribute, JsonDefaults.Web);

        // Assert
        Assert.Contains("\"name\":", json);
        Assert.Contains("\"isPk\":", json);
        Assert.Contains("\"isFk\":", json);
        Assert.Contains("\"position\":", json);
        Assert.Contains("\"default\":", json);
        Assert.Contains("\"nullable\":", json);
        Assert.Contains("\"datatype\":", json);
        Assert.Contains("\"length\":", json);
        Assert.Contains("\"precision\":", json);
    }

    [Fact]
    public void BusinessDomainDTO_Serialization_ShouldUseCamelCase()
    {
        // Arrange
        var businessDomain = new BusinessDomainDTO
        {
            Name = "TestDomain",
            ParentBusinessDomainId = "ParentDomain"
        };

        // Act
        var json = JsonSerializer.Serialize(businessDomain, JsonDefaults.Web);

        // Assert
        Assert.Contains("\"name\":", json);
        Assert.Contains("\"parentBusinessDomainId\":", json);
        
        // Verify that PascalCase keys are NOT present
        Assert.DoesNotContain("\"Name\":", json);
        Assert.DoesNotContain("\"ParentBusinessDomainId\":", json);
    }

    [Fact]
    public void AttributeSetDTO_Serialization_ShouldUseCamelCase()
    {
        // Arrange
        var attributeSet = new AttributeSetDTO
        {
            Name = "TestAttributeSet",
            BusinessObjectId = "TestBusinessObject"
        };

        // Act
        var json = JsonSerializer.Serialize(attributeSet, JsonDefaults.Web);

        // Assert
        Assert.Contains("\"name\":", json);
        Assert.Contains("\"businessObjectId\":", json);
        
        // Verify that PascalCase keys are NOT present
        Assert.DoesNotContain("\"Name\":", json);
        Assert.DoesNotContain("\"BusinessObjectId\":", json);
    }

    [Fact]
    public void AttributeSetMappingDTO_Serialization_ShouldUseCamelCase()
    {
        // Arrange
        var mapping = new AttributeSetMappingDTO
        {
            AttributeSetId = "TestAttributeSet",
            SourceInterfaceId = "TestInterface",
            SourceAttributeName = "TestAttribute",
            OrderNo = 1,
            Position = 1,
            Default = "0",
            Nullable = false,
            Datatype = "int",
            Length = 4,
            Precision = 0
        };

        // Act
        var json = JsonSerializer.Serialize(mapping, JsonDefaults.Web);

        // Assert
        Assert.Contains("\"attributeSetId\":", json);
        Assert.Contains("\"sourceInterfaceId\":", json);
        Assert.Contains("\"sourceAttributeName\":", json);
        Assert.Contains("\"orderNo\":", json);
        Assert.Contains("\"position\":", json);
        Assert.Contains("\"default\":", json);
        Assert.Contains("\"nullable\":", json);
        Assert.Contains("\"datatype\":", json);
        Assert.Contains("\"length\":", json);
        Assert.Contains("\"precision\":", json);
    }

    [Fact]
    public void Transformation_Serialization_ShouldUseCamelCase()
    {
        // Arrange
        var transformation = new Transformation
        {
            SourceInterfaceId = "TestInterface",
            SourceAttributeName = "TestAttribute",
            TransformationExpression = "UPPER(column)"
        };

        // Act
        var json = JsonSerializer.Serialize(transformation, JsonDefaults.Web);

        // Assert
        Assert.Contains("\"sourceInterfaceId\":", json);
        Assert.Contains("\"sourceAttributeName\":", json);
        Assert.Contains("\"transformationExpression\":", json);
        
        // Verify that PascalCase keys are NOT present
        Assert.DoesNotContain("\"SourceInterfaceId\":", json);
        Assert.DoesNotContain("\"SourceAttributeName\":", json);
        Assert.DoesNotContain("\"TransformationExpression\":", json);
    }

    [Fact]
    public void RoundTrip_Serialization_ShouldPreserveData()
    {
        // Arrange
        var original = new SourceSystemDTO
        {
            Name = "TestSystem",
            Driver = "SQL Server",
            Server = "localhost",
            ConnectionString = "Server=localhost;Database=test;",
            Version = "2019"
        };

        // Act
        var json = JsonSerializer.Serialize(original, JsonDefaults.Web);
        var deserialized = JsonSerializer.Deserialize<SourceSystemDTO>(json, JsonDefaults.Web);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(original.Name, deserialized.Name);
        Assert.Equal(original.Driver, deserialized.Driver);
        Assert.Equal(original.Server, deserialized.Server);
        Assert.Equal(original.ConnectionString, deserialized.ConnectionString);
        Assert.Equal(original.Version, deserialized.Version);
    }
}
