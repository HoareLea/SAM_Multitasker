using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SAM.Core.Multitasker
{
    public class MultitaskerInput : IJSAMObject
    {
        public Dictionary<string, MultitaskerVariable> dictionary;

        public MultitaskerInput()
        {

        }

        public MultitaskerInput(MultitaskerInput multitaskerInput)
        {
            if(multitaskerInput != null)
            {
                if(multitaskerInput.dictionary != null)
                {
                    dictionary = new Dictionary<string, MultitaskerVariable>();
                    foreach (MultitaskerVariable multitaskerVariable in multitaskerInput.dictionary.Values)
                    {
                        if(multitaskerVariable != null)
                        {
                            Add(new MultitaskerVariable(multitaskerVariable));
                        }
                    }
                }
            }
        }

        public MultitaskerInput(IEnumerable<MultitaskerVariable> multitaskerVariables)
        {
            if(multitaskerVariables != null)
            {
                dictionary = new Dictionary<string, MultitaskerVariable>();
                foreach (MultitaskerVariable multitaskerVariable in multitaskerVariables)
                {
                    if(!string.IsNullOrWhiteSpace(multitaskerVariable?.Name))
                    {
                        dictionary[multitaskerVariable.Name] = multitaskerVariable;
                    }
                }
            }
        }

        public MultitaskerInput(string name, object value)
        {
            dictionary = new Dictionary<string, MultitaskerVariable>();
            if(name != null)
            {
                dictionary[name] = new MultitaskerVariable(name, value);
            }
        }

        public MultitaskerInput(string name, ValueType valueType, object value)
        {
            dictionary = new Dictionary<string, MultitaskerVariable>();
            if (name != null)
            {
                dictionary[name] = new MultitaskerVariable(name, valueType, value);
            }
        }

        public MultitaskerInput(JObject jObject)
        {
            FromJObject(jObject);   
        }

        public bool Add(MultitaskerVariable multitaskerVariable)
        {
            if(string.IsNullOrWhiteSpace(multitaskerVariable?.Name))
            {
                return false;
            }

            if(dictionary == null)
            {
                dictionary = new Dictionary<string, MultitaskerVariable>();
            }

            dictionary[multitaskerVariable.Name] = multitaskerVariable;
            return true;
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("MultitaskerVariables"))
            {
                dictionary = new Dictionary<string, MultitaskerVariable>();

                JArray jArray = jObject.Value<JArray>("MultitaskerVariables");
                foreach(JObject jObject_MultitaskerVariable in jArray)
                {
                    Add(new MultitaskerVariable(jObject_MultitaskerVariable));
                }
            }

            return true;
        }

        public JObject ToJObject()
        {
            JObject result = new JObject();
            result.Add("_type", Core.Query.FullTypeName(this));

            if(dictionary != null)
            {
                JArray jArray = new JArray();

                foreach(MultitaskerVariable multitaskerVariable in dictionary.Values)
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
                if(dictionary == null)
                {
                    return null;
                }

                Dictionary<string, dynamic> result = new Dictionary<string, dynamic>();
                foreach(MultitaskerVariable multitaskerVariable in dictionary.Values)
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
