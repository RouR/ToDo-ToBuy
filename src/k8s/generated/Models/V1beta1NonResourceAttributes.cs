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
    /// NonResourceAttributes includes the authorization attributes available
    /// for non-resource requests to the Authorizer interface
    /// </summary>
    public partial class V1beta1NonResourceAttributes
    {
        /// <summary>
        /// Initializes a new instance of the V1beta1NonResourceAttributes
        /// class.
        /// </summary>
        public V1beta1NonResourceAttributes()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1beta1NonResourceAttributes
        /// class.
        /// </summary>
        /// <param name="path">Path is the URL path of the request</param>
        /// <param name="verb">Verb is the standard HTTP verb</param>
        public V1beta1NonResourceAttributes(string path = default(string), string verb = default(string))
        {
            Path = path;
            Verb = verb;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets path is the URL path of the request
        /// </summary>
        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets verb is the standard HTTP verb
        /// </summary>
        [JsonProperty(PropertyName = "verb")]
        public string Verb { get; set; }

    }
}
