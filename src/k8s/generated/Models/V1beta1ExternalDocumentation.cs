// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// ExternalDocumentation allows referencing an external resource for
    /// extended documentation.
    /// </summary>
    public partial class V1beta1ExternalDocumentation
    {
        /// <summary>
        /// Initializes a new instance of the V1beta1ExternalDocumentation
        /// class.
        /// </summary>
        public V1beta1ExternalDocumentation()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1beta1ExternalDocumentation
        /// class.
        /// </summary>
        public V1beta1ExternalDocumentation(string description = default(string), string url = default(string))
        {
            Description = description;
            Url = url;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

    }
}
