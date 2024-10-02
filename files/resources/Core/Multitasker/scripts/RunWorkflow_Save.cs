using Newtonsoft.Json;
using SAM.Core;
using SAM.Analytical;
using SAM.Analytical.Tas;
using System.Linq;

string path = [Value];

string fileName = System.IO.Path.GetFileNameWithoutExtension(path);
string directoryName = System.IO.Path.GetDirectoryName(path);

string path_TBD = System.IO.Path.Combine(directoryName, fileName + ".tbd");

AnalyticalModel analyticalModel = SAM.Core.Convert.ToSAM<AnalyticalModel>(path).FirstOrDefault();
if(analyticalModel == null)
{
    return false;
}

string path_gbXML = System.IO.Path.Combine(directoryName, fileName + ".xml");

SAM.Analytical.gbXML.Create.gbXML(analyticalModel, path_gbXML, SAM.Core.Tolerance.MacroDistance, 0.00001);

WorkflowSettings workflowSettings = new WorkflowSettings()
{
    Path_TBD = path_TBD,
    Path_gbXML = path_gbXML,
    WeatherData = null,
    DesignDays_Heating = null,
    DesignDays_Cooling = null,
    SurfaceOutputSpecs = null,
    UnmetHours = true,
    Simulate = true,
    Sizing = true,
    UpdateZones = false,
    UseWidths = false,
    AddIZAMs = false,
    SimulateFrom = 1,
    SimulateTo = 365,
    RemoveExistingTBD = true
};

analyticalModel = SAM.Analytical.Tas.Modify.RunWorkflow(analyticalModel, workflowSettings);

if(true)
{
	string path_Json =  System.IO.Path.Combine(directoryName, fileName + "_Done" + ".json");
	
	string json = SAM.Core.Convert.ToString(analyticalModel);
	json = json == null ? string.Empty : json;
	System.IO.File.WriteAllText(path_Json, json);
}

return true;