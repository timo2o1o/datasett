using System.Text.Json;
using DataSett.Metamodel;

namespace DataSett.Metamodel.Serde;

/// <summary>
/// JSON serialization and deserialization service for the DataSettMetamodel.
/// Demonstrates the Base/DTO/Domain pattern for clean separation of concerns.
/// 
/// Key Concepts:
/// - DTOs (Data Transfer Objects) are used for serialization to JSON
/// - Domain entities are used for business logic and navigation
/// - Deserialization reads DTOs then converts to domain entities
/// - Serialization converts domain entities to DTOs before writing JSON
/// </summary>
public class Json
{
    /// <summary>
    /// Deserializes SourceSystem entities from JSON files using the DTO pattern.
    /// Reads SourceSystemDTO from JSON, converts to domain entities, and wires up navigation properties.
    /// </summary>
    /// <param name="repositoryPath">Path to the directory containing JSON files</param>
    /// <returns>Enumerable of SourceSystem domain entities with navigation properties populated</returns>
    public IEnumerable<SourceSystem> Deserialize(string repositoryPath)
    {
        foreach (string sSystem in Directory.EnumerateFiles(repositoryPath, "SourceSystem_*.json"))
        {
            string cSystem = File.ReadAllText(sSystem);
            
            // Deserialize to DTO (which contains only serializable properties)
            SourceSystemDTO? sourceSystemDto = JsonSerializer.Deserialize<SourceSystemDTO>(cSystem);
            if (sourceSystemDto == null)
            {
                // Handle the error, e.g., skip this file or throw an exception
                continue;
            }

            // Convert DTO to domain entity
            SourceSystem sourceSystem = SourceSystem.FromDTO(sourceSystemDto);

            // Wire up navigation properties by reading related entities
            List<SourceInterface> sInterfaceList = new();

            foreach (string sInterface in Directory.EnumerateFiles(repositoryPath, string.Format("SourceInterface_{0}.*.json", sourceSystem.Name)))
            {
                string cInterface = File.ReadAllText(sInterface);
                
                // Deserialize to DTO
                SourceInterfaceDTO? sourceInterfaceDto = JsonSerializer.Deserialize<SourceInterfaceDTO>(cInterface);
                if (sourceInterfaceDto != null)
                {
                    // Convert DTO to domain entity
                    SourceInterface sourceInterface = SourceInterface.FromDTO(sourceInterfaceDto);
                    
                    // Set up bidirectional navigation property
                    sourceInterface.ParentSourceSystem = sourceSystem;
                    
                    sInterfaceList.Add(sourceInterface);
                }
            }

            // Set the navigation property on the parent entity
            sourceSystem.SourceInterfaces = sInterfaceList;

            yield return sourceSystem;
        }
    }

    /// <summary>
    /// Serializes SourceSystem entities to JSON files using the DTO pattern.
    /// Converts domain entities to DTOs before serialization to ensure clean JSON output.
    /// Each entity type is serialized to separate files with ID references instead of full object graphs.
    /// </summary>
    /// <param name="sourceSystems">Collection of SourceSystem domain entities to serialize</param>
    /// <param name="repositoryPath">Path to the directory where JSON files will be written</param>
    public void Serialize(IEnumerable<SourceSystem> sourceSystems, string repositoryPath)
    {
        // Ensure directory exists
        Directory.CreateDirectory(repositoryPath);

        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        foreach (var sourceSystem in sourceSystems)
        {
            // Convert domain entity to DTO for serialization
            SourceSystemDTO sourceSystemDto = sourceSystem.ToDTO();
            
            // Serialize SourceSystem to its own file
            string systemFileName = Path.Combine(repositoryPath, $"SourceSystem_{sourceSystem.Name}.json");
            string systemJson = JsonSerializer.Serialize(sourceSystemDto, jsonOptions);
            File.WriteAllText(systemFileName, systemJson);

            // Serialize each SourceInterface to its own file
            if (sourceSystem.SourceInterfaces != null)
            {
                foreach (var sourceInterface in sourceSystem.SourceInterfaces)
                {
                    // Convert domain entity to DTO for serialization
                    SourceInterfaceDTO sourceInterfaceDto = sourceInterface.ToDTO();
                    
                    string interfaceFileName = Path.Combine(repositoryPath, 
                        $"SourceInterface_{sourceSystem.Name}.{sourceInterface.Name}.json");
                    string interfaceJson = JsonSerializer.Serialize(sourceInterfaceDto, jsonOptions);
                    File.WriteAllText(interfaceFileName, interfaceJson);
                }
            }
        }
    }

    /// <summary>
    /// Example method demonstrating how to serialize BusinessDomain entities using the DTO pattern.
    /// </summary>
    public void SerializeBusinessDomains(IEnumerable<BusinessDomain> businessDomains, string repositoryPath)
    {
        Directory.CreateDirectory(repositoryPath);

        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        foreach (var domain in businessDomains)
        {
            // Convert domain entity to DTO for serialization
            BusinessDomainDTO domainDto = domain.ToDTO();
            
            // Serialize to JSON file
            string fileName = Path.Combine(repositoryPath, $"BusinessDomain_{domain.Name}.json");
            string json = JsonSerializer.Serialize(domainDto, jsonOptions);
            File.WriteAllText(fileName, json);

            // Serialize related BusinessObjects to separate files
            if (domain.BusinessObjects != null)
            {
                foreach (var businessObject in domain.BusinessObjects)
                {
                    BusinessObjectDTO objectDto = businessObject.ToDTO();
                    string objectFileName = Path.Combine(repositoryPath, 
                        $"BusinessObject_{domain.Name}.{businessObject.Name}.json");
                    string objectJson = JsonSerializer.Serialize(objectDto, jsonOptions);
                    File.WriteAllText(objectFileName, objectJson);

                    // Serialize AttributeSets to separate files
                    if (businessObject.AttributeSets != null)
                    {
                        foreach (var attributeSet in businessObject.AttributeSets)
                        {
                            AttributeSetDTO attrSetDto = attributeSet.ToDTO();
                            string attrSetFileName = Path.Combine(repositoryPath,
                                $"AttributeSet_{domain.Name}.{businessObject.Name}.{attributeSet.Name}.json");
                            string attrSetJson = JsonSerializer.Serialize(attrSetDto, jsonOptions);
                            File.WriteAllText(attrSetFileName, attrSetJson);
                        }
                    }
                }
            }
        }
    }
}

