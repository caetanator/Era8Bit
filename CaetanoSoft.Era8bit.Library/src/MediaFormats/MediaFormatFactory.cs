/**
 * MediaFactory.cs
 *
 * PURPOSE
 *  Implements a factory to create the correct object to handle a media file format.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSoft.Era8bit.MediaFormats" project:
 *      José Caetano Silva, jcaetano@users.sourceforge.net
 *
 * COPYRIGHT
 *  This file is distributed under the terms of the GNU General Public
 *  License (GPL) v3. Copies of the GPL can be obtained from:
 *      ftp://prep.ai.mit.edu/pub/gnu/GPL
 *  Each contributing author retains all rights to their own work.
 *
 *  (C)2006-2017 José Caetano Silva
 *
 * HISTORY
 *  2017-03-30: Created.
 *  2021-04-23: Major re-wright.
 */

using System;

using CaetanoSoft.Era8bit.FileFormats.DCK;

namespace CaetanoSoft.Era8bit.MediaFormats
{
    /// <summary>
    /// Singleton pattern class that implements a factory to create the correct object to handle a specific 
    /// media file format.
    /// </summary>
    public sealed class MediaFormatFactory
    {
        /// <summary>
        /// Stores the unique instance of this Singleton Pattern object. The object instance is created lazy 
        /// (only on first instantiation).
        /// </summary>
        private static readonly Lazy<MediaFormatFactory> m_lazyInstance = new Lazy<MediaFormatFactory>(() => new MediaFormatFactory());

        /// <summary>Gets the unique instance of the this Singleton Pattern object.</summary>
        /// <value>The unique instance.</value>
        public static MediaFormatFactory Instance { get { return m_lazyInstance.Value; } }

        /// <summary>
        /// Prevents a default instance of the <see cref="MediaFormatFactory" /> class from being created.
        /// <para>This is a Singleton Pattern object, so can't be instantiated more than one time.</para>
        /// </summary>
        private MediaFormatFactory()
        {
            // Singleton pattern objects doesn't have public constructors
        }

        public IMediaFormat GetMediaHandler(String fileName)
        {
            IMediaFormat retObject = null;

            try
            {
                if (fileName.EndsWith(".dck", StringComparison.OrdinalIgnoreCase))
                {
                    retObject = new TimexCommandCartridge(fileName);
                }
				else if (fileName.EndsWith(".rom", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Not implemented
                    throw new NotImplementedException();
                }
                else if (fileName.EndsWith(".scr", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Not implemented
                    throw new NotImplementedException();
                }
				else if (fileName.EndsWith(".dsk", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Not implemented
                    throw new NotImplementedException();
                }
				else if (fileName.EndsWith(".tzx", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Not implemented
                    throw new NotImplementedException();
                }
                else if (fileName.EndsWith(".tap", StringComparison.OrdinalIgnoreCase))
                {
                    // TODO: Not implemented
                    throw new NotImplementedException();
                }
                else
                {
                    // TODO: Not implemented
                    throw new NotImplementedException();
                }
            }
            catch(Exception)
            {
                // Do nothing
            }

            return retObject;
        }
    }
}
