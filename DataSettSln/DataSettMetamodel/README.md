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
        +BusinessDomain? Hierarchy
        +string? Name
        +IList~BusinessObject~? BusinessObjects
        +IList~string~ BusinessObjectIds
        +IList~BusinessObjectRelation~? BusinessRelations
        +BusinessDomain()
        +BusinessDomain(string name)
    }

    class BusinessObject {
        +string? Id
        +string? Name
        +IList~AttributeSet~? AttributeSets
        +IList~string~ AttributeSetIds
        +BusinessObject()
        +BusinessObject(string name, BusinessDomain? businessDomain)
    }

    class AttributeSet {
        +string? Id
        +string? Name
        +BusinessObject? BusinessObject
        +string? BusinessObjectId
        +AttributeSet()
        +AttributeSet(string name, BusinessObject businessObject)
    }

    class AttributeSetMapping {
        +string? AttributeSetId
        +string? SourceInterfaceId
        +int? OrderNo
        +string? SourceAttributeName
        +SourceAttribute? SourceAttribute
        +HistoryType? HistoryType
        +SourceAttributeRole? Role
        +string? Relation
        +BusinessObjectRelation? RelatedRelation
        +int? Position
        +string? Default
        +bool? Nullable
        +string? Datatype
        +int? Length
        +AttributeSetMapping()
        +AttributeSetMapping(int orderNo, SourceInterface sourceInterface, SourceAttribute sourceAttribute)
    }

    class BusinessObjectRelation {
        +string? Name
        +IList~BusinessObjectRelationItem~? RelatedKeys
        +BusinessObjectRelation()
        +BusinessObjectRelation(string name)
    }

    class BusinessObjectRelationItem {
        +BusinessObject? RelatedKey
        +string? Parent
        +string? RelatedKeyId
        +bool? IsLeadingKey
        +BusinessObjectRelationItem()
        +BusinessObjectRelationItem(BusinessObject relatedKey, BusinessObjectRelation relation = null, bool isLeadingKey = false)
    }

    class HistoryType {
        +string? Name
    }

    class Transformation {
        +string? SourceInterfaceId
        +string? SourceAttributeName
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
        +IList~SourceInterface~? SourceInterfaces
    }

    class SourceInterface {
        +string? SourceInterfaceId
        +string? SourceSystemId
        +string? Schema
        +string? Catalog
        +string? Name
        +IList~SourceAttribute~? SourceAttributes
        +IList~SourceAttributeRelation~? SourceAttributeRelations
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
        +string? Transformation
        +SourceAttributeRole? Role
        +AttributeSet? AttributeSet
        +BusinessObjectRelation? Relation
        +BusinessObject? RelatedBusinessObject
        +SourceAttribute()
    }

    class SourceAttributeRelation {
        +string? Name
        +SourceAttributeRelationType? RelationType
        +int? Order
        +string? LocalKey
        +string? ParentTable
        +string? ParentKey
        +SourceAttributeRelation()
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
    BusinessDomain "1" --> "*" BusinessObject : contains
    BusinessDomain "1" --> "*" BusinessObjectRelation : contains
    BusinessDomain "1" --> "0..1" BusinessDomain : hierarchy
    
    BusinessObject "1" --> "*" AttributeSet : contains
    BusinessObject "1" --> "*" BusinessObjectRelationItem : "related in"
    
    AttributeSet "1" --> "*" AttributeSetMapping : "mapped by"
    
    AttributeSetMapping "*" --> "1" SourceAttribute : "maps from"
    AttributeSetMapping "*" --> "0..1" BusinessObjectRelation : "relates to"
    AttributeSetMapping "*" --> "0..1" HistoryType : "has history type"
    
    BusinessObjectRelation "1" --> "*" BusinessObjectRelationItem : contains
    
    SourceSystem "1" --> "*" SourceInterface : contains
    SourceInterface "1" --> "*" SourceAttribute : contains
    SourceInterface "1" --> "*" SourceAttributeRelation : contains
    
    SourceAttribute "*" --> "1" SourceAttributeRole : "has role"
    SourceAttribute "*" --> "0..1" AttributeSet : "mapped to"
    SourceAttribute "*" --> "0..1" BusinessObjectRelation : "participates in"
    SourceAttribute "*" --> "0..1" BusinessObject : "related to"
    
    SourceAttributeRelation "*" --> "1" SourceAttributeRelationType : "has type"
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