using System.Text.Json;
using DataSett.Metamodel;

namespace DataSett.Metamodel.Serde;

public class Json
{

    public IEnumerable<SourceSystem> Deserialize(string repositoryPath)
    {
        foreach (string sSystem in Directory.EnumerateFiles(repositoryPath, "SourceSystem_*.json"))
            {
                string cSystem = File.ReadAllText(sSystem);
                SourceSystem? sourceSystem = JsonSerializer.Deserialize<SourceSystem>(cSystem);
                if (sourceSystem == null)
                {
                    // Handle the error, e.g., skip this file or throw an exception
                    continue;
                }

                List<SourceInterface> sInterfaceList = new();

                foreach (string sInterface in Directory.EnumerateFiles(repositoryPath, string.Format("SourceInterface_{0}.*.json", sourceSystem.Name)))
                {
                    string cInterface = File.ReadAllText(sInterface);
                    SourceInterface? sourceInterface = JsonSerializer.Deserialize<SourceInterface>(cInterface);
                    if (sourceInterface != null)
                    {
                        sInterfaceList.Add(sourceInterface);
                    }
                }

                sourceSystem.SourceInterfaces = sInterfaceList;

                yield return sourceSystem;
            }
    }

}
