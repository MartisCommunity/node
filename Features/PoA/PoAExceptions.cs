using System;

namespace XOuranos.Features.PoA
{
    public class NotAFederationMemberException : Exception
    {
        public NotAFederationMemberException() : base("Not a federation member!")
        {
        }
    }
}
