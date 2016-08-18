namespace Escc.EastSussexGovUK
{
    /// <summary>
    /// A content security policy on which an <see cref="IClientDependencySet"/> depends
    /// </summary>
    public class ContentSecurityPolicyDependency
    {
        /// <summary>
        /// The alias of the content security policy, used with <see cref="ContentSecurityPolicy"/>
        /// </summary>
        public string Alias { get; set; }
    }
}