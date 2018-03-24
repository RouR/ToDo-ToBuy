// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// JSON represents any valid JSON value. These types are supported: bool,
    /// int64, float64, string, []interface{}, map[string]interface{} and nil.
    /// </summary>
    public partial class V1beta1JSON
    {
        /// <summary>
        /// Initializes a new instance of the V1beta1JSON class.
        /// </summary>
        public V1beta1JSON()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1beta1JSON class.
        /// </summary>
        public V1beta1JSON(byte[] raw)
        {
            Raw = raw;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Raw")]
        public byte[] Raw { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Raw == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Raw");
            }
        }
    }
}
