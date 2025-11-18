using System.Text.Json;
using DataSett.Metamodel;

namespace DataSett.Metamodel.Serde;

/// <summary>
/// JSON serialization and deserialization context for the DataSettMetamodel.
/// 
/// Key Concepts:
/// - DTOs (Data Transfer Objects) are used for serialization to JSON
/// - Domain entities are used for business logic and navigation
/// - Deserialization reads DTOs then converts to domain entities
/// - Serialization converts domain entities to DTOs before writing JSON
/// </summary>
public class JsonContext
{

    // Physical Source System DTOs:
    private readonly IList<SourceSystemDTO> _sourceSystemDTOs;
    private readonly IList<SourceInterfaceDTO> _sourceInterfaceDTOs;

    // Logical Business Object Model DTOs
    private readonly IList<BusinessDomainDTO> _businessDomainDTOs;
    private readonly IList<BusinessObjectDTO> _businessObjectDTOs;

    public JsonContext()
    {
        _sourceSystemDTOs = new List<SourceSystemDTO>();
        _sourceInterfaceDTOs = new List<SourceInterfaceDTO>();

        _businessDomainDTOs = new List<BusinessDomainDTO>();
        _businessObjectDTOs = new List<BusinessObjectDTO>();
    }

    public IEnumerable<SourceSystemDTO> SourceSystemDTOs => _sourceSystemDTOs;

    public IEnumerable<SourceInterfaceDTO> SourceInterfaceDTOs => _sourceInterfaceDTOs;

    public IEnumerable<BusinessDomainDTO> BusinessDomainDTOs => _businessDomainDTOs;

    public async Task LoadAsync(string repositoryPath)
    {
        _sourceSystemDTOs.Clear();
        _sourceInterfaceDTOs.Clear();

        _businessDomainDTOs.Clear();
        _businessObjectDTOs.Clear();

        if (Directory.Exists(repositoryPath))
        {

            await DeserializeMetadataObjectsAsync<SourceSystemDTO>(repositoryPath, "SourceSystem_*.json", _sourceSystemDTOs);
            await DeserializeMetadataObjectsAsync<SourceInterfaceDTO>(repositoryPath, "SourceInterface_*.json", _sourceInterfaceDTOs);

            string businessDomainsFilePath = Path.Combine(repositoryPath, "LogicalBusinessObjectModel", "BusinessDomains.json");
            await DeserializeListOfMetadataObjectsAsync<BusinessDomainDTO>(businessDomainsFilePath, _businessDomainDTOs);

            string businessObjectsFilePath = Path.Combine(repositoryPath, "LogicalBusinessObjectModel", "BusinessObjects.json");
            await DeserializeListOfMetadataObjectsAsync<BusinessObjectDTO>(businessObjectsFilePath, _businessObjectDTOs);

        }
    }

    private async Task DeserializeMetadataObjectsAsync<T>(string repositoryPath, string searchPattern, IList<T> targetList)
    {
        foreach (string currentFilePath in Directory.EnumerateFiles(repositoryPath, searchPattern, SearchOption.AllDirectories))
        {

            using (FileStream filestream = File.OpenRead(currentFilePath))
            {
                T? objectDto = await JsonSerializer.DeserializeAsync<T>(filestream, JsonDefaults.Web);
                if (objectDto != null)
                {
                    targetList.Add(objectDto);
                }
            }

        }
    }

    private async Task DeserializeListOfMetadataObjectsAsync<T>(string listObjectFilepath, IList<T> targetList)
    {
        if (File.Exists(listObjectFilepath))
        {
            using (FileStream objectsFilestream = File.OpenRead(listObjectFilepath))
            {
                IList<T>? objectDTOs = await JsonSerializer.DeserializeAsync<List<T>>(objectsFilestream, JsonDefaults.Web);
                if (objectDTOs != null)
                {
                    foreach (T currentObject in objectDTOs)
                    {
                        targetList.Add(currentObject);
                    }
                }
            }

        }
    }

    public IEnumerable<SourceSystem> GetSourceSystems()
    {
        // TODO: Can this method be made asynchronous?

        foreach (SourceSystemDTO current_sourceSystem_dto in SourceSystemDTOs)
        {
            // Create the SourceSystem object:
            SourceSystem newSourceSystem = SourceSystem.FromDTO(current_sourceSystem_dto);
            
            // Find all SourceInterfaceDTOs that belong to this SourceSystemDTO:
            foreach (SourceInterfaceDTO current_sourceInterface_dto in SourceInterfaceDTOs)
            {
                if (current_sourceInterface_dto.SourceSystemId == current_sourceSystem_dto.SourceSystemId)
                {
                    // Convert DTO to domain entity:
                    SourceInterface sourceInterface = SourceInterface.FromDTO(current_sourceInterface_dto, newSourceSystem);
                    
                    newSourceSystem.SourceInterfaces.Add(sourceInterface);
                }
            }

            yield return newSourceSystem;
        }
    }

    public IEnumerable<BusinessDomain> GetBusinessDomains()
    {
        // Build a dictionary to track all BusinessDomain entities by their ID (Name)
        var businessDomainDict = new Dictionary<string, BusinessDomain>();
        
        // First pass: Create all BusinessDomain entities
        foreach (BusinessDomainDTO dto in BusinessDomainDTOs)
        {
            if (!string.IsNullOrEmpty(dto.Name))
            {
                var businessDomain = BusinessDomain.FromDTO(dto, null);
                businessDomainDict[dto.Name] = businessDomain;
            }
        }
        
        // Second pass: Set up parent-child relationships
        foreach (BusinessDomainDTO dto in BusinessDomainDTOs)
        {
            if (!string.IsNullOrEmpty(dto.Name) && businessDomainDict.ContainsKey(dto.Name))
            {
                var currentDomain = businessDomainDict[dto.Name];
                
                // Set parent relationship if ParentBusinessDomainId is specified
                if (!string.IsNullOrEmpty(dto.ParentBusinessDomainId) && 
                    businessDomainDict.ContainsKey(dto.ParentBusinessDomainId))
                {
                    var parentDomain = businessDomainDict[dto.ParentBusinessDomainId];
                    currentDomain.ParentBusinessDomain = parentDomain;
                    parentDomain.ChildBusinessDomains.Add(currentDomain);
                }
            }
        }
        
        // Return all business domains
        return businessDomainDict.Values;
    }

    public Task SaveChangesAsync(string repositoryPath, IEnumerable<SourceSystem> sourceSystems)
    {
        // Implementation for saving domain entities as DTOs to JSON files would go here
        throw new NotImplementedException();
    }

}