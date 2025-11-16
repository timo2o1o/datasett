# DataSettMetamodelSerde

This project provides JSON serialization and deserialization capabilities for the DataSettMetamodel.

## Overview

The DataSettMetamodelSerde library enables reading and writing of DataSettMetamodel entities to and from JSON files, supporting the persistence layer for the DataSett application.

## Key Components

### JsonDefaults

The `JsonDefaults` class provides a shared, reusable `JsonSerializerOptions` instance that ensures consistent JSON serialization across the entire library.

#### Usage

```csharp
using DataSett.Metamodel.Serde;
using System.Text.Json;

// Serialize with camelCase naming
var json = JsonSerializer.Serialize(myObject, JsonDefaults.Web);

// Deserialize with camelCase naming
var obj = JsonSerializer.Deserialize<MyType>(json, JsonDefaults.Web);
```

#### Configuration

The `JsonDefaults.Web` property provides a `JsonSerializerOptions` instance configured with:

- **Base Configuration**: `JsonSerializerDefaults.Web` - Provides sensible defaults for web scenarios
- **Property Naming**: `JsonNamingPolicy.CamelCase` - All property names are serialized in camelCase
- **Dictionary Key Naming**: `JsonNamingPolicy.CamelCase` - Dictionary keys are also serialized in camelCase

This configuration ensures that all JSON output uses consistent camelCase naming conventions, making the JSON more idiomatic for web APIs and JavaScript interoperability.

#### Benefits of Shared Options

- **Consistency**: All serialization operations use the same naming conventions
- **Performance**: A single `JsonSerializerOptions` instance is created and reused, avoiding repeated allocations
- **Maintainability**: Serialization settings are centralized in one location
- **Reduced Errors**: No risk of inconsistent naming from ad-hoc options construction

### JsonContext

The `JsonContext` class manages loading and converting between DTOs and domain entities.

#### Key Methods

- **`LoadAsync(string repositoryPath)`**: Asynchronously loads all SourceSystem and SourceInterface DTOs from JSON files in the specified repository path
- **`GetSourceSystems()`**: Converts loaded DTOs to domain entities with navigation properties properly hydrated

#### File Naming Convention

The loader expects JSON files to follow these naming patterns:
- Source Systems: `SourceSystem_*.json`
- Source Interfaces: `SourceInterface_*.json`

All deserialization operations use `JsonDefaults.Web` to ensure consistent camelCase handling.

## JSON Serialization

### Naming Convention

All JSON properties use **camelCase** naming (e.g., `sourceSystemId`, `businessObjectName`). The C# properties use PascalCase (e.g., `SourceSystemId`, `BusinessObjectName`), and the conversion is handled automatically by the `JsonDefaults.Web` options.

### Property Name Attributes

**Note**: The metamodel classes no longer use `[JsonPropertyName]` attributes. The global camelCase naming policy in `JsonDefaults.Web` handles all property name conversions automatically. This approach:

- Reduces code duplication and maintenance burden
- Ensures consistent naming across all types
- Makes the code cleaner and easier to read

If you need to override the global naming policy for a specific property, you can still use `[JsonPropertyName("customName")]`, but this is discouraged to maintain consistency.

### Example JSON Output

**SourceSystem JSON** (`SourceSystem_Example.json`):
```json
{
  "name": "ProductionDB",
  "driver": "SQL Server",
  "server": "prod-sql-01",
  "connectionString": "Server=prod-sql-01;Database=ProductionDB;",
  "version": "2019"
}
```

**SourceInterface JSON** (`SourceInterface_Example.json`):
```json
{
  "sourceSystemId": "ProductionDB",
  "schema": "dbo",
  "name": "Customers",
  "catalog": "ProductionDB",
  "sourceAttributes": [
    {
      "name": "CustomerId",
      "isPk": true,
      "isFk": false,
      "position": 1,
      "nullable": false,
      "datatype": "int"
    },
    {
      "name": "CustomerName",
      "isPk": false,
      "isFk": false,
      "position": 2,
      "nullable": false,
      "datatype": "nvarchar",
      "length": 100
    }
  ]
}
```

## Testing

The project includes comprehensive tests in `DataSettMetamodelSerde.Tests` that validate:

- Correct camelCase serialization for all DTO types
- Round-trip serialization/deserialization preserves data
- Consistent naming conventions across all types

Run tests with:
```bash
dotnet test DataSettMetamodelSerde.Tests
```

## Integration with DataSettMetamodel

This library works in conjunction with the [DataSettMetamodel](../DataSettMetamodel/README.md) project, which defines the Base/DTO/Domain pattern used for all entities.

### Serialization Flow

1. **Domain Entity → DTO**: Domain entities are converted to DTOs using `ToDTO()` methods
2. **DTO → JSON**: DTOs are serialized to JSON using `JsonSerializer.Serialize(dto, JsonDefaults.Web)`
3. **JSON → DTO**: JSON files are deserialized to DTOs using `JsonSerializer.Deserialize<DTO>(json, JsonDefaults.Web)`
4. **DTO → Domain Entity**: DTOs are converted to domain entities using `FromDTO()` static methods

This separation ensures clean serialization without circular references while maintaining rich domain models with navigation properties.
