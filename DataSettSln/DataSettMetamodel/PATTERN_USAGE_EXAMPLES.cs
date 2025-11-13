using System;
using System.Collections.Generic;
using System.Text.Json;
using DataSett.Metamodel;

namespace DataSett.Metamodel.Examples
{
    /// <summary>
    /// Example usage of the Base/DTO/Domain pattern in DataSettMetamodel.
    /// This class demonstrates how to work with the three-tier architecture.
    /// </summary>
    public class PatternUsageExamples
    {
        /// <summary>
        /// Example 1: Creating domain entities and converting to DTOs for serialization.
        /// </summary>
        public static void Example1_DomainToDTO()
        {
            Console.WriteLine("=== Example 1: Domain to DTO Conversion ===\n");

            // Create domain entities with full object graph
            var salesDomain = new BusinessDomain("Sales");
            
            var customer = new BusinessObject("Customer", salesDomain);
            salesDomain.BusinessObjects!.Add(customer);
            salesDomain.BusinessObjectIds.Add(customer.Id ?? "");

            var coreAttributes = new AttributeSet("Core", customer);
            customer.AttributeSets!.Add(coreAttributes);
            customer.AttributeSetIds.Add(coreAttributes.Id ?? "");

            // Convert to DTO for serialization
            BusinessDomainDTO domainDto = salesDomain.ToDTO();
            BusinessObjectDTO customerDto = customer.ToDTO();
            AttributeSetDTO coreDto = coreAttributes.ToDTO();

            // Serialize to JSON
            var options = new JsonSerializerOptions { WriteIndented = true };
            
            Console.WriteLine("BusinessDomain DTO:");
            Console.WriteLine(JsonSerializer.Serialize(domainDto, options));
            Console.WriteLine("\nBusinessObject DTO:");
            Console.WriteLine(JsonSerializer.Serialize(customerDto, options));
            Console.WriteLine("\nAttributeSet DTO:");
            Console.WriteLine(JsonSerializer.Serialize(coreDto, options));
            Console.WriteLine();
        }

        /// <summary>
        /// Example 2: Deserializing DTOs and converting to domain entities.
        /// </summary>
        public static void Example2_DTOToDomain()
        {
            Console.WriteLine("=== Example 2: DTO to Domain Conversion ===\n");

            // Simulate JSON deserialization
            string businessObjectJson = @"{
                ""businessObjectId"": ""Sales.Customer"",
                ""businessObjectName"": ""Customer"",
                ""attributeSets"": [""Sales.Customer.Core"", ""Sales.Customer.Extended""]
            }";

            // Deserialize to DTO
            BusinessObjectDTO? customerDto = JsonSerializer.Deserialize<BusinessObjectDTO>(businessObjectJson);
            
            if (customerDto != null)
            {
                // Convert DTO to domain entity
                BusinessObject customer = BusinessObject.FromDTO(customerDto);
                
                Console.WriteLine($"Created domain entity: {customer.Name}");
                Console.WriteLine($"ID: {customer.Id}");
                Console.WriteLine($"Attribute Set IDs: {string.Join(", ", customer.AttributeSetIds)}");
                Console.WriteLine("Note: Navigation properties need to be wired up separately\n");
            }
        }

        /// <summary>
        /// Example 3: Working with SourceSystem entities.
        /// </summary>
        public static void Example3_SourceSystemPattern()
        {
            Console.WriteLine("=== Example 3: SourceSystem Pattern ===\n");

            // Create domain entity
            var sourceSystem = new SourceSystem
            {
                SourceSystemId = "SQLDB01",
                Name = "Production Database",
                Driver = "SQL Server",
                Server = "localhost",
                Version = "2022",
                ConnectionString = "Server=localhost;Database=Production;"
            };

            var customerTable = new SourceInterface
            {
                SourceInterfaceId = "SQLDB01.dbo.Customer",
                Name = "Customer",
                Schema = "dbo",
                Catalog = "Production"
            };

            // Set up navigation properties
            customerTable.ParentSourceSystem = sourceSystem;
            sourceSystem.SourceInterfaces = new List<SourceInterface> { customerTable };

            // Convert to DTO for serialization
            SourceSystemDTO systemDto = sourceSystem.ToDTO();
            SourceInterfaceDTO tableDto = customerTable.ToDTO();

            var options = new JsonSerializerOptions { WriteIndented = true };
            
            Console.WriteLine("SourceSystem DTO (no navigation properties):");
            Console.WriteLine(JsonSerializer.Serialize(systemDto, options));
            Console.WriteLine("\nSourceInterface DTO (parent reference removed):");
            Console.WriteLine(JsonSerializer.Serialize(tableDto, options));
            Console.WriteLine();
        }

