
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading;
using Exceptionless;

namespace EsccWebTeam.EastSussexGovUK.MasterPages.Remote
{
    /// <summary>
    /// Cache remote template elements as files on disk
    /// </summary>
    public class RemoteMasterPageFileCacheProvider : RemoteMasterPageCacheProviderBase
    {
        readonly string _cacheFilename;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteMasterPageFileCacheProvider"/> class.
        /// </summary>
        /// <param name="control">A key identifying the control to cache.</param>
        /// <param name="selectedSection">A key representing the selected section of the site.</param>
        public RemoteMasterPageFileCacheProvider(string control, string selectedSection)
            : base(control, selectedSection)
        {
            SupportsAsync = true;

            // Get the filename we want to cache this control as
            var cacheToken = GetCacheToken();
            _cacheFilename = GetCacheFilename(cacheToken);

            // Work out when is the oldest cached response that's still valid
            var cacheThreshold = GetCacheThreshold();

            // Do we have the control cached already, and is the cache still fresh?
            CachedVersionExists = File.Exists(_cacheFilename);
            var currentCacheTime = File.GetLastWriteTimeUtc(_cacheFilename);
            CachedVersionIsFresh = CachedVersionExists ? (currentCacheTime >= cacheThreshold) : false;
        }

        /// <summary>
        /// Gets the cache filename from web.config.
        /// </summary>
        /// <param name="cacheToken">The cache token.</param>
        /// <returns></returns>
        private string GetCacheFilename(string cacheToken)
        {
            EnsureConfigurationSettings();

            if (String.IsNullOrEmpty(ConfigurationSettings["CachePath"]))
            {
                throw new ConfigurationErrorsException("web.config entry not found: <EsccWebTeam.EastSussexGovUK><RemoteMasterPage><add key=\"CachePath\" value=\"filename pattern including {0}\" /></RemoteMasterPage></EsccWebTeam.EastSussexGovUK>");
            }

            var folderPath = Path.GetDirectoryName(ConfigurationSettings["CachePath"]);
            if (!Directory.Exists(folderPath)) throw new ConfigurationErrorsException("Folder does not exist: " + folderPath);

            return String.Format(CultureInfo.CurrentCulture, ConfigurationSettings["CachePath"], cacheToken);
        }


        /// <summary>
        /// Saves the remote HTML to the cache.
        /// </summary>
        /// <param name="stream">The HTML stream.</param>
        public override void SaveRemoteHtmlToCache(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                using (var writer = new StreamWriter(_cacheFilename + ".temp", false))
                {
                    writer.WriteLine(reader.ReadToEnd());
                }
            }

            // Overwrite real cached file with temporary file. Idea is to minimise lock time on cached file.
            OverwriteFile(_cacheFilename, _cacheFilename + ".temp", 6);
        }

        /// <summary>
        /// Overwrites the cached HTML with the copy just downloaded.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="tempFile">The temp file.</param>
        /// <param name="attemptsLeft">The number of attempts left.</param>
        private static void OverwriteFile(string filename, string tempFile, int attemptsLeft)
        {
            try
            {
                File.Copy(tempFile, filename, true);
                File.Delete(tempFile);
            }
            catch (IOException ex)
            {
                // Possibility when it tries to update the file there's an IOException, because a web page
                // is in the process of reading the file. If we keep retrying we should be able to write it. 
                // Limited number of attempts though to avoid any risk of an infinite loop, or a conflict 
                // with the next request that tries to update it.
                if (attemptsLeft > 0)
                {
                    attemptsLeft--;
                    Thread.Sleep(10000); // wait 10 secs to allow file to become available
                    OverwriteFile(filename, tempFile, attemptsLeft);
                }
                else
                {
                    // Don't leave temp file hanging around
                    File.Delete(tempFile);

                    // Publish exception, otherwise it just disappears as async method has no calling code to throw to.
                    ex.ToExceptionless().Submit();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                // Publish exception, otherwise it just disappears as async method has no calling code to throw to.
                ex.ToExceptionless().Submit();
            }
        }

        /// <summary>
        /// Gets the best available cached response (up-to-date or not)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public override string ReadHtmlFromCache()
        {
            try
            {
                using (var reader = new StreamReader(_cacheFilename))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (FileNotFoundException x)
            {
                // If writing file hits UnauthorizedAccessException the file is not present, so this throws FileNotFoundException
                x.Data.Add("Probable cause", "The cached file could not be written because the application pool identity does not have write permissions to the cache folder.");
                throw;
            }

        }
    }
}