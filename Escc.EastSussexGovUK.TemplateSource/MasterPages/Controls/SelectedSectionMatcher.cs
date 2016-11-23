using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace Escc.EastSussexGovUK.TemplateSource.MasterPages.Controls
{
    /// <summary>
    /// Match a plain English section name with a WebForms control representing the menu item for that section
    /// </summary>
    public class SelectedSectionMatcher
    {
        public HtmlContainerControl MatchSection(string selectedSection, HtmlContainerControl[] sections)
        {
            if (selectedSection.Length == 0) return null;

            selectedSection = NormaliseSelectedSection(selectedSection);

            var matchedSection = SelectedSectionIsExactMatch(selectedSection, sections);
            if (matchedSection == null) matchedSection = SelectedSectionIsFirstWordMatch(selectedSection, sections);
            return matchedSection;
        }

        private static HtmlContainerControl SelectedSectionIsExactMatch(string selectedSection, IEnumerable<HtmlContainerControl> sections)
        {
            foreach (var section in sections)
            {
                if (selectedSection == NormaliseSelectedSection(section.InnerText))
                {
                    return section;
                }
            }
            return null;
        }

        private static HtmlContainerControl SelectedSectionIsFirstWordMatch(string selectedSection, IEnumerable<HtmlContainerControl> sections)
        {
            var firstWord = FirstWordOf(selectedSection);
            foreach (var section in sections)
            {
                if (firstWord == FirstWordOf(NormaliseSelectedSection(section.InnerText)))
                {
                    return section;
                }
            }
            return null;
        }

        private static string FirstWordOf(string selectedSection)
        {
            var space = selectedSection.IndexOf(" ", StringComparison.OrdinalIgnoreCase);
            if (space == -1) return selectedSection;
            return selectedSection.Substring(0, space);
        }

        private static string NormaliseSelectedSection(string selectedSection)
        {
            selectedSection = selectedSection.ToUpperInvariant();
            selectedSection = selectedSection.Replace("&", String.Empty);
            selectedSection = selectedSection.Replace(" AND ", " ");
            selectedSection = selectedSection.Replace("<br />", " ");

            var normalise = new Regex("[^A-Z ]");
            selectedSection = normalise.Replace(selectedSection, String.Empty);

            return selectedSection;
        }
    }
}