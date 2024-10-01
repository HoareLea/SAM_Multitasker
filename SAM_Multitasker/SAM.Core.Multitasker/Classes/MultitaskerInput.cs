using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SAM.Core.Multitasker
{
    public class MultitaskerInput : IJSAMObject
    {
        public List<MultitaskerVariable> multitaskerVariables;

        public MultitaskerInput()
        {

        }

        public MultitaskerInput(MultitaskerInput multitaskerInput)
        {
            if(multitaskerInput != null)
            {
                if(multitaskerInput.multitaskerVariables != null)
                {
                    multitaskerVariables = new List<MultitaskerVariable>();
                    foreach(MultitaskerVariable multitaskerVariable in multitaskerVariables)
                    {
                        if(multitaskerVariable != null)
                        {
                            multitaskerVariables.Add(new MultitaskerVariable(multitaskerVariable));
                        }
                    }
                }
            }
        }

        public MultitaskerInput(string name, object value)
        {
            multitaskerVariables = new List<MultitaskerVariable>();
            if(name != null)
            {
                multitaskerVariables.Add(new MultitaskerVariable(name, value));
            }
        }

        public MultitaskerInput(string name, ValueType valueType, object value)
        {
            multitaskerVariables = new List<MultitaskerVariable>();
            if (name != null)
            {
                multitaskerVariables.Add(new MultitaskerVariable(name, valueType, value));
            }
        }

        public MultitaskerInput(JObject jObject)
        {
            FromJObject(jObject);   
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("MultitaskerVariables"))
            {
                multitaskerVariables = new List<MultitaskerVariable>();

                JArray jArray = jObject.Value<JArray>("MultitaskerVariables");
                foreach(JObject jObject_MultitaskerVariable in jArray)
                {
                    multitaskerVariables.Add(new MultitaskerVariable(jObject_MultitaskerVariable));
                }
            }

            return true;
        }

        public JObject ToJObject()
        {
            JObject result = new JObject();
            result.Add("_type", Core.Query.FullTypeName(this));

            if(multitaskerVariables != null)
            {
                JArray jArray = new JArray();

                foreach(MultitaskerVariable multitaskerVariable in multitaskerVariables)
                {
                    if(multitaskerVariable == null)
                    {
                        continue;
                    }

                    jArray.Add(multitaskerVariable.ToJObject());
                }

                result.Add("MultitaskerVariables", jArray);
            }

            return result;
        }

        public Dictionary<string, dynamic> Variables
        {
            get
            {
                if(multitaskerVariables == null)
                {
                    return null;
                }

                Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
                foreach(MultitaskerVariable multitaskerVariable in multitaskerVariables)
                {
                    if(string.IsNullOrWhiteSpace(multitaskerVariable.Name))
                    {
                        continue;
                    }

                    result[multitaskerVariable.Name] = multitaskerVariable.Value;
                }

                return result;
            }
        }
    }
}
