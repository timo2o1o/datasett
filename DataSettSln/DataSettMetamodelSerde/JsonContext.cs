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

    private BusinessDomain? CreateBusinessDomainFromDTO(BusinessDomainDTO dto, IList<BusinessDomain> listBusinessDomainCache)
    {

        if (!string.IsNullOrEmpty(dto.Name))
        {

            BusinessDomain? parentBusinessDomain = null;

            foreach (BusinessDomain bd in listBusinessDomainCache)
            {
                if (bd.Name == dto.Name)
                {
                    return bd;
                }
                else if (bd.Name == dto.ParentBusinessDomainId)
                {
                    parentBusinessDomain = bd;
                }
            }

            if (string.IsNullOrEmpty(dto.ParentBusinessDomainId))
            {
                BusinessDomain newBD = BusinessDomain.FromDTO(dto, null);
                listBusinessDomainCache.Add(newBD);
                return newBD;
            }
            else
            {

                if (parentBusinessDomain != null)
                {
                    BusinessDomain newBD = BusinessDomain.FromDTO(dto, parentBusinessDomain);
                    listBusinessDomainCache.Add(newBD);
                    return newBD;
                }
                else
                {
                    // If we have a ParentBusinessDomainId, but cannot find the according object in the cache, we search it in our DTOs and create it recursively:
                    var parentDto = BusinessDomainDTOs.FirstOrDefault(x => x.Name == dto.ParentBusinessDomainId);
                    if (parentDto != null)
                    {
                        parentBusinessDomain = CreateBusinessDomainFromDTO(parentDto, listBusinessDomainCache);

                        if (parentBusinessDomain != null)
                            listBusinessDomainCache.Add(parentBusinessDomain);

                        return BusinessDomain.FromDTO(dto, parentBusinessDomain);
                    }
                    else
                    {
                        throw new InvalidDataException($"Cannot find ParentBusinessDomainDTO with Name '{dto.ParentBusinessDomainId}' for BusinessDomainDTO with Name '{dto.Name}'.");
                    }
                }
            }
        }
        else
        {
            return null;
        }

    }

    public IEnumerable<BusinessDomain> GetBusinessDomains()
    {
        
        IList<BusinessDomain> businessDomains = new List<BusinessDomain>();

        // First pass: Create all BusinessDomain entities
        foreach (BusinessDomainDTO dto in BusinessDomainDTOs)
        {
            if (!string.IsNullOrEmpty(dto.Name))
            {
                // Watch out: List of business domains gets updated within the method!
                // This is why we check here if the business domain is already in the list:
                _ = CreateBusinessDomainFromDTO(dto, businessDomains);
            }
        }

        return businessDomains;
    }

    public Task SaveChangesAsync(string repositoryPath, IEnumerable<SourceSystem> sourceSystems)
    {
        // Implementation for saving domain entities as DTOs to JSON files would go here
        throw new NotImplementedException();
    }

}