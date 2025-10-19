# DataSettMetamodel

The DataSettMetamodel project contains the core data models for the DataSett system, providing a comprehensive metamodel for business objects and their relationships to physical source systems.

## Overview

This project defines two main model categories:

1. **Logical Business Object Model** - Represents business concepts and their relationships
2. **Physical Source System Model** - Represents the actual data sources and their structure

## Architecture

The metamodel follows a layered architecture where business concepts are mapped to physical data sources through attribute set mappings, enabling data integration and transformation scenarios.

## Class Structure

```mermaid
classDiagram
namespace LogicalBusinessObjectModel {

    class BusinessDomain {
        +string? Name
    }

    class BusinessObject {
        +string? Id
        +string? Name
    }

    class AttributeSet {
        +string? Id
        +string? Name
    }

    class AttributeSetMapping {
        +int? OrderNo
        +string? SourceAttributeName
        +string? Relation
        +int? Position
        +string? Default
        +bool? Nullable
        +string? Datatype
        +int? Length
    }

    class BusinessObjectRelation {
        +string? Name
    }

    class BusinessObjectRelationItem {
        +bool? IsLeadingKey
    }

    class HistoryType {
        +string? Name
    }

    class Transformation {
        +string? TransformationValue
    }
}

namespace PhysicalSourceSystemModel {

    class SourceSystem {
        +string? Driver
        +string? ConnectionString
        +string? SourceSystemId
        +string? Server
        +string? Name
        +string? Version
    }

    class SourceInterface {
        +string? SourceInterfaceId
        +string? Schema
        +string? Catalog
        +string? Name
    }

    class SourceAttribute {
        +string? Name
        +bool? IsPk
        +bool? IsFk
        +int? Position
        +string? Default
        +bool? Nullable
        +string? Datatype
        +int? Length
        +int? Precision
    }

    class SourceAttributeRelation {
        +string? Name
        +int? Order
        +string? LocalKey
        +string? ParentTable
        +string? ParentKey
    }

    class SourceAttributeRole {
        <<enumeration>>
        Unclassified
        BusinessKey
        Descriptive
        SelfReferencedBusinessKey
    }

    class SourceAttributeRelationType {
        <<enumeration>>
        Undefined
        ForeignKeyConstraint
    }
}

    %% Relationships
    BusinessDomain "1" --> "0..1" BusinessDomain : hierarchy
    BusinessDomain "1" --> "*" BusinessObject : contains
    BusinessDomain "1" --> "*" BusinessObjectRelation : defines
    
    BusinessObject "1" --> "*" AttributeSet : contains
    
    AttributeSet "1" --> "*" AttributeSetMapping : "mapped by"
    
    AttributeSetMapping "*" --> "1" AttributeSet : "maps to"
    AttributeSetMapping "*" --> "1" SourceInterface : "maps from"
    AttributeSetMapping "*" --> "0..1" HistoryType : "has history type"
    AttributeSetMapping "*" --> "1" SourceAttributeRole : "has role"
    
    BusinessObjectRelation "1" --> "*" BusinessObjectRelationItem : contains
    BusinessObjectRelationItem "*" --> "1" BusinessObject : "relates to"
    
    SourceSystem "1" --> "*" SourceInterface : contains
    SourceInterface "1" --> "*" SourceAttribute : contains
    SourceInterface "1" --> "*" SourceAttributeRelation : contains
    
    SourceAttributeRelation "*" --> "1" SourceAttributeRelationType : "has type"
    
    Transformation "*" --> "1" SourceInterface : "transforms from"
    Transformation "*" --> "1" SourceAttribute : "transforms"
```

## Key Concepts

### Logical Business Object Model

- **BusinessDomain**: Represents a business domain that can contain business objects and relations. Supports hierarchical structure.
- **BusinessObject**: Represents a business entity with a collection of attribute sets.
- **AttributeSet**: Groups related attributes that belong to a business object.
- **AttributeSetMapping**: Maps business attributes to physical source attributes, enabling data integration.
- **BusinessObjectRelation**: Defines relationships between business objects.
- **BusinessObjectRelationItem**: Represents individual items in a business object relationship.
- **HistoryType**: Defines how historical data should be handled.
- **Transformation**: Defines data transformations applied during mapping.

### Physical Source System Model

- **SourceSystem**: Represents a physical data source system with connection details.
- **SourceInterface**: Represents a data interface (table, view, etc.) within a source system.
- **SourceAttribute**: Represents individual attributes/columns in source interfaces.
- **SourceAttributeRelation**: Defines relationships between source attributes (foreign keys, etc.).
- **SourceAttributeRole**: Enumeration defining the role of source attributes in business context.
- **SourceAttributeRelationType**: Enumeration defining types of relationships between source attributes.

## Usage

This metamodel serves as the foundation for:

- Data modeling and business object definition
- Data source discovery and cataloging
- Data mapping and transformation definition
- Data lineage and impact analysis
- Metadata management and governance

## JSON Serialization

All classes are designed for JSON serialization using `System.Text.Json` with appropriate attributes for property naming and serialization control.