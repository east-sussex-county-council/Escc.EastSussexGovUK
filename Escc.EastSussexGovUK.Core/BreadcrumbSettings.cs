using System.Collections.Generic;

namespace Escc.EastSussexGovUK.Core
{
    /// <summary>
    /// An ordered collection of <see cref="BreadcrumbLevel"/>s which together represent a path to a position within the hierarchy of the site
    /// </summary>
    /// <remarks>This class exists to provide a unique type to bind configuration settings to</remarks>
    public class BreadcrumbSettings : List<BreadcrumbLevel>
    {
    }
}