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
    /// DaemonSetStatus represents the current status of a daemon set.
    /// </summary>
    public partial class V1beta1DaemonSetStatus
    {
        /// <summary>
        /// Initializes a new instance of the V1beta1DaemonSetStatus class.
        /// </summary>
        public V1beta1DaemonSetStatus()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1beta1DaemonSetStatus class.
        /// </summary>
        /// <param name="currentNumberScheduled">The number of nodes that are
        /// running at least 1 daemon pod and are supposed to run the daemon
        /// pod. More info:
        /// https://kubernetes.io/docs/concepts/workloads/controllers/daemonset/</param>
        /// <param name="desiredNumberScheduled">The total number of nodes that
        /// should be running the daemon pod (including nodes correctly running
        /// the daemon pod). More info:
        /// https://kubernetes.io/docs/concepts/workloads/controllers/daemonset/</param>
        /// <param name="numberMisscheduled">The number of nodes that are
        /// running the daemon pod, but are not supposed to run the daemon pod.
        /// More info:
        /// https://kubernetes.io/docs/concepts/workloads/controllers/daemonset/</param>
        /// <param name="numberReady">The number of nodes that should be
        /// running the daemon pod and have one or more of the daemon pod
        /// running and ready.</param>
        /// <param name="collisionCount">Count of hash collisions for the
        /// DaemonSet. The DaemonSet controller uses this field as a collision
        /// avoidance mechanism when it needs to create the name for the newest
        /// ControllerRevision.</param>
        /// <param name="numberAvailable">The number of nodes that should be
        /// running the daemon pod and have one or more of the daemon pod
        /// running and available (ready for at least
        /// spec.minReadySeconds)</param>
        /// <param name="numberUnavailable">The number of nodes that should be
        /// running the daemon pod and have none of the daemon pod running and
        /// available (ready for at least spec.minReadySeconds)</param>
        /// <param name="observedGeneration">The most recent generation
        /// observed by the daemon set controller.</param>
        /// <param name="updatedNumberScheduled">The total number of nodes that
        /// are running updated daemon pod</param>
        public V1beta1DaemonSetStatus(int currentNumberScheduled, int desiredNumberScheduled, int numberMisscheduled, int numberReady, int? collisionCount = default(int?), int? numberAvailable = default(int?), int? numberUnavailable = default(int?), long? observedGeneration = default(long?), int? updatedNumberScheduled = default(int?))
        {
            CollisionCount = collisionCount;
            CurrentNumberScheduled = currentNumberScheduled;
            DesiredNumberScheduled = desiredNumberScheduled;
            NumberAvailable = numberAvailable;
            NumberMisscheduled = numberMisscheduled;
            NumberReady = numberReady;
            NumberUnavailable = numberUnavailable;
            ObservedGeneration = observedGeneration;
            UpdatedNumberScheduled = updatedNumberScheduled;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets count of hash collisions for the DaemonSet. The
        /// DaemonSet controller uses this field as a collision avoidance
        /// mechanism when it needs to create the name for the newest
        /// ControllerRevision.
        /// </summary>
        [JsonProperty(PropertyName = "collisionCount")]
        public int? CollisionCount { get; set; }

        /// <summary>
        /// Gets or sets the number of nodes that are running at least 1 daemon
        /// pod and are supposed to run the daemon pod. More info:
        /// https://kubernetes.io/docs/concepts/workloads/controllers/daemonset/
        /// </summary>
        [JsonProperty(PropertyName = "currentNumberScheduled")]
        public int CurrentNumberScheduled { get; set; }

        /// <summary>
        /// Gets or sets the total number of nodes that should be running the
        /// daemon pod (including nodes correctly running the daemon pod). More
        /// info:
        /// https://kubernetes.io/docs/concepts/workloads/controllers/daemonset/
        /// </summary>
        [JsonProperty(PropertyName = "desiredNumberScheduled")]
        public int DesiredNumberScheduled { get; set; }

        /// <summary>
        /// Gets or sets the number of nodes that should be running the daemon
        /// pod and have one or more of the daemon pod running and available
        /// (ready for at least spec.minReadySeconds)
        /// </summary>
        [JsonProperty(PropertyName = "numberAvailable")]
        public int? NumberAvailable { get; set; }

        /// <summary>
        /// Gets or sets the number of nodes that are running the daemon pod,
        /// but are not supposed to run the daemon pod. More info:
        /// https://kubernetes.io/docs/concepts/workloads/controllers/daemonset/
        /// </summary>
        [JsonProperty(PropertyName = "numberMisscheduled")]
        public int NumberMisscheduled { get; set; }

        /// <summary>
        /// Gets or sets the number of nodes that should be running the daemon
        /// pod and have one or more of the daemon pod running and ready.
        /// </summary>
        [JsonProperty(PropertyName = "numberReady")]
        public int NumberReady { get; set; }

        /// <summary>
        /// Gets or sets the number of nodes that should be running the daemon
        /// pod and have none of the daemon pod running and available (ready
        /// for at least spec.minReadySeconds)
        /// </summary>
        [JsonProperty(PropertyName = "numberUnavailable")]
        public int? NumberUnavailable { get; set; }

        /// <summary>
        /// Gets or sets the most recent generation observed by the daemon set
        /// controller.
        /// </summary>
        [JsonProperty(PropertyName = "observedGeneration")]
        public long? ObservedGeneration { get; set; }

        /// <summary>
        /// Gets or sets the total number of nodes that are running updated
        /// daemon pod
        /// </summary>
        [JsonProperty(PropertyName = "updatedNumberScheduled")]
        public int? UpdatedNumberScheduled { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            //Nothing to validate
        }
    }
}
