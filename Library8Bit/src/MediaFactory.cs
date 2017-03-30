/**
 * MediaFactory.cs
 *
 * PURPOSE
 *  Implements a factory to create the correct object to handle a media file format.
 *
 * CONTACTS
 *  E-mail regarding any portion of the "CaetanoSof.Utils" project:
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
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CaetanoSof.Era8Bit.Library8Bit.MediaFormats
{
    /// <summary>
    /// Singleton pattern class that implements a factory to create the correct object to handle a specific media file format.
    /// </summary>
    public sealed class MediaFactory
    {
        private static readonly Lazy<MediaFactory> m_lazy = new Lazy<MediaFactory>(() => new MediaFactory());

        public static MediaFactory Instance { get { return m_lazy.Value; } }

        private MediaFactory()
        {
            // Singleton pattern objects dosen't have public constructors
        }

        public static IMediaFormat GetMediaHandler(String fileName)
        {
            IMediaFormat retObject = null;

            if(fileName.EndsWith(TimexCartridge.Extensions[0], StringComparison.OrdinalIgnoreCase))
            {
                retObject = new TimexCartridge(fileName);
            }

            return retObject;
        }
    }
}
