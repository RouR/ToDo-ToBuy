using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IServerValidation
    {
        /// <summary>
        /// (field name, message for user)
        /// </summary>
        List<KeyValuePair<string, string>> ValidationErrors { get; set; }
    }
}