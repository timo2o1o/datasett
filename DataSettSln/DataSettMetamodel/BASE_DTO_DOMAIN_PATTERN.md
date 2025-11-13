# Base/DTO/Domain Pattern Architecture

## Overview

This document explains the Base/DTO/Domain separation pattern implemented in the DataSettMetamodel project.

## Pattern Structure

Each entity in the metamodel follows a three-tier class hierarchy:

1. **Base Class** (`[EntityName]Base`) - Contains scalar/context properties only
2. **DTO Class** (`[EntityName]DTO`) - Inherits from base, adds foreign key references for serialization
3. **Domain Class** (`[EntityName]`) - Inherits from base, adds navigation properties and business logic

## Purpose and Benefits

### Why This Pattern?

- **Separation of Concerns**: Clear distinction between data structure (base), serialization (DTO), and business logic (domain)
- **Serialization Control**: DTOs can be serialized to separate JSON files with ID references instead of full object graphs
- **Maintainability**: Changes to business logic don't affect serialization format
- **Testability**: Each layer can be tested independently
- **Backward Compatibility**: Domain classes maintain compatibility with existing ViewModels

### Base Classes (`[EntityName]Base`)

Base classes contain only scalar and context properties:
- Primitive types (string, int, bool, etc.)
- Enumerations
- Value types
- No navigation properties
- No collections of other entities

**Example:**
```csharp
public abstract class BusinessObjectBase
{
    [JsonPropertyName("businessObjectId")]
    public string? Id { get; set; }

    [JsonPropertyName("businessObjectName")]
    public string? Name { get; set; }
}
```

### DTO Classes (`[EntityName]DTO`)

DTO classes inherit from base classes and add:
- Foreign key references (IDs) to related entities
- Properties needed for serialization
- No navigation properties (to avoid circular references during serialization)
- Parameterless constructor for JSON deserialization

**Example:**
```csharp
public class BusinessObjectDTO : BusinessObjectBase
{
    [JsonConstructor]
    public BusinessObjectDTO() { }

    [JsonPropertyName("attributeSets")]
    public IList<string> AttributeSetIds { get; set; }
}
```

### Domain Classes (`[EntityName]`)

Domain classes inherit from base classes and add:
- Navigation properties (relationships to other entities)
- Business logic methods
- Helper collections for backward compatibility
- `ToDTO()` method to convert to DTO for serialization
- `FromDTO()` static method to create domain entity from DTO

**Example:**
```csharp
public class BusinessObject : BusinessObjectBase
{
    [JsonIgnore]
    public IList<AttributeSet>? AttributeSets { get; set; }

    [JsonIgnore]
    public BusinessDomain? BusinessDomain { get; set; }

    public BusinessObjectDTO ToDTO() { /* ... */ }
    public static BusinessObject FromDTO(BusinessObjectDTO dto) { /* ... */ }
}
```

## Serialization Workflow

### Writing (Domain → DTO → JSON)

1. Create and populate domain entities with full object graph
2. Call `ToDTO()` on each entity to convert to DTO
3. Serialize DTOs to separate JSON files
4. DTOs contain ID references instead of full navigation properties

### Reading (JSON → DTO → Domain)

1. Deserialize JSON files to DTOs
2. Call `FromDTO()` to create domain entities
3. Manually wire up navigation properties using ID references
4. Result: Full domain object graph ready for business logic

## Implementation Examples

### Entities with Base/DTO/Domain Pattern

The following entities have been refactored to use this pattern:

#### Logical Business Object Model
- **BusinessDomain** - Business domain container
- **BusinessObject** - Business entity
- **AttributeSet** - Attribute grouping
- **AttributeSetMapping** - Physical-to-logical attribute mapping
- **BusinessObjectRelationItem** - Relationship item

#### Physical Source System Model
- **SourceSystem** - Physical data source
- **SourceInterface** - Table/file/endpoint within source system

### Entities Without Full Pattern

Some entities don't require the full pattern:
- **SourceAttribute** - No navigation properties, simple value object
- **BusinessObjectRelation** - Simple container, no complex navigation
- **Transformation** - Configuration object
- **Enumerations** (HistoryType, SourceAttributeRole, etc.) - No pattern needed

## Usage Guidelines

### Creating New Entities

When adding new entities to the metamodel:

1. Create base class with scalar properties
2. Create DTO class inheriting from base, add foreign keys
3. Create domain class inheriting from base, add navigation properties
4. Implement `ToDTO()` and `FromDTO()` methods
5. Mark navigation properties with `[JsonIgnore]`

### Backward Compatibility

For compatibility with existing ViewModels, domain classes may include:
- ID collections (e.g., `BusinessObjectIds`) marked with `[JsonIgnore]`
- These are NOT serialized but available for ViewModel operations

### Serialization Best Practices

1. Always use DTOs for JSON serialization
2. Each entity type should serialize to its own file(s)
3. Use ID references for relationships
4. Reconstruct navigation properties after deserialization

## JSON File Structure

Example serialization output structure:

```
metadata/
  SourceSystem_DatabaseA.json      (SourceSystemDTO)
  SourceInterface_DatabaseA.Table1.json  (SourceInterfaceDTO)
  BusinessDomain_Sales.json        (BusinessDomainDTO)
  BusinessObject_Sales.Customer.json     (BusinessObjectDTO)
  AttributeSet_Sales.Customer.Core.json  (AttributeSetDTO)
```

Each file contains serialized DTO with ID references to related entities.

## Migration Notes

When migrating existing code:

1. Existing domain classes retain their navigation properties
2. New base and DTO classes separate serialization concerns
3. ViewModels continue to work with domain classes
4. Serialization code updated to use DTOs
5. All changes are backward compatible

## Future Enhancements

Potential improvements to consider:

1. AutoMapper for DTO/Domain conversions
2. Generic base repository using DTOs
3. Validation attributes in base classes
4. Audit properties (CreatedAt, UpdatedAt) in base classes
5. Unit of Work pattern for navigation property resolution
