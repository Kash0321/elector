using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kash.Elector
{
    public class Elector
    {
        [Key]
        [MaxLength(36)]
        public string CredentialId { get; protected set; }

        public District District { get; protected set; }

        protected Elector() { }

        public Elector(string credentialId, District district)
        {
            Check.NotNull(credentialId, nameof(credentialId));
            Check.NotNull(district, nameof(district));

            CredentialId = credentialId;
            District = district;
        }

        public override string ToString()
        {
            return $"{CredentialId}, {District.Name}";
        }
    }
}