        /// <summary>
        /// Example 4: Complete workflow - Create, Serialize, Deserialize, Reconstruct.
        /// </summary>
        public static void Example4_CompleteWorkflow()
        {
            Console.WriteLine("=== Example 4: Complete Workflow ===\n");

            // Step 1: Create domain model
            Console.WriteLine("Step 1: Creating domain entities...");
            var domain = new BusinessDomain("Finance");
            var account = new BusinessObject("Account", domain);
            domain.BusinessObjects!.Add(account);
            domain.BusinessObjectIds.Add(account.Id ?? "");

            var accountCore = new AttributeSet("Core", account);
            account.AttributeSets!.Add(accountCore);
            account.AttributeSetIds.Add(accountCore.Id ?? "");

            Console.WriteLine($"  Created: {domain.Name} > {account.Name} > {accountCore.Name}");

            // Step 2: Convert to DTOs
            Console.WriteLine("\nStep 2: Converting to DTOs...");
            var domainDto = domain.ToDTO();
            var accountDto = account.ToDTO();
            var coreDto = accountCore.ToDTO();

            // Step 3: Serialize
            Console.WriteLine("Step 3: Serializing to JSON...");
            var options = new JsonSerializerOptions { WriteIndented = true };
            var domainJson = JsonSerializer.Serialize(domainDto, options);
            var accountJson = JsonSerializer.Serialize(accountDto, options);
            var coreJson = JsonSerializer.Serialize(coreDto, options);

            Console.WriteLine("  JSON created for 3 entities");

            // Step 4: Deserialize
            Console.WriteLine("\nStep 4: Deserializing from JSON...");
            var deserializedDomainDto = JsonSerializer.Deserialize<BusinessDomainDTO>(domainJson);
            var deserializedAccountDto = JsonSerializer.Deserialize<BusinessObjectDTO>(accountJson);
            var deserializedCoreDto = JsonSerializer.Deserialize<AttributeSetDTO>(coreJson);

            // Step 5: Convert back to domain
            Console.WriteLine("Step 5: Converting back to domain entities...");
            var restoredDomain = BusinessDomain.FromDTO(deserializedDomainDto!);
            var restoredAccount = BusinessObject.FromDTO(deserializedAccountDto!);
            var restoredCore = AttributeSet.FromDTO(deserializedCoreDto!);

            // Step 6: Wire up navigation properties
            Console.WriteLine("Step 6: Wiring up navigation properties...");
            restoredAccount.BusinessDomain = restoredDomain;
            restoredDomain.BusinessObjects!.Add(restoredAccount);
            
            restoredCore.BusinessObject = restoredAccount;
            restoredAccount.AttributeSets!.Add(restoredCore);

            Console.WriteLine($"  Restored: {restoredDomain.Name} > {restoredAccount.Name} > {restoredCore.Name}");
            Console.WriteLine($"  Navigation works: {restoredDomain.BusinessObjects[0].AttributeSets![0].Name}");
            Console.WriteLine();
        }

        /// <summary>
        /// Example 5: AttributeSetMapping pattern usage.
        /// </summary>
        public static void Example5_AttributeSetMapping()
        {
            Console.WriteLine("=== Example 5: AttributeSetMapping Pattern ===\n");

            // Create a mapping
            var mapping = new AttributeSetMapping
            {
                OrderNo = 1,
                Role = SourceAttributeRole.BusinessKey,
                HistoryType = HistoryType.None,
                Position = 1,
                Nullable = false,
                Datatype = "VARCHAR",
                Length = 50
            };

            // Create related entities for context
            var attributeSet = new AttributeSet
            {
                Id = "Finance.Account.Core",
                Name = "Core"
            };

            var sourceAttribute = new SourceAttribute
            {
                Name = "AccountNumber",
                IsPk = true,
                Datatype = "VARCHAR",
                Length = 50
            };

            // Set navigation properties
            mapping.AttributeSet = attributeSet;
            mapping.SourceAttribute = sourceAttribute;

            // Convert to DTO
            var mappingDto = mapping.ToDTO();

            var options = new JsonSerializerOptions { WriteIndented = true };
            Console.WriteLine("AttributeSetMapping DTO:");
            Console.WriteLine(JsonSerializer.Serialize(mappingDto, options));
            Console.WriteLine("\nNote: DTO contains foreign key references, not navigation properties");
            Console.WriteLine();
        }

        /// <summary>
        /// Runs all examples.
        /// </summary>
        public static void Main(string[] args)
        {
            Console.WriteLine("DataSettMetamodel - Base/DTO/Domain Pattern Examples");
            Console.WriteLine("======================================================\n");

            Example1_DomainToDTO();
            Example2_DTOToDomain();
            Example3_SourceSystemPattern();
            Example4_CompleteWorkflow();
            Example5_AttributeSetMapping();

            Console.WriteLine("All examples completed successfully!");
        }
    }
}
