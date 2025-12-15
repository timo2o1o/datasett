# Base/DTO/Domain Pattern Architecture

## Overview

This document explains the Base/DTO/Domain separation pattern implemented in the DataSettMetamodel project. This pattern enables clean serialization of complex object graphs to separate JSON files using ID references instead of full object graphs.

## Pattern Structure

Each entity in the metamodel follows a three-tier class hierarchy:

1. **Base Class** (`[EntityName]Base`) - Contains scalar/context properties and value objects
2. **DTO Class** (`[EntityName]DTO`) - Inherits from base, adds foreign key references (string IDs) for serialization
3. **Domain Class** (`[EntityName]`) - Inherits from base, adds navigation properties and business logic

## Purpose and Benefits

### Why This Pattern?

- **Separation of Concerns**: Clear distinction between data structure (base), serialization (DTO), and business logic (domain)
- **Serialization Control**: DTOs can be serialized to separate JSON files with ID references instead of full object graphs, avoiding circular references
- **Maintainability**: Changes to business logic don't affect serialization format
- **Testability**: Each layer can be tested independently
- **Backward Compatibility**: Domain classes maintain compatibility with existing ViewModels

## Layer Details

### Base Classes (`[EntityName]Base`)

Base classes contain scalar properties, value objects, and simple collections:
- Primitive types (string, int, bool, etc.)
- Enumerations
- Value objects (e.g., `AttributeProperties`)
- Simple collections of value objects (e.g., `IList<SourceAttribute>` in `SourceInterfaceBase`)
- No navigation properties to other domain entities
- No foreign key ID references

**Examples:**

```csharp
// Simple base with only scalar properties
public abstract class BusinessDomainBase
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}

// Base with scalar properties and value object
public abstract class BusinessConceptKeyPartBase
{
    public string? Name { get; set; }
    public AttributeProperties KeyProperties { get; set; }
}

// Base with scalar properties and embedded collection
public abstract class SourceInterfaceBase
{
    public string? Schema { get; set; }
    public string? Catalog { get; set; }
    public string? Name { get; set; }
    public IList<SourceAttribute>? SourceAttributes { get; set; }
}
```

### DTO Classes (`[EntityName]DTO`)

DTO classes inherit from base classes and add:
- Foreign key references (string IDs) to related entities
- Collections of child DTOs (not domain objects)
- Computed ID properties (often marked with `[JsonIgnore]`)
- Parameterless constructor for JSON deserialization
- No navigation properties to domain entities

**Key Characteristics:**
- All relationships are represented as string IDs
- Child collections use DTO types (e.g., `IList<BusinessConceptKeyPartDTO>`)
- ID properties are typically computed from Name or other properties
- IDs are often marked `[JsonIgnore]` to exclude them from serialization

**Examples:**

```csharp
// DTO with computed ID and parent reference
public class BusinessDomainDTO : BusinessDomainBase
{
    [JsonIgnore]
    public string? BusinessDomainId
    {
        get { return Name; }
    }

    public string? ParentBusinessDomainId { get; set; }
}

// DTO with computed composite ID and parent reference
public class BusinessConceptDTO : BusinessConceptBase
{
    public BusinessConceptDTO()
    {
        KeyParts = new List<BusinessConceptKeyPartDTO>();
    }

    [JsonIgnore]
    public string BusinessConceptId
    {
        get { return string.Format("{0}.{1}", BusinessDomainId, Name); }
    }

    public IList<BusinessConceptKeyPartDTO> KeyParts { get; set; }
    public string? BusinessDomainId { get; set; }
}

// Nested DTO with computed ID
public class BusinessConceptKeyPartDTO : BusinessConceptKeyPartBase
{
    [JsonIgnore]
    public string? BusinessConceptKeyPartId
    {
        get { return $"{BusinessConceptId}.{Name}"; }
    }

    [JsonIgnore]
    public string? BusinessConceptId { get; set; }
}

// DTO with child collection of DTOs
public class BusinessConceptRelationDTO : BusinessConceptRelationBase
{
    public BusinessConceptRelationDTO()
    {
        RelatedConcepts = new List<BusinessConceptRelationItemDTO>();
    }

    [JsonIgnore]
    public string? BusinessConceptRelationId
    {
        get { return Name; }
    }

    public IList<BusinessConceptRelationItemDTO> RelatedConcepts { get; set; }
}
```

### Domain Classes (`[EntityName]`)

Domain classes inherit from base classes and add:
- Navigation properties (relationships to other domain entities)
- Collections of child domain objects
- Business logic methods
- `FromDTO()` static method to create domain entity from DTO

**Key Characteristics:**
- Navigation properties are typically marked with `[JsonIgnore]`
- Child collections use domain types (e.g., `IList<BusinessConcept>`)
- Constructor initializes child collections to empty lists
- `FromDTO()` method handles conversion from DTO, navigation properties set separately

**Examples:**

