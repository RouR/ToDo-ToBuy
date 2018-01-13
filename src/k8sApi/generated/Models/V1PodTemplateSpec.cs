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
    /// PodTemplateSpec describes the data a pod should have when created from
    /// a template
    /// </summary>
    public partial class V1PodTemplateSpec
    {
        /// <summary>
        /// Initializes a new instance of the V1PodTemplateSpec class.
        /// </summary>
        public V1PodTemplateSpec()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1PodTemplateSpec class.
        /// </summary>
        /// <param name="metadata">Standard object's metadata. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#metadata</param>
        /// <param name="spec">Specification of the desired behavior of the
        /// pod. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#spec-and-status</param>
        public V1PodTemplateSpec(V1ObjectMeta metadata = default(V1ObjectMeta), V1PodSpec spec = default(V1PodSpec))
        {
            Metadata = metadata;
            Spec = spec;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets standard object's metadata. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#metadata
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public V1ObjectMeta Metadata { get; set; }

        /// <summary>
        /// Gets or sets specification of the desired behavior of the pod. More
        /// info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#spec-and-status
        /// </summary>
        [JsonProperty(PropertyName = "spec")]
        public V1PodSpec Spec { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Metadata != null)
            {
                Metadata.Validate();
            }
            if (Spec != null)
            {
                Spec.Validate();
            }
        }
    }
}
