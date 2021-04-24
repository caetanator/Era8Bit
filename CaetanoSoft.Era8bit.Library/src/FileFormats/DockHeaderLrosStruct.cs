using System.Runtime.InteropServices;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// This little-endian structure must be present at memory address 0 (0000H) in all Timex Command Cartridge 
    /// that contains a DOCK memory bank with a LROS (Language ROM Oriented Software) program.<br />
    /// The contained program must be written in Z80 Machine Code and this information helps to execute it.<br />
    /// All LROS cartridges will auto-start as soon the Operating System initialization is complete.
    /// <para>See <see href="https://timex.comboios.info/tscart.html">Timex Computer World - Timex Command Cartridges</see> for more information.</para>
    /// </summary>
    /// <remarks>
    /// The 5 bytes little-endian structure at memory location 0 (0000H), as the following representation:
    /// <para>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Location</term>
    ///         <description>Description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>0<br />(0000H)<br /></term>
    ///         <description>
    ///			Language Type:<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>0</term>
    ///			        <description>LROS =&gt; Not used</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>1</term>
    ///			        <description>AROS =&gt; Timex BASIC (with optional machine code)</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>2</term>
    ///			        <description>AROS =&gt; Z80 Machine Code (only)</description>
    ///			    </item>
    ///         </list>
    ///         <strong>NOTE:</strong> Any other value will result in: Error S, Missing LROS.
    ///			</description>
    ///     </item>
    ///     <item>
    ///         <term>1<br />(0001H)<br /></term>
    ///         <description>
    ///			Cartridge Type:<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>1</term>
    ///			        <description>LROS</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>2</term>
    ///			        <description>AROS</description>
    ///			    </item>
    ///			</list>
    ///			</description>
    ///     </item>
    ///     <item>
    ///         <term>2-3<br />(0002H-0003H)<br /></term>
    ///         <description>
    ///			Starting Code (LSB/MSB):<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>LROS =&gt; Machine Code</term>
    ///			        <description>Address to the Z80 instruction be jumped to after the Operating System initialization is complete.</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>AROS =&gt; BASIC</term>
    ///			        <description>Line number on the BASIC program to run after the program initialization is complete.</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>AROS =&gt; Machine Code</term>
    ///			        <description>Address to the Z80 instruction be jumped to after the program initialization is complete.</description>
    ///			    </item>
    ///			</list>
    ///			</description>
    ///     </item>
    ///     <item>
    ///         <term>4<br />(0004H)<br /></term>
    ///         <description>
    ///			Memory Chunk Specification:<br />
    ///         Bits 0-7 represent chunks 0-7 respectively in the DOCK bank in Low Active format as follows:<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>0</term>
    ///			        <description>In use</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>1</term>
    ///			        <description>Not in use</description>
    ///			    </item>
    ///			</list>
    ///         <strong>NOTE:</strong> When writing to the Horizontal Select Register (Port F4H), the Chunk Specification is High Active format.
    ///			</description>
    ///     </item>
    /// </list>
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 5)]
    public struct DockHeaderLrosStruct
    {
        /// <summary>
        /// Not used for DOCK memory bank cartridge LROS type, must be 0.
        /// <para>See <see cref="CaetanoSoft.Era8bit.FileFormats.DCK.DockHeaderLanguageTypeEnum" />.</para>
        /// </summary>
        public byte LanguageType;

        /// <summary>
        /// DOCK memory bank cartridge LROS type, must be 1.
        /// <para>See <see cref="CaetanoSoft.Era8bit.FileFormats.DCK.DockHeaderCartridgeTypeEnum" />.</para>
        /// </summary>
        public byte CartridgeType;

        /// <summary>
        /// The Zilog Z80 machine code address of the instruction to start/jump to 
        /// (stored in LSB/MSB order, aka little-endian)
        /// </summary>
        public ushort StartingCode;

        /// <summary>
        /// The memory chunk specification. When writing to the Horizontal Select Register (Port F4H), 
        /// the Chunk Specification is High Active
        /// </summary>
        public byte MemoryChunkSpecification;
    }
}
