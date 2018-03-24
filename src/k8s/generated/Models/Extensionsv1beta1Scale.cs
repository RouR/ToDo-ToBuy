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
    /// represents a scaling request for a resource.
    /// </summary>
    public partial class Extensionsv1beta1Scale
    {
        /// <summary>
        /// Initializes a new instance of the Extensionsv1beta1Scale class.
        /// </summary>
        public Extensionsv1beta1Scale()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the Extensionsv1beta1Scale class.
        /// </summary>
        /// <param name="apiVersion">APIVersion defines the versioned schema of
        /// this representation of an object. Servers should convert recognized
        /// schemas to the latest internal value, and may reject unrecognized
        /// values. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#resources</param>
        /// <param name="kind">Kind is a string value representing the REST
        /// resource this object represents. Servers may infer this from the
        /// endpoint the client submits requests to. Cannot be updated. In
        /// CamelCase. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#types-kinds</param>
        /// <param name="metadata">Standard object metadata; More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#metadata.</param>
        /// <param name="spec">defines the behavior of the scale. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#spec-and-status.</param>
        /// <param name="status">current status of the scale. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#spec-and-status.
        /// Read-only.</param>
        public Extensionsv1beta1Scale(string apiVersion = default(string), string kind = default(string), V1ObjectMeta metadata = default(V1ObjectMeta), Extensionsv1beta1ScaleSpec spec = default(Extensionsv1beta1ScaleSpec), Extensionsv1beta1ScaleStatus status = default(Extensionsv1beta1ScaleStatus))
        {
            ApiVersion = apiVersion;
            Kind = kind;
            Metadata = metadata;
            Spec = spec;
            Status = status;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets aPIVersion defines the versioned schema of this
        /// representation of an object. Servers should convert recognized
        /// schemas to the latest internal value, and may reject unrecognized
        /// values. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#resources
        /// </summary>
        [JsonProperty(PropertyName = "apiVersion")]
        public string ApiVersion { get; set; }

        /// <summary>
        /// Gets or sets kind is a string value representing the REST resource
        /// this object represents. Servers may infer this from the endpoint
        /// the client submits requests to. Cannot be updated. In CamelCase.
        /// More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#types-kinds
        /// </summary>
        [JsonProperty(PropertyName = "kind")]
        public string Kind { get; set; }

        /// <summary>
        /// Gets or sets standard object metadata; More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#metadata.
        /// </summary>
        [JsonProperty(PropertyName = "metadata")]
        public V1ObjectMeta Metadata { get; set; }

        /// <summary>
        /// Gets or sets defines the behavior of the scale. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#spec-and-status.
        /// </summary>
        [JsonProperty(PropertyName = "spec")]
        public Extensionsv1beta1ScaleSpec Spec { get; set; }

        /// <summary>
        /// Gets or sets current status of the scale. More info:
        /// https://git.k8s.io/community/contributors/devel/api-conventions.md#spec-and-status.
        /// Read-only.
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public Extensionsv1beta1ScaleStatus Status { get; set; }

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
            if (Status != null)
            {
                Status.Validate();
            }
        }
    }
}