```csharp
// Domain with navigation properties and child collections
public class BusinessDomain : BusinessDomainBase
{
    public BusinessDomain()
    {
        BusinessConcepts = new List<BusinessConcept>();
        ChildBusinessDomains = new List<BusinessDomain>();
    }

    [JsonIgnore]
    public BusinessDomain? ParentBusinessDomain { get; set; }

    public IList<BusinessDomain> ChildBusinessDomains { get; set; }

    [JsonIgnore]
    public IList<BusinessConcept> BusinessConcepts { get; set; }

    public static BusinessDomain FromDTO(BusinessDomainDTO dto, BusinessDomain? parentBusinessDomain)
    {
        return new BusinessDomain
        {
            Name = dto.Name,
            Description = dto.Description,
            ParentBusinessDomain = parentBusinessDomain
        };
    }
}

// Domain with navigation and child collections
public class BusinessConcept : BusinessConceptBase
{
    public BusinessConcept()
    {
        AttributeSets = new List<AttributeSet>();
        KeyParts = new List<BusinessConceptKeyPart>();
    }

    public IList<AttributeSet> AttributeSets { get; set; }
    public IList<BusinessConceptKeyPart> KeyParts { get; set; }
    
    [JsonIgnore]
    public BusinessDomain? ParentBusinessDomain { get; set; }

    public static BusinessConcept FromDTO(BusinessConceptDTO dto, BusinessDomain parentBusinessDomain)
    {
        return new BusinessConcept
        {
            Name = dto.Name,
            ParentBusinessDomain = parentBusinessDomain
        };
    }
}

// Domain with single navigation property
public class BusinessConceptKeyPart : BusinessConceptKeyPartBase
{
    public BusinessConcept? ParentBusinessConcept { get; set; }
}

// Domain with collection of domain entities
public class BusinessConceptRelation : BusinessConceptRelationBase
{
    public BusinessConceptRelation()
    {
        RelatedConcepts = new List<BusinessConceptRelationItem>();
    }

    public IList<BusinessConceptRelationItem>? RelatedConcepts { get; set; }
}
```

## Pattern Variations

### Nested Collections in DTOs

Some DTOs include collections of child DTOs directly embedded (not separate files):

```csharp
public class BusinessConceptDTO : BusinessConceptBase
{
    public IList<BusinessConceptKeyPartDTO> KeyParts { get; set; }
    // KeyParts are serialized inline with the BusinessConcept
}
```

### Value Objects in Base Classes

Base classes can contain value objects or simple collections:

```csharp
public abstract class SourceInterfaceBase
{
    public IList<SourceAttribute>? SourceAttributes { get; set; }
    // SourceAttributes are part of the base structure
}
```

### JsonIgnore on Navigation Properties

Navigation properties in domain classes are marked with `[JsonIgnore]` to prevent serialization:

```csharp
[JsonIgnore]
public BusinessDomain? ParentBusinessDomain { get; set; }
```

### Computed ID Properties

DTOs often have computed ID properties that are also marked `[JsonIgnore]`:

```csharp
[JsonIgnore]
public string BusinessConceptId
{
    get { return string.Format("{0}.{1}", BusinessDomainId, Name); }
}
```

## Serialization Flow

### DTO to JSON
1. Domain object is converted to DTO (if ToDTO() method exists, or constructed manually)
2. Navigation properties are replaced with ID references
3. DTO is serialized to JSON using System.Text.Json
4. Computed IDs (marked `[JsonIgnore]`) are excluded from JSON

### JSON to Domain
1. JSON is deserialized to DTO
2. Domain object is created using `FromDTO()` static method
3. Navigation properties are set separately by the consuming code
4. Child collections are built by iterating child DTOs

## Example JSON Structure

Given the BusinessDomain hierarchy:

```json
[
  {
    "name": "Customer Management",
    "description": "Domain for customer-related entities",
    "parentBusinessDomainId": null
  },
  {
    "name": "Order Processing",
    "description": "Domain for order-related entities",
    "parentBusinessDomainId": "Customer Management"
  }
]
```

Note: `businessDomainId` is excluded (marked `[JsonIgnore]`)

## Implementation Guidelines

### When Creating a New Entity:

1. **Create the Base class first**:
   - Include only scalar properties, enums, and value objects
   - Use abstract class
   - Inherit from no other classes (unless sharing common base)

2. **Create the DTO class**:
   - Inherit from Base
   - Add string ID properties for related entities (often `[JsonIgnore]`)
   - Add collections of child DTOs if needed
   - Provide parameterless constructor
   - Initialize child collections in constructor

3. **Create the Domain class**:
   - Inherit from Base
   - Add navigation properties (mark with `[JsonIgnore]`)
   - Add collections of child domain objects
   - Initialize collections in constructor
   - Implement `FromDTO()` static method

### Naming Conventions:

- Base: `[EntityName]Base`
- DTO: `[EntityName]DTO`
- Domain: `[EntityName]`

### ID Conventions:

- Simple IDs: Use Name property (e.g., `BusinessDomainId` ? `Name`)
- Composite IDs: Combine parent ID and name (e.g., `{BusinessDomainId}.{Name}`)
- Mark ID properties with `[JsonIgnore]` in DTOs