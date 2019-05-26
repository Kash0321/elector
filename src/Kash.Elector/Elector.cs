using Kash.CrossCutting.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kash.Elector
{
    public class Elector
    {
        public string CredentialId { get; protected set; }

        public string Name { get; protected set; }

        public District District { get; protected set; }

        public bool HasVoted { get; protected set; }

        public Elector(string credentialId, string name, District district)
        {
            Check.NotNull(credentialId, nameof(credentialId));
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(district, nameof(district));

            CredentialId = credentialId;
            Name = name;
            District = district;
        }
    }
}
