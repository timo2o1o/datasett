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
    /// <summary>
    /// Name of the business object
    /// </summary>
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
    public BusinessObjectDTO() { }

    [JsonIgnore]
    public string BusinessObjectId
    {
        get
        {
            return string.Format("{0}.{1}", BusinessDomainId, Name);
        }
    }

    /// <summary>
    /// Reference to parent business domain
    /// </summary>
    public string? BusinessDomainId { get; set; }
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