/**
 * cascading-content.js
 * Filter data items which support cascade and inheritance settings, based on whether they are targeting the current page
 *
 * Note: You must filter inheritance settings before cascade settings. If an ancestor of the current page blocks inheritance 
 * it needs to be considered when deciding whether to inherit items further up the tree. This must happen before checking the
 * cascade settings, as the same item may get removed if it blocks cascade as well.
 * 
 * eg when evaluating item 3 in this tree, item 2 needs to be present when considering whether to inherit item 1, 
 *    but item 2 will be removed when filtering by cascade settings, so filter by inheritance, then cascade:
 * 
 * --- [1. has cascading item]
 *     --- [2. has item with no cascade, no inherit]
 *         --- [3. allows inherit, but should not inherit top-level item]
 * 

 * @author East Sussex County Council
 * @example 
  
 var data = [{TargetUrls:[string],Inherit:bool,Cascade:bool}]; // expects JSON data matching this schema
 var cascade = cascadingContentFilter();

 data = cascade.filterByUrl(data, window.location.pathname);
 data = cascade.filterByInherit(data, window.location.pathname);
 data = cascade.filterByCascade(data, window.location.pathname);
 data = cascade.filterIfBlank(data, function (item) { return item.isblank(); });
 
 if (data.length) {
      // do something
 }
 */
function cascadingContentFilter() {
    function filterIfBlank(items, isBlankStrategy) {
        /// <summary>Filters items based on the supplied strategy</summary>
        var filtered = [];

        $.each(items, function (key, item) {
            if (!isBlankStrategy(item)) {
                filtered.push(item);
            }
        });

        return filtered;
    }

    function filterByInherit(items, pageUrl) {
        /// <summary>Filters items based on their inheritance settings</summary>

        // If inheritance is blocked, that's a new base URL. Anything matching that URL or longer is valid.
        var minimumUrl = '';

        // Start by getting the deepest target URL which blocks inheritance.
        $.each(items, function (key, item) {
            if (!item.Inherit) {
                $.each(item.TargetUrls, function (key2, url) {
                    if (doesUrlMatchPage(url, pageUrl)) {
                        var minimumUrlCandidate = stripTrailingSlash(url);
                        if (minimumUrlCandidate.length > minimumUrl.length) { minimumUrl = minimumUrlCandidate; }
                    }
                });
            }
        });

        // If nothing blocks inheritance, return unchanged items
        if (!minimumUrl.length) return items;

        // Otherwise go through all the items, and select only those matching the new base URL or longer.
        var filtered = [];
        $.each(items, function (key, item) {
            $.each(item.TargetUrls, function (key2, url) {
                var normalisedUrl = stripTrailingSlash(url);
                if (normalisedUrl.indexOf(minimumUrl) === 0) {
                    filtered.push(item);
                    return false;
                }
                return true;
            });
        });

        return filtered;
    }

    function filterByCascade(items, urlToMatch) {
        /// <summary>Filters items based on their cascade settings</summary>
        var filtered = [];

        $.each(items, function (key, item) {
            if (isExactUrlMatch(item, urlToMatch) || item.Cascade) {
                filtered.push(item);
            }
        });

        return filtered;
    }

    function filterByUrl(items, pageUrl) {
        /// <summary>Filters items based on whether they start from the current URL or an ancestor</summary>
        var filtered = [];

        $.each(items, function (key, item) {
            if (hasUrlMatch(item, pageUrl)) {
                filtered.push(item);
            }
        });

        return filtered;
    }

    function hasUrlMatch(item, pageUrl) {
        /// <summary>Checks whether an item is displayed starting from the current URL or an ancestor</summary>
        var match = false;
        $.each(item.TargetUrls, function (key, url) {
            match = doesUrlMatchPage(url, pageUrl);
            return !match;
        });
        return match;
    }

    function doesUrlMatchPage(url, pageUrl) {
        /// <summary>Checks whether a URL matches the current URL or an child page</summary>
        var normalisedUrl = stripTrailingSlash(url);
        if (pageUrl.indexOf(normalisedUrl) === 0) {
            return true;
        }
        return false;
    }

    function isExactUrlMatch(item, urlToMatch) {
        /// <summary>Checks whether an item is displayed starting from the urlToMatch</summary>
        var pageUrl = stripTrailingSlash(urlToMatch);
        var match = false;
        $.each(item.TargetUrls, function (key, url) {
            var targetUrl = stripTrailingSlash(url);
            if (pageUrl === targetUrl) {
                match = true;
                return false;
            }
            return true;
        });
        return match;
    }

    function stripTrailingSlash(str) {
        /// <summary>Trim a trailing / from a string</summary>
        if (str.substr(str.length - 1) == '/') {
            return str.substr(0, str.length - 1);
        }
        return str;
    }

    return {
        filterByUrl: filterByUrl,
        filterByCascade: filterByCascade,
        filterByInherit: filterByInherit,
        filterIfBlank: filterIfBlank
    }
}