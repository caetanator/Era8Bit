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
    /// <copyright>(c) 2016-2017 by José Caetano Silva</copyright>
	/// <license type="GPL-3">See LICENSE for full terms</license>
    public sealed class MediaFactory
    {
        private static readonly Lazy<MediaFactory> m_lazy = new Lazy<MediaFactory>(() => new MediaFactory());

        public static MediaFactory Instance { get { return m_lazy.Value; } }

        private MediaFactory()
        {
            // Singleton pattern objects dosen't have public constructors
        }

        public IMediaFormat GetMediaHandler(String fileName)
        {
            IMediaFormat retObject = null;

            try
            {
                if (fileName.EndsWith(".dck", StringComparison.OrdinalIgnoreCase))
                {
                    retObject = new TimexCartridge(fileName);
                }
                else if (fileName.EndsWith(".tap", StringComparison.OrdinalIgnoreCase))
                {
                    // FIXME: Test only
                    retObject = new TimexCartridge(@"C:\Users\JCaetano\Desktop\Emulators\Sinclair\Programs\Timex TC2068\Cartridges\Timex Dock\Games\Chess\Chess.dck");
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
