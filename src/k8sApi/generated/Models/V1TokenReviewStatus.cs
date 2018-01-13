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
    /// TokenReviewStatus is the result of the token authentication request.
    /// </summary>
    public partial class V1TokenReviewStatus
    {
        /// <summary>
        /// Initializes a new instance of the V1TokenReviewStatus class.
        /// </summary>
        public V1TokenReviewStatus()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1TokenReviewStatus class.
        /// </summary>
        /// <param name="authenticated">Authenticated indicates that the token
        /// was associated with a known user.</param>
        /// <param name="error">Error indicates that the token couldn't be
        /// checked</param>
        /// <param name="user">User is the UserInfo associated with the
        /// provided token.</param>
        public V1TokenReviewStatus(bool? authenticated = default(bool?), string error = default(string), V1UserInfo user = default(V1UserInfo))
        {
            Authenticated = authenticated;
            Error = error;
            User = user;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets authenticated indicates that the token was associated
        /// with a known user.
        /// </summary>
        [JsonProperty(PropertyName = "authenticated")]
        public bool? Authenticated { get; set; }

        /// <summary>
        /// Gets or sets error indicates that the token couldn't be checked
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets user is the UserInfo associated with the provided
        /// token.
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public V1UserInfo User { get; set; }

    }
}
