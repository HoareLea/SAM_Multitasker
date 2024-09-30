using Newtonsoft.Json.Linq;
using System;
using System.Drawing;

namespace SAM.Core.Multitasker
{
    public class MultitaskerOutput : IJSAMObject
    {
        private ValueType valueType;
        private object result;

        public MultitaskerOutput(object result)
        {
            this.result = result;
            valueType = result == null ? ValueType.Undefined : result.ValueType();
        }

        public MultitaskerOutput(MultitaskerOutput multitaskerOutput)
        {
            if(multitaskerOutput != null)
            {
                valueType = multitaskerOutput.valueType;
                result = multitaskerOutput.result;
            }
        }

        public MultitaskerOutput(JObject jObject)
        {
            FromJObject(jObject);
        }

        public object Result
        {
            get
            {
                return result;
            }
        }

        public bool FromJObject(JObject jObject)
        {
            if(jObject.ContainsKey("ValueType"))
            {
                valueType = Query.Enum<ValueType>(jObject.Value<string>("ValueType"));
            }

            if(jObject.ContainsKey("Result"))
            {
                switch (valueType)
                {
                    case ValueType.Boolean:
                        result = jObject.Value<bool>("Result");
                        return true;

                    case ValueType.Color:
                        result = new SAMColor(jObject.Value<JObject>("Result")).ToColor();
                        return true;

                    case ValueType.DateTime:
                        result = jObject.Value<DateTime>("Result");
                        return true;

                    case ValueType.Double:
                        result = jObject.Value<double>("Result");
                        return true;

                    case ValueType.Guid:
                        if (!Enum.TryParse(jObject.Value<string>("Result"), out Guid guid))
                        {
                            return false;
                        }
                        result = guid;
                        return true;

                    case ValueType.IJSAMObject:
                        JObject jObject_Temp = jObject.Value<JObject>("Result");
                        if (jObject_Temp == null)
                        {
                            return false;
                        }

                        result = new JSAMObjectWrapper(jObject_Temp).ToIJSAMObject();
                        return true;

                    case ValueType.Integer:
                        result = jObject.Value<int>("Result");
                        return true;

                    case ValueType.String:
                        result = jObject.Value<string>("Result");
                        return true;
                }
            }

            return true;
        }

        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Query.FullTypeName(this));

            jObject.Add("ValueType", valueType.ToString());

            if (valueType != ValueType.Undefined)
            {
                object value = null;
                switch (valueType)
                {
                    case ValueType.Boolean:
                        if (Query.TryConvert(result, out bool @bool))
                        {
                            value = @bool;
                        }
                        break;

                    case ValueType.Color:
                        if (Query.TryConvert(result, out Color color))
                        {
                            value = new SAMColor(color).ToJObject();
                        }
                        break;

                    case ValueType.DateTime:
                        if (Query.TryConvert(result, out DateTime dateTime))
                        {
                            value = dateTime;
                        }
                        break;

                    case ValueType.Double:
                        if (Query.TryConvert(result, out double @double))
                        {
                            value = @double;
                        }
                        break;

                    case ValueType.Guid:
                        if (Query.TryConvert(result, out Guid @guid))
                        {
                            value = @guid;
                        }
                        break;

                    case ValueType.IJSAMObject:
                        value = ((IJSAMObject)result).ToJObject();
                        break;

                    case ValueType.Integer:
                        if (Query.TryConvert(result, out int @int))
                        {
                            value = @int;
                        }
                        break;

                    case ValueType.String:
                        if (Query.TryConvert(result, out string @string))
                        {
                            value = @string;
                        }
                        break;
                }

                if (value != null)
                {
                    jObject.Add("Result", value as dynamic);
                }
            }

            return jObject;
        }
    }
}
