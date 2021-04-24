using System.Runtime.InteropServices;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// This little-endian structure must be present at memory address 32768 (8000H) in all Timex Command Cartridge 
    /// that contains a DOCK memory bank with an AROS (Application ROM Oriented Software) program.<br />
    /// It serves to identify what kind of program is contained (Timex BASIC or Z80 Machine Code) and 
    /// other information to start it.
    /// <para>See <see href="https://timex.comboios.info/tscart.html">Timex Computer World - Timex Command Cartridges</see> for more information.</para>
    /// </summary>
    /// <remarks>
    /// The 8 bytes little-endian structure at memory location 32768 (8000H), as the following representation:
    /// <para>
    /// <list type="table">
    ///     <listheader>
    ///         <term>Location</term>
    ///         <description>Description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>32768<br />(8000H)<br /></term>
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
    ///         <term>32769<br />(8001H)<br /></term>
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
    ///         <term>32770-32771<br />(8002H-8003H)<br /></term>
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
    ///         <term>32772<br />(8004H)<br /></term>
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
    ///     <item>
    ///         <term>32773<br />(8005H)<br /></term>
    ///         <description>
    ///			Auto-Start Specification:<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>0</term>
    ///			        <description>No auto-start</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>1</term>
    ///			        <description>auto-start</description>
    ///			    </item>
    ///			</list>
    ///			</description>
    ///     </item>
    ///     <item>
    ///         <term>32774-32775<br />(8006H-8007H)<br /></term>
    ///         <description>
    ///			Number of BASIC RAM bytes (LSB/MSB) to be reserved for machine code variables (LSB/MSB).<br /> Examples:<br />
    ///			<list type="bullet">
    ///			    <item>
    ///			        <term>0100H</term>
    ///			        <description>1 byte reserved</description>
    ///			    </item>
    ///			    <item>
    ///			        <term>0002H</term>
    ///			        <description>512 bytes reserved</description>
    ///			    </item>
    ///			</list>
    ///			</description>
    ///     </item>
    /// </list>
    /// </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1, Size = 8)]
    public struct DockHeaderArosStruct
    {
        /// <summary>
        /// DOCK memory bank cartridge AROS type, must be one of:
        /// <para>
        /// <list type="bullet">
        ///     <item>
        ///         <term>1 (<c>DockArosLanguageTypeEnum.BASIC</c>)</term>
        ///         <description>
        ///			Timex 2068 BASIC [with optional machine code]
        ///			</description>
        ///     </item>
        ///     <item>
        ///         <term>2 (<c>DockArosLanguageTypeEnum.MACHINE_CODE</c>)</term>
        ///         <description>
        ///			Zilog Z80 Machine Code [only]
        ///			</description>
        ///     </item>
        /// </list>
        /// </para>
        /// <para>See <see cref="CaetanoSoft.Era8bit.FileFormats.DCK.DockHeaderLanguageTypeEnum" />.</para>
        /// </summary>
        public byte LanguageType;

        /// <summary>
        /// For DOCK memory bank cartridge AROS type, must be 2.
        /// <para>See <see cref="CaetanoSoft.Era8bit.FileFormats.DCK.DockHeaderCartridgeTypeEnum" />.</para>
        /// </summary>
        public byte CartridgeType;

        /// <summary>
        /// The Timex 2068 BASIC line or the Zilog Z80 machine code address of the instruction to start/jump to 
        /// (stored in LSB/MSB order, aka little-endian)
        /// </summary>
        public ushort StartingCode;

        /// <summary>
        /// The memory chunk specification. When writing to the Horizontal Select Register (Port F4H), 
        /// the Chunk Specification is High Active
        /// </summary>
        public byte MemoryChunkSpecification;

        /// <summary>
        /// The program auto-start specification: 0 =&gt; No; 1 =&gt; Yes
        /// <para>
        /// <list type="bullet">
        ///     <item>
        ///	        <term>0</term>
        ///			<description>No auto-start</description>
        ///	    </item>
        ///		<item>
        ///			<term>1</term>
        ///			<description>Auto-start</description>
        ///		</item>
        ///	</list>
        /// </para>
        /// </summary>
        public byte AutoStartSpecification;

        /// <summary>
        /// Number of bytes of RAM to be reserved for machine code variables, stored in LSB MSB order.
        /// <para>When setting the AROS Overhead parameter requesting RAM space for machine code variables, 
        /// 21 + n bytes (15H + n) must be requested where n is the number of bytes needed. 
        /// The machine language variables area then starts at 6 85 5H immediately following the 21-byte CHANS area.</para>
        /// <para><strong>NOTE:</strong> This does not apply to an AROS that contains both BASIC and machine code.</para>
        /// </summary>
        public ushort ReservedRAM;
    }
}
