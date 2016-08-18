namespace Escc.EastSussexGovUK
{
    public interface ILibraryCatalogueContext
    {
        /// <summary>
        /// Gets whether the user is on a library catalogue machine in a library.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if user is on a library catalogue machine; otherwise, <c>false</c>.
        /// </value>
        bool RequestIsFromLibraryCatalogueMachine();
    }
}