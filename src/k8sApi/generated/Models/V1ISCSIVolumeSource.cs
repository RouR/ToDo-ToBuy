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
    /// Represents an ISCSI disk. ISCSI volumes can only be mounted as
    /// read/write once. ISCSI volumes support ownership management and SELinux
    /// relabeling.
    /// </summary>
    public partial class V1ISCSIVolumeSource
    {
        /// <summary>
        /// Initializes a new instance of the V1ISCSIVolumeSource class.
        /// </summary>
        public V1ISCSIVolumeSource()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the V1ISCSIVolumeSource class.
        /// </summary>
        /// <param name="iqn">Target iSCSI Qualified Name.</param>
        /// <param name="lun">iSCSI target lun number.</param>
        /// <param name="targetPortal">iSCSI target portal. The portal is
        /// either an IP or ip_addr:port if the port is other than default
        /// (typically TCP ports 860 and 3260).</param>
        /// <param name="chapAuthDiscovery">whether support iSCSI Discovery
        /// CHAP authentication</param>
        /// <param name="chapAuthSession">whether support iSCSI Session CHAP
        /// authentication</param>
        /// <param name="fsType">Filesystem type of the volume that you want to
        /// mount. Tip: Ensure that the filesystem type is supported by the
        /// host operating system. Examples: "ext4", "xfs", "ntfs". Implicitly
        /// inferred to be "ext4" if unspecified. More info:
        /// https://kubernetes.io/docs/concepts/storage/volumes#iscsi</param>
        /// <param name="initiatorName">Custom iSCSI initiator name. If
        /// initiatorName is specified with iscsiInterface simultaneously, new
        /// iSCSI interface &lt;target portal&gt;:&lt;volume name&gt; will be
        /// created for the connection.</param>
        /// <param name="iscsiInterface">Optional: Defaults to 'default' (tcp).
        /// iSCSI interface name that uses an iSCSI transport.</param>
        /// <param name="portals">iSCSI target portal List. The portal is
        /// either an IP or ip_addr:port if the port is other than default
        /// (typically TCP ports 860 and 3260).</param>
        /// <param name="readOnlyProperty">ReadOnly here will force the
        /// ReadOnly setting in VolumeMounts. Defaults to false.</param>
        /// <param name="secretRef">CHAP secret for iSCSI target and initiator
        /// authentication</param>
        public V1ISCSIVolumeSource(string iqn, int lun, string targetPortal, bool? chapAuthDiscovery = default(bool?), bool? chapAuthSession = default(bool?), string fsType = default(string), string initiatorName = default(string), string iscsiInterface = default(string), IList<string> portals = default(IList<string>), bool? readOnlyProperty = default(bool?), V1LocalObjectReference secretRef = default(V1LocalObjectReference))
        {
            ChapAuthDiscovery = chapAuthDiscovery;
            ChapAuthSession = chapAuthSession;
            FsType = fsType;
            InitiatorName = initiatorName;
            Iqn = iqn;
            IscsiInterface = iscsiInterface;
            Lun = lun;
            Portals = portals;
            ReadOnlyProperty = readOnlyProperty;
            SecretRef = secretRef;
            TargetPortal = targetPortal;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets whether support iSCSI Discovery CHAP authentication
        /// </summary>
        [JsonProperty(PropertyName = "chapAuthDiscovery")]
        public bool? ChapAuthDiscovery { get; set; }

        /// <summary>
        /// Gets or sets whether support iSCSI Session CHAP authentication
        /// </summary>
        [JsonProperty(PropertyName = "chapAuthSession")]
        public bool? ChapAuthSession { get; set; }

        /// <summary>
        /// Gets or sets filesystem type of the volume that you want to mount.
        /// Tip: Ensure that the filesystem type is supported by the host
        /// operating system. Examples: "ext4", "xfs", "ntfs". Implicitly
        /// inferred to be "ext4" if unspecified. More info:
        /// https://kubernetes.io/docs/concepts/storage/volumes#iscsi
        /// </summary>
        [JsonProperty(PropertyName = "fsType")]
        public string FsType { get; set; }

        /// <summary>
        /// Gets or sets custom iSCSI initiator name. If initiatorName is
        /// specified with iscsiInterface simultaneously, new iSCSI interface
        /// &amp;lt;target portal&amp;gt;:&amp;lt;volume name&amp;gt; will be
        /// created for the connection.
        /// </summary>
        [JsonProperty(PropertyName = "initiatorName")]
        public string InitiatorName { get; set; }

        /// <summary>
        /// Gets or sets target iSCSI Qualified Name.
        /// </summary>
        [JsonProperty(PropertyName = "iqn")]
        public string Iqn { get; set; }

        /// <summary>
        /// Gets or sets optional: Defaults to 'default' (tcp). iSCSI interface
        /// name that uses an iSCSI transport.
        /// </summary>
        [JsonProperty(PropertyName = "iscsiInterface")]
        public string IscsiInterface { get; set; }

        /// <summary>
        /// Gets or sets iSCSI target lun number.
        /// </summary>
        [JsonProperty(PropertyName = "lun")]
        public int Lun { get; set; }

        /// <summary>
        /// Gets or sets iSCSI target portal List. The portal is either an IP
        /// or ip_addr:port if the port is other than default (typically TCP
        /// ports 860 and 3260).
        /// </summary>
        [JsonProperty(PropertyName = "portals")]
        public IList<string> Portals { get; set; }

        /// <summary>
        /// Gets or sets readOnly here will force the ReadOnly setting in
        /// VolumeMounts. Defaults to false.
        /// </summary>
        [JsonProperty(PropertyName = "readOnly")]
        public bool? ReadOnlyProperty { get; set; }

        /// <summary>
        /// Gets or sets CHAP secret for iSCSI target and initiator
        /// authentication
        /// </summary>
        [JsonProperty(PropertyName = "secretRef")]
        public V1LocalObjectReference SecretRef { get; set; }

        /// <summary>
        /// Gets or sets iSCSI target portal. The portal is either an IP or
        /// ip_addr:port if the port is other than default (typically TCP ports
        /// 860 and 3260).
        /// </summary>
        [JsonProperty(PropertyName = "targetPortal")]
        public string TargetPortal { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Iqn == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Iqn");
            }
            if (TargetPortal == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TargetPortal");
            }
        }
    }
}
