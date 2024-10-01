using Newtonsoft.Json.Linq;
using System.Drawing;
using System;

namespace SAM.Core.Multitasker
{
    public class MultitaskerVariable : IJSAMObject
    {
        private string name;
        private ValueType valueType;
        private object value;

        public MultitaskerVariable(string name, object value)
        {
            this.name = name;
            valueType = Core.Query.ValueType(value);
            this.value = value;
        }

        public MultitaskerVariable(string name, ValueType valueType)
        {
            this.name = name;
            this.valueType = valueType;
        }

        public MultitaskerVariable(string name, ValueType valueType, object value)
        {
            this.name = name;
            this.valueType = valueType;
            
            if(valueType == ValueType.Undefined || (value != null && valueType == Core.Query.ValueType(value)))
            {
                this.value = value;
            }
        }

        public MultitaskerVariable(MultitaskerVariable multitaskerVariable)
        { 
            if(multitaskerVariable != null)
            {
                name = multitaskerVariable.name;
                valueType = multitaskerVariable.valueType;
                value = multitaskerVariable.value;
            }
        }

        public MultitaskerVariable(JObject jObject)
        {
            FromJObject(jObject);   
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public ValueType ValueType
        {
            get
            {
                return valueType;
            }
        }

        public object Value
        {
            get
            {
                return value;
            }
        }

        public T GetObject<T>()
        {
            if (Core.Query.TryConvert(value, out T result))
            {
                return result;
            }

            return default;
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("Name"))
            {
                name = jObject.Value<string>("Name");
            }

            if (jObject.ContainsKey("ValueType"))
            {
                valueType = Core.Query.Enum<ValueType>(jObject.Value<string>("Name"));
            }

            if(jObject.ContainsKey("Value"))
            {
                object value = jObject.Value<object>("Value");
                switch(valueType)
                {
                    case ValueType.String:
                        if(Core.Query.TryConvert(value, out string @string))
                        {
                            value = @string;
                        }
                        break;

                    case ValueType.Boolean:
                        if (Core.Query.TryConvert(value, out bool @bool))
                        {
                            value = @bool;
                        }
                        break;

                    case ValueType.IJSAMObject:
                        if (Core.Query.TryConvert(value, out IJSAMObject jSAMObject))
                        {
                            value = jSAMObject;
                        }
                        break;

                    case ValueType.Integer:
                        if (Core.Query.TryConvert(value, out int @int))
                        {
                            value = @int;
                        }
                        break;

                    case ValueType.Double:
                        if (Core.Query.TryConvert(value, out double @double))
                        {
                            value = @double;
                        }
                        break;

                    case ValueType.Guid:
                        if (Core.Query.TryConvert(value, out Guid guid))
                        {
                            value = guid;
                        }
                        break;

                    case ValueType.DateTime:
                        if (Core.Query.TryConvert(value, out DateTime dateTime))
                        {
                            value = dateTime;
                        }
                        break;

                    case ValueType.Undefined:
                        if(value is JObject)
                        {
                            IJSAMObject jSAMObject_Undefined = Core.Query.IJSAMObject(jObject);
                            if (jSAMObject_Undefined != null)
                            {
                                value = jSAMObject_Undefined;
                            }
                        }
                        break;
                }

               this.value = value;
            }


            return true;
        }

        public JObject ToJObject()
        {
            JObject result = new JObject();
            result.Add("_type", Core.Query.FullTypeName(this));

            ValueType valueType = ValueType;
            result.Add("ValueType", valueType.ToString());

            if (valueType != ValueType.Undefined)
            {
                object value = null;
                switch (valueType)
                {
                    case ValueType.Boolean:
                        if (Core.Query.TryConvert(Value, out bool @bool))
                        {
                            value = @bool;
                        }
                        break;

                    case ValueType.Color:
                        if (Core.Query.TryConvert(Value, out Color color))
                        {
                            value = new SAMColor(color).ToJObject();
                        }
                        break;

                    case ValueType.DateTime:
                        if (Core.Query.TryConvert(Value, out DateTime dateTime))
                        {
                            value = dateTime;
                        }
                        break;

                    case ValueType.Double:
                        if (Core.Query.TryConvert(Value, out double @double))
                        {
                            value = @double;
                        }
                        break;

                    case ValueType.Guid:
                        if (Core.Query.TryConvert(Value, out Guid @guid))
                        {
                            value = @guid;
                        }
                        break;

                    case ValueType.IJSAMObject:
                        value = ((IJSAMObject)Value).ToJObject();
                        break;

                    case ValueType.Integer:
                        if (Core.Query.TryConvert(Value, out int @int))
                        {
                            value = @int;
                        }
                        break;

                    case ValueType.String:
                        if (Core.Query.TryConvert(Value, out string @string))
                        {
                            value = @string;
                        }
                        break;
                }

                if (value != null)
                {
                    result.Add("Value", value as dynamic);
                }
            }

            return result;
        }
    }
}
