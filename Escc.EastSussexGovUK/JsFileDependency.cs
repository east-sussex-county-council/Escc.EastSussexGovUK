namespace Escc.EastSussexGovUK
{ 
    /// <summary>
    /// A JavaScript file on which an <see cref="IClientDependencySet"/> depends
    /// </summary>
    public class JsFileDependency
    {
        /// <summary>
        /// Creates a new <see cref="JsFileDependency"/>
        /// </summary>
        public JsFileDependency()
        {
            Priority = 100;
        }

        /// <summary>
        /// The alias of the JavaScript file, used with <see cref="Escc.ClientDependencyFramework"/>
        /// </summary>
        public string JsFileAlias { get; set; }
       
        /// <summary>
        /// The priority for loading the script, where 100 is normal.
        /// </summary>
        public int Priority { get; set; }
    }
}