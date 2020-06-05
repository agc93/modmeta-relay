using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ModMeta.Core
{
    /// <summary>
    /// Information about a single file.
    /// </summary>
    public interface IModInfo
    {
        /// <summary>
        /// REQUIRED. File name for the given file. This is generally the file name on disk, including extension.
        /// </summary>
        /// <value>The file name, including extension.</value>
        string FileName { get; }

        /// <summary>
        /// File size in bytes of the given file.
        /// </summary>
        /// <value>The file size.</value>
        [JsonPropertyName("fileSizeBytes")]
        long FileSize { get; }

        /// <summary>
        /// The specific game this file is for use with.
        /// </summary>
        /// <value>The Nexus-style game ID.</value>
        string GameId { get; }

        /// <summary>
        /// The logical file name for this file.
        /// </summary>
        /// <value>The logical file name.</value>
        string LogicalFileName { get; }

        /// <summary>
        /// The version of this specific file.
        /// </summary>
        /// <value>Version of this file.</value>
        string FileVersion { get; }

        /// <summary>
        /// MD5 hash of this file (generally an archive file).
        /// </summary>
        /// <value>The file md5 hash.</value>
        [JsonPropertyName("fileMD5")]
        string FileMD5Hash { get; }

        /// <summary>
        /// A URI where this file can be found/downloaded.
        /// </summary>
        /// <value></value>
        [JsonPropertyName("sourceURI")]
        Uri SourceUrl { get; }

        /// <summary>
        /// The source name for this specific file.
        /// </summary>
        /// <remarks>It's possible a file can come from multiple places, this should be where *this* file is coming from.</remarks>
        /// <value>The source name.</value>
        string Source { get; }
        IList<IRule> Rules { get; }

        /// <summary>
        /// When this file's metadata should be considered out of date.
        /// </summary>
        /// <value>The ticks value for this record's expiration.</value>
        long Expires { get; }

        /// <summary>
        /// Mod metadata/details for this file.
        /// </summary>
        /// <value>The details for this file.</value>
        ModDetails Details { get; }
    }
}