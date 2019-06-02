using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public class Elector
    {
        public string CredentialId { get; protected set; }

        public District District { get; protected set; }

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
