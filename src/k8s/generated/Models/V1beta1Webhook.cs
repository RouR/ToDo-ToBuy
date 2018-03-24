// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace k8s.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Webhook describes an admission webhook and the resources and operations
    /// it applies to.
    /// </summary>
    public partial class V1beta1Webhook
    {
        /// <summary>
        /// Initializes a new instance of the V1beta1Webhook class.
        /// </summary>
        public V1beta1Webhook()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1beta1Webhook class.
        /// </summary>
        /// <param name="clientConfig">ClientConfig defines how to communicate
        /// with the hook. Required</param>
        /// <param name="name">The name of the admission webhook. Name should
        /// be fully qualified, e.g., imagepolicy.kubernetes.io, where
        /// "imagepolicy" is the name of the webhook, and kubernetes.io is the
        /// name of the organization. Required.</param>
        /// <param name="failurePolicy">FailurePolicy defines how unrecognized
        /// errors from the admission endpoint are handled - allowed values are
        /// Ignore or Fail. Defaults to Ignore.</param>
        /// <param name="namespaceSelector">NamespaceSelector decides whether
        /// to run the webhook on an object based on whether the namespace for
        /// that object matches the selector. If the object itself is a
        /// namespace, the matching is performed on object.metadata.labels. If
        /// the object is other cluster scoped resource, it is not subjected to
        /// the webhook.
        ///
        /// For example, to run the webhook on any objects whose namespace is
        /// not associated with "runlevel" of "0" or "1";  you will set the
        /// selector as follows: "namespaceSelector": {
        /// "matchExpressions": [
        /// {
        /// "key": "runlevel",
        /// "operator": "NotIn",
        /// "values": [
        /// "0",
        /// "1"
        /// ]
        /// }
        /// ]
        /// }
        ///
        /// If instead you want to only run the webhook on any objects whose
        /// namespace is associated with the "environment" of "prod" or
        /// "staging"; you will set the selector as follows:
        /// "namespaceSelector": {
        /// "matchExpressions": [
        /// {
        /// "key": "environment",
        /// "operator": "In",
        /// "values": [
        /// "prod",
        /// "staging"
        /// ]
        /// }
        /// ]
        /// }
        ///
        /// See
        /// https://kubernetes.io/docs/concepts/overview/working-with-objects/labels/
        /// for more examples of label selectors.
        ///
        /// Default to the empty LabelSelector, which matches
        /// everything.</param>
        /// <param name="rules">Rules describes what operations on what
        /// resources/subresources the webhook cares about. The webhook cares
        /// about an operation if it matches _any_ Rule.</param>
        public V1beta1Webhook(V1beta1WebhookClientConfig clientConfig, string name, string failurePolicy = default(string), V1LabelSelector namespaceSelector = default(V1LabelSelector), IList<V1beta1RuleWithOperations> rules = default(IList<V1beta1RuleWithOperations>))
        {
            ClientConfig = clientConfig;
            FailurePolicy = failurePolicy;
            Name = name;
            NamespaceSelector = namespaceSelector;
            Rules = rules;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets clientConfig defines how to communicate with the hook.
        /// Required
        /// </summary>
        [JsonProperty(PropertyName = "clientConfig")]
        public V1beta1WebhookClientConfig ClientConfig { get; set; }

        /// <summary>
        /// Gets or sets failurePolicy defines how unrecognized errors from the
        /// admission endpoint are handled - allowed values are Ignore or Fail.
        /// Defaults to Ignore.
        /// </summary>
        [JsonProperty(PropertyName = "failurePolicy")]
        public string FailurePolicy { get; set; }

        /// <summary>
        /// Gets or sets the name of the admission webhook. Name should be
        /// fully qualified, e.g., imagepolicy.kubernetes.io, where
        /// "imagepolicy" is the name of the webhook, and kubernetes.io is the
        /// name of the organization. Required.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets namespaceSelector decides whether to run the webhook
        /// on an object based on whether the namespace for that object matches
        /// the selector. If the object itself is a namespace, the matching is
        /// performed on object.metadata.labels. If the object is other cluster
        /// scoped resource, it is not subjected to the webhook.
        ///
        /// For example, to run the webhook on any objects whose namespace is
        /// not associated with "runlevel" of "0" or "1";  you will set the
        /// selector as follows: "namespaceSelector": {
        /// "matchExpressions": [
        /// {
        /// "key": "runlevel",
        /// "operator": "NotIn",
        /// "values": [
        /// "0",
        /// "1"
        /// ]
        /// }
        /// ]
        /// }
        ///
        /// If instead you want to only run the webhook on any objects whose
        /// namespace is associated with the "environment" of "prod" or
        /// "staging"; you will set the selector as follows:
        /// "namespaceSelector": {
        /// "matchExpressions": [
        /// {
        /// "key": "environment",
        /// "operator": "In",
        /// "values": [
        /// "prod",
        /// "staging"
        /// ]
        /// }
        /// ]
        /// }
        ///
        /// See
        /// https://kubernetes.io/docs/concepts/overview/working-with-objects/labels/
        /// for more examples of label selectors.
        ///
        /// Default to the empty LabelSelector, which matches everything.
        /// </summary>
        [JsonProperty(PropertyName = "namespaceSelector")]
        public V1LabelSelector NamespaceSelector { get; set; }

        /// <summary>
        /// Gets or sets rules describes what operations on what
        /// resources/subresources the webhook cares about. The webhook cares
        /// about an operation if it matches _any_ Rule.
        /// </summary>
        [JsonProperty(PropertyName = "rules")]
        public IList<V1beta1RuleWithOperations> Rules { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ClientConfig == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ClientConfig");
            }
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (ClientConfig != null)
            {
                ClientConfig.Validate();
            }
        }
    }
}
