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
    private readonly IList<SourceSystemDTO> _sourceSystemDTOs;
    private readonly IList<SourceInterfaceDTO> _sourceInterfaceDTOs;
    private readonly IList<BusinessDomainDTO> _businessDomainDTOs;

    public JsonContext()
    {
        _sourceSystemDTOs = new List<SourceSystemDTO>();
        _sourceInterfaceDTOs = new List<SourceInterfaceDTO>();
        _businessDomainDTOs = new List<BusinessDomainDTO>();
    }

    public IEnumerable<SourceSystemDTO> SourceSystemDTOs => _sourceSystemDTOs;

    public IEnumerable<SourceInterfaceDTO> SourceInterfaceDTOs => _sourceInterfaceDTOs;

    public IEnumerable<BusinessDomainDTO> BusinessDomainDTOs => _businessDomainDTOs;

    public async Task LoadAsync(string repositoryPath)
    {
        _sourceSystemDTOs.Clear();
        _sourceInterfaceDTOs.Clear();
        _businessDomainDTOs.Clear();

        if (Directory.Exists(repositoryPath))
        {

            foreach (string current_source_system_file_path in Directory.EnumerateFiles(repositoryPath, "SourceSystem_*.json", SearchOption.AllDirectories))
            {

                using (FileStream source_system_filestream = File.OpenRead(current_source_system_file_path))
                {
                    SourceSystemDTO? sourceSystemDto = await JsonSerializer.DeserializeAsync<SourceSystemDTO>(source_system_filestream, JsonDefaults.Web);
                    if (sourceSystemDto != null)
                    {
                        _sourceSystemDTOs.Add(sourceSystemDto);
                    }
                }

            }

            foreach (string current_source_interface_file_path in Directory.EnumerateFiles(repositoryPath, "SourceInterface_*.json", SearchOption.AllDirectories))
            {

                using (FileStream source_interface_filestream = File.OpenRead(current_source_interface_file_path))
                {
                    SourceInterfaceDTO? sourceInterfaceDto = await JsonSerializer.DeserializeAsync<SourceInterfaceDTO>(source_interface_filestream, JsonDefaults.Web);
                    if (sourceInterfaceDto != null)
                    {
                        _sourceInterfaceDTOs.Add(sourceInterfaceDto);
                    }
                }

            }

            foreach (string current_business_domain_file_path in Directory.EnumerateFiles(repositoryPath, "BusinessDomain_*.json", SearchOption.AllDirectories))
            {

                using (FileStream business_domain_filestream = File.OpenRead(current_business_domain_file_path))
                {
                    BusinessDomainDTO? businessDomainDto = await JsonSerializer.DeserializeAsync<BusinessDomainDTO>(business_domain_filestream, JsonDefaults.Web);
                    if (businessDomainDto != null)
                    {
                        _businessDomainDTOs.Add(businessDomainDto);
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