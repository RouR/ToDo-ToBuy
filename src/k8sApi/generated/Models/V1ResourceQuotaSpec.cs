// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// ResourceQuotaSpec defines the desired hard limits to enforce for Quota.
    /// </summary>
    public partial class V1ResourceQuotaSpec
    {
        /// <summary>
        /// Initializes a new instance of the V1ResourceQuotaSpec class.
        /// </summary>
        public V1ResourceQuotaSpec()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1ResourceQuotaSpec class.
        /// </summary>
        /// <param name="hard">Hard is the set of desired hard limits for each
        /// named resource. More info:
        /// https://git.k8s.io/community/contributors/design-proposals/admission_control_resource_quota.md</param>
        /// <param name="scopes">A collection of filters that must match each
        /// object tracked by a quota. If not specified, the quota matches all
        /// objects.</param>
        public V1ResourceQuotaSpec(IDictionary<string, ResourceQuantity> hard = default(IDictionary<string, ResourceQuantity>), IList<string> scopes = default(IList<string>))
        {
            Hard = hard;
            Scopes = scopes;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets hard is the set of desired hard limits for each named
        /// resource. More info:
        /// https://git.k8s.io/community/contributors/design-proposals/admission_control_resource_quota.md
        /// </summary>
        [JsonProperty(PropertyName = "hard")]
        public IDictionary<string, ResourceQuantity> Hard { get; set; }

        /// <summary>
        /// Gets or sets a collection of filters that must match each object
        /// tracked by a quota. If not specified, the quota matches all
        /// objects.
        /// </summary>
        [JsonProperty(PropertyName = "scopes")]
        public IList<string> Scopes { get; set; }

    }
}
