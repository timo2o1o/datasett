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

    public IEnumerable<SourceSystemDTO> SourceSystemDTOs => _sourceSystemDTOs;

    public IEnumerable<SourceInterfaceDTO> SourceInterfaceDTOs => _sourceInterfaceDTOs;

    // Logical Business Concept Model DTOs
    private readonly IList<BusinessDomainDTO> _businessDomainDTOs;
    private readonly IList<BusinessConceptDTO> _businessConceptDTOs;
    private readonly IList<BusinessConceptRelationDTO> _businessConceptRelationDTOs;
    private readonly IList<BusinessConceptMappingCollectionDTO> _conceptMappingDTOs;

    public IEnumerable<BusinessDomainDTO> BusinessDomainDTOs => _businessDomainDTOs;

    public IEnumerable<BusinessConceptDTO> BusinessConceptDTOs => _businessConceptDTOs;

    public IEnumerable<BusinessConceptRelationDTO> BusinessConceptRelationDTOs => _businessConceptRelationDTOs;

    public IEnumerable<BusinessConceptMappingCollectionDTO> BusinessConceptMappingCollectionDTOs => _conceptMappingDTOs;

    // Some private fields to cache deserializing information:
    private readonly IDictionary<string, SourceAttribute> _sourceAttributeCache;

    private IDictionary<string, SourceAttribute> SourceAttributeCache => _sourceAttributeCache;

    // Constructor initializes the lists and dictionaries:
    public JsonContext()
    {
        _sourceSystemDTOs = new List<SourceSystemDTO>();
        _sourceInterfaceDTOs = new List<SourceInterfaceDTO>();

        _businessDomainDTOs = new List<BusinessDomainDTO>();
        _businessConceptDTOs = new List<BusinessConceptDTO>();
        _businessConceptRelationDTOs = new List<BusinessConceptRelationDTO>();
        _conceptMappingDTOs = new List<BusinessConceptMappingCollectionDTO>();

        _sourceAttributeCache = new Dictionary<string, SourceAttribute>();
    }

    public async Task LoadAsync(string repositoryPath)
    {
        _sourceSystemDTOs.Clear();
        _sourceInterfaceDTOs.Clear();

        _businessDomainDTOs.Clear();
        _businessConceptDTOs.Clear();
        _businessConceptRelationDTOs.Clear();
        _conceptMappingDTOs.Clear();

        if (Directory.Exists(repositoryPath))
        {

            // Physical Source System DTOs:
            string sourceSystemPath = Path.Combine(repositoryPath, "PhysicalSourceSystemModel");

            foreach (SourceSystemDTO currentSrcSystem in await DeserializeMetadataObjectsAsync<SourceSystemDTO>(sourceSystemPath, "SourceSystem_*.json"))
            {
                _sourceSystemDTOs.Add(currentSrcSystem);
            }

            foreach (SourceInterfaceDTO currentInterface in await DeserializeMetadataObjectsAsync<SourceInterfaceDTO>(sourceSystemPath, "SourceInterface_*.json"))
            {
                _sourceInterfaceDTOs.Add(currentInterface);
            }

            // Logical Business Concept Model DTOs
            string logicalBOMPath = Path.Combine(repositoryPath, "LogicalBusinessConceptModel");
            
            string businessDomainsFilePath = Path.Combine(logicalBOMPath, "BusinessDomains.json");
            foreach (BusinessDomainDTO currentBD in await DeserializeListOfMetadataConceptsAsync<BusinessDomainDTO>(businessDomainsFilePath))
            {
                _businessDomainDTOs.Add(currentBD);
            }

            string businessConceptsFilePath = Path.Combine(logicalBOMPath, "BusinessConcepts.json");
            foreach (BusinessConceptDTO currentBC in await DeserializeListOfMetadataConceptsAsync<BusinessConceptDTO>(businessConceptsFilePath))
            {
                _businessConceptDTOs.Add(currentBC);
            }

            string businessConceptRelationsFilePath = Path.Combine(logicalBOMPath, "BusinessConceptRelations.json");
            foreach (BusinessConceptRelationDTO currentRelation in await DeserializeListOfMetadataConceptsAsync<BusinessConceptRelationDTO>(businessConceptRelationsFilePath))
            {
                _businessConceptRelationDTOs.Add(currentRelation);
            }

            foreach (BusinessConceptMappingCollectionDTO currentMappingCollection in await DeserializeMetadataObjectsAsync<BusinessConceptMappingCollectionDTO>(logicalBOMPath, "BusinessConceptMappings_*.json"))
            {
                _conceptMappingDTOs.Add(currentMappingCollection);
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

    private async Task<IList<T>> DeserializeListOfMetadataConceptsAsync<T>(string listObjectFilepath)
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

    private async Task SerializeMetadataObjectsAsync(string filePath, object data)
    {
        using (FileStream fs = File.Create(filePath))
        {
            await JsonSerializer.SerializeAsync(fs, data, JsonDefaults.Web);
        }
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

    private IEnumerable<BusinessConceptMapping> GetMappingsOfBusinessConceptFromDTO(string businessConceptID, BusinessConcept businessConcept)
    {
        foreach (BusinessConceptMappingCollectionDTO currentMappingCollectionDTO in BusinessConceptMappingCollectionDTOs)
        {
            if (currentMappingCollectionDTO.BusinessConceptId == businessConceptID)
            {
                foreach (BusinessConceptMappingDTO currentMappingDTO in currentMappingCollectionDTO.BusinessConceptMappings)
                {
                    BusinessConceptMapping mapping = BusinessConceptMapping.FromDTO(currentMappingDTO, businessConcept, SourceAttributeCache[$"{currentMappingDTO.SourceInterfaceId}.{currentMappingDTO.SourceAttributeName}"]);
                    yield return mapping;
                }
            }
        }
    }

    public IEnumerable<BusinessDomain> GetBusinessDomains()
    {
        
        IList<BusinessDomain> businessDomains = new List<BusinessDomain>();
        IDictionary<string, BusinessConcept> businessConceptCache = new Dictionary<string, BusinessConcept>();

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

            foreach (BusinessConceptDTO currentBCDTO in BusinessConceptDTOs)
            {

                if (currentBCDTO.BusinessDomainId == currentBusinessDomain.Name)
                {
                    BusinessConcept businessConcept = BusinessConcept.FromDTO(currentBCDTO, currentBusinessDomain);

                    businessConcept.BusinessConceptMappings = GetMappingsOfBusinessConceptFromDTO(currentBCDTO.BusinessConceptId, businessConcept).ToList();

                    currentBusinessDomain.BusinessConcepts.Add(businessConcept);

                    businessConceptCache.Add(currentBCDTO.BusinessConceptId, businessConcept);
                }

            }

        }

        // We need a complete list of all business concepts in order to create the BCRelations afterwards.
        // That's why we iterate over the business domains twice:
        foreach (BusinessDomain currentBusinessDomain in businessDomains)
        { 
            foreach (BusinessConceptRelationDTO currentBCRDTO in BusinessConceptRelationDTOs)
            {
                if (currentBCRDTO.BusinessDomainId == currentBusinessDomain.Name)
                {
                    BusinessConceptRelation businessConceptRelation = BusinessConceptRelation.FromDTO(currentBCRDTO, currentBusinessDomain, businessConceptCache);
                    currentBusinessDomain.BusinessConceptRelations.Add(businessConceptRelation);
                }
            }
        }

        return businessDomains;
    }

    public async Task WriteLBCMAsync(string repositoryPath, IEnumerable<BusinessDomain> businessDomains)
    {

        string lbcmPath = Path.Combine(repositoryPath, "LogicalBusinessConceptModel");

        List<BusinessDomainDTO> businessDomainDTOs = new List<BusinessDomainDTO>();
        List<BusinessConceptDTO> businessConceptDTOs = new List<BusinessConceptDTO>();

        foreach (BusinessDomain currentBD in businessDomains)
        {

            BusinessDomainDTO dto = BusinessDomain.ToDTO(currentBD);
            businessDomainDTOs.Add(dto);

            foreach (BusinessConcept currentBC in currentBD.BusinessConcepts)
            {
                BusinessConceptDTO bcDto = BusinessConcept.ToDTO(currentBC, dto.BusinessDomainId ?? string.Empty);
                businessConceptDTOs.Add(bcDto);

                // The attributemappings will be written to one file for each business concept:
                BusinessConceptMappingCollectionDTO bcmCollection = new BusinessConceptMappingCollectionDTO(bcDto.BusinessConceptId);

                foreach (BusinessConceptMapping currentMapping in currentBC.BusinessConceptMappings)
                {
                    BusinessConceptMappingDTO mappingDto = BusinessConceptMapping.ToDTO(currentMapping, bcDto.BusinessConceptId);
                    bcmCollection.BusinessConceptMappings.Add(mappingDto);
                }

                await SerializeMetadataObjectsAsync(Path.Combine(lbcmPath, $"BusinessConceptMappings_{currentBD.Name}_{currentBC.Name}.json"), bcmCollection);
            }

        }

        await SerializeMetadataObjectsAsync(Path.Combine(lbcmPath, "BusinessDomains.json"), businessDomainDTOs);
        await SerializeMetadataObjectsAsync(Path.Combine(lbcmPath, "BusinessConcepts.json"), businessConceptDTOs);
        //TODO: Write BusinessConceptRelations as well once we have them in our domain model

    }

}