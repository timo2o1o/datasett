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
    private readonly IList<AttributeSetDTO> _attributeSetDTOs;

    // Some private fields to cache deserializing information:
    private readonly IDictionary<string, SourceAttribute> _sourceAttributeCache;

    public JsonContext()
    {
        _sourceSystemDTOs = new List<SourceSystemDTO>();
        _sourceInterfaceDTOs = new List<SourceInterfaceDTO>();

        _businessDomainDTOs = new List<BusinessDomainDTO>();
        _businessObjectDTOs = new List<BusinessObjectDTO>();
        _attributeSetDTOs = new List<AttributeSetDTO>();

        _sourceAttributeCache = new Dictionary<string, SourceAttribute>();
    }

    public IEnumerable<SourceSystemDTO> SourceSystemDTOs => _sourceSystemDTOs;

    public IEnumerable<SourceInterfaceDTO> SourceInterfaceDTOs => _sourceInterfaceDTOs;

    public IEnumerable<BusinessDomainDTO> BusinessDomainDTOs => _businessDomainDTOs;

    public IEnumerable<BusinessObjectDTO> BusinessObjectDTOs => _businessObjectDTOs;

    public IEnumerable<AttributeSetDTO> AttributeSetDTOs => _attributeSetDTOs;

    private IDictionary<string, SourceAttribute> SourceAttributeCache => _sourceAttributeCache;

    public async Task LoadAsync(string repositoryPath)
    {
        _sourceSystemDTOs.Clear();
        _sourceInterfaceDTOs.Clear();

        _businessDomainDTOs.Clear();
        _businessObjectDTOs.Clear();
        _attributeSetDTOs.Clear();

        if (Directory.Exists(repositoryPath))
        {

            string sourceSystemPath = Path.Combine(repositoryPath, "PhysicalSourceSystemModel");

            foreach (SourceSystemDTO currentSrcSystem in await DeserializeMetadataObjectsAsync<SourceSystemDTO>(sourceSystemPath, "SourceSystem_*.json"))
            {
                _sourceSystemDTOs.Add(currentSrcSystem);

            }

            foreach (SourceInterfaceDTO currentInterface in await DeserializeMetadataObjectsAsync<SourceInterfaceDTO>(sourceSystemPath, "SourceInterface_*.json"))
            {
                _sourceInterfaceDTOs.Add(currentInterface);
            }

            string logicalBOMPath = Path.Combine(repositoryPath, "LogicalBusinessObjectModel");
            
            string businessDomainsFilePath = Path.Combine(logicalBOMPath, "BusinessDomains.json");
            foreach (BusinessDomainDTO currentBD in await DeserializeListOfMetadataObjectsAsync<BusinessDomainDTO>(businessDomainsFilePath))
            {
                _businessDomainDTOs.Add(currentBD);
            }

            string businessObjectsFilePath = Path.Combine(logicalBOMPath, "BusinessObjects.json");
            foreach (BusinessObjectDTO currentBO in await DeserializeListOfMetadataObjectsAsync<BusinessObjectDTO>(businessObjectsFilePath))
            {
                _businessObjectDTOs.Add(currentBO);
            }

            foreach (AttributeSetDTO[] currentAttributeSets in await DeserializeMetadataObjectsAsync<AttributeSetDTO[]>(logicalBOMPath, "AttributeSets_*.json"))
            {
                foreach (AttributeSetDTO currentAttributeSet in currentAttributeSets)
                {
                    _attributeSetDTOs.Add(currentAttributeSet);
                }
            }

        }
    }

    private async Task<IList<T>> DeserializeMetadataObjectsAsync<T>(string repositoryPath, string searchPattern)
    {
        List<T> result = new List<T>();

        foreach (string currentFilePath in Directory.EnumerateFiles(repositoryPath, searchPattern, SearchOption.AllDirectories))
        {

            using (FileStream filestream = File.OpenRead(currentFilePath))
            {
                T? objectDto = await JsonSerializer.DeserializeAsync<T>(filestream, JsonDefaults.Web);
                if (objectDto != null)
                {
                    result.Add(objectDto);
                }
            }

        }

        return result;
    }

    private async Task<IList<T>> DeserializeListOfMetadataObjectsAsync<T>(string listObjectFilepath)
    {
        List<T> result = new List<T>();

        if (File.Exists(listObjectFilepath))
        {
            using (FileStream objectsFilestream = File.OpenRead(listObjectFilepath))
            {
                IList<T>? objectDTOs = await JsonSerializer.DeserializeAsync<List<T>>(objectsFilestream, JsonDefaults.Web);
                if (objectDTOs != null)
                {
                    foreach (T currentObject in objectDTOs)
                    {
                        result.Add(currentObject);
                    }
                }
            }

        }

        return result;
    }

    public IEnumerable<SourceSystem> GetSourceSystems()
    {
        // TODO: Can this method be made asynchronous?

        SourceAttributeCache.Clear();

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

                    // Cache SourceAttributes for adding them to AttributeSetMapppings later on:
                    if (sourceInterface.SourceAttributes != null)
                    {
                        foreach (var currentKVPair in sourceInterface.SourceAttributes.Select(sa => new KeyValuePair<string, SourceAttribute>($"{current_sourceInterface_dto.SourceInterfaceId}.{sa.Name}", sa)))
                        {
                            SourceAttributeCache.Add(currentKVPair);
                        }
                    }

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
                    parentBusinessDomain.ChildBusinessDomains.Add(newBD);
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

                        BusinessDomain newBD = BusinessDomain.FromDTO(dto, parentBusinessDomain);

                        if (parentBusinessDomain != null)
                        { 
                            parentBusinessDomain.ChildBusinessDomains.Add(newBD);
                            listBusinessDomainCache.Add(parentBusinessDomain);
                        }

                        return newBD;
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

    private IEnumerable<AttributeSet> GetAttributeSetsOfBusinessObjectFromDTO(string businessObjectID, BusinessObject businessObject)
    {
        foreach (AttributeSetDTO currentAttributeSetDTO in AttributeSetDTOs)
        {
            if (currentAttributeSetDTO.BusinessObjectId == businessObjectID)
            {
                AttributeSet attributeSet = AttributeSet.FromDTO(currentAttributeSetDTO, businessObject, SourceAttributeCache);
                yield return attributeSet;
            }
        }
    }

    public IEnumerable<BusinessDomain> GetBusinessDomains()
    {
        
        IList<BusinessDomain> businessDomains = new List<BusinessDomain>();

        foreach (BusinessDomainDTO dto in BusinessDomainDTOs)
        {
            if (!string.IsNullOrEmpty(dto.Name))
            {
                // Watch out: List of business domains gets updated within the method!
                _ = CreateBusinessDomainFromDTO(dto, businessDomains);
            }
        }

        foreach (BusinessDomain currentBusinessDomain in businessDomains)
        {

            foreach (BusinessObjectDTO currentBODTO in BusinessObjectDTOs)
            {

                if (currentBODTO.BusinessDomainId == currentBusinessDomain.Name)
                {
                    BusinessObject businessObject = BusinessObject.FromDTO(currentBODTO, currentBusinessDomain);

                    businessObject.AttributeSets = GetAttributeSetsOfBusinessObjectFromDTO(currentBODTO.BusinessObjectId, businessObject).ToList();

                    currentBusinessDomain.BusinessObjects.Add(businessObject);

                }

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