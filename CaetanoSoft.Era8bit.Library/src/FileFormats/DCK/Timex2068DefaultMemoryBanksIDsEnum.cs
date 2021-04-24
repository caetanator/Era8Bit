using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// This enumeration contains the 3 default 64 KB memory banks that are implemented on the hardware of the 
    /// Timex/Sinclair 2068 (TS2068), Timex Computer 2068 (TC2068) and Unipolbrit Komputer 2086 (UK2086).
    /// <para>Banks IDs 1 to 253 are reserved for use with the never released BEU (Bus Expansion Unit) 
    /// to allow memory expansion up to 16 MB.</para>
    /// <para>The electrical signals for this banks are present on the expansion bus port and the cartridge port.
    /// Be aware that they differ on the TS2068 and the TC20688/UK2086. 
    /// Not all signals exist on the two ports, and if they exist on the connector, they probably are in a different 
    /// contact pin.</para>
    /// <para>
    /// Memory Bank ID:<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>0</term>
    ///			        <description>DOCK bank (TCC ROM and/or RAM up to 64 KB)</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>1-253</term>
    ///			        <description>Reserved banks (BEU RAM memory expansions of 64 KB)</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>254</term>
    ///			        <description>EXROM bank (Timex 2068 8 KB Extension ROM)</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>255</term>
    ///			        <description>HOME bank (Timex 2068 16 KB Home ROM + 48 KB RAM)</description>
    ///			    </item>
    ///			</list>
    /// </para>
    /// </summary>
    public enum Timex2068DefaultMemoryBanksIDsEnum : byte
    {
        /// <summary>
        /// 8 KB to 64K RAM and/or ROM Timex cartridge format, used for programs in Timex BASIC and/or Zilog Z80 machine code.
        /// <para>This format have to types: LROS (Language ROM Oriented software), AROS (Application ROM Oriented software).</para>
        /// <para>"Timex Chess" and "Timex Crazy Bugs" use this format.</para>
        /// </summary>
        DOCK = 0,

        /// <summary>
        /// Replaces the TS2068/TC2068/UK2086 8 KB Extended ROM (starts at 4000h) by the one on the Timex cartridge format by ROM or RAM.
        /// <para> use this bank.</para>
        /// </summary>
        EXROM = 254,

        /// <summary>
        /// Replaces the TS2068/TC2068/UK2086 16 KB Home ROM (starts at 0000H) by the one on the Timex cartridge ROM or RAM.
        /// <para>"ZX Spectrum 16/48 KB", "Timex Computer 2048 Emulator" and "Timex Time Word" use this bank.</para>
        /// </summary>
        HOME = 255
    };
}
