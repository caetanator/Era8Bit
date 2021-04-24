using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// This enumeration contains the two possible program language type implementation on 
    /// AROS (Application ROM Oriented Software) and one for the LROS (Language ROM Oriented Software), 
    /// DOCK memory bank cartridge , for the Timex Command Cartridge (TCC) of a Timex/Sinclair 2068 (TS2068) 
    /// or Timex Computer 2068 (TC2068):
    /// <para>
    /// <list type="bullet">
    ///     <item>
    ///         <term>0</term>
    ///         <description>
    ///			LROS =&gt; Not Used [must be 0]
    ///			</description>
    ///     </item>
    ///     <item>
    ///         <term>1</term>
    ///         <description>
    ///			AROS =&gt; Timex 2068 BASIC [with optional machine code]
    ///			</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>
    ///			AROS =&gt; Zilog Z80 Machine Code [only]
    ///			</description>
    ///     </item>
    /// </list>
    /// </para>
    /// See <see cref="CaetanoSoft.Era8bit.FileFormats.DCK.DockHeaderArosStruct.LanguageType"/> and <see cref="CaetanoSoft.Era8bit.FileFormats.DCK.DockHeaderLrosStruct.LanguageType"/> 
    /// for more information.
    /// </summary>
    public enum DockHeaderLanguageTypeEnum : byte
    {
        /// <summary>
        /// LROS =&gt; Not used, ROM and LROS cartridges are always in Z80 machine code
        /// </summary>
        NotUsed = 0,

        /// <summary>
        /// AROS =&gt; Timex 2068 BASIC [with optional machine code]
        /// </summary>
        BASIC = 1,

        /// <summary>
        /// AROS =&gt; Zilog Z80 Machine Code [only]
        /// </summary>
        MACHINE_CODE = 2
    };
}
