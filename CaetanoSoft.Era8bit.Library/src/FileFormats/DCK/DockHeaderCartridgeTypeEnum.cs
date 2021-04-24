using System;
using System.Collections.Generic;
using System.Text;

namespace CaetanoSoft.Era8bit.FileFormats.DCK
{
    /// <summary>
    /// Enumeration with the type of program in Timex Command Cartridge (TCC) DOCK memory bank: LROS, AROS or ROM.
    /// <para>Both LROS and AROS type of programs have a short header at the beginning of the data which
    /// contains the necessary information (language, type, start address, etc.) for their execution.</para>
    /// </summary>
    public enum DockHeaderCartridgeTypeEnum : byte
    {
        /// <summary>
        /// Timex Command Cartridge (TCC) DOCK memory bank contains a simple ROM chip starting at memory location 0 (0000H).
        /// <para>In order to activate and run the ROM, you must type: <strong>OUT 244,3</strong> to "page" the DOCK port.</para>
        /// <para>The home made "Sinclair ZX Spectrum" cartridge for the Timex/Sinclair 2068 use this format.</para>
        /// </summary>
        ROM = 0,

        /// <summary>
        /// Timex Command Cartridge (TCC) DOCK memory bank type is in the LROS (Language ROM Oriented software) format.
        /// <para>LROS programs are mapped at memory address 0 (0000H) in the DOCK memory bank and they must
        /// be written in machine code.</para>
        /// <para>They can replace the TS2068/TC2068 ROMs.</para>
        /// <para>LROS program cartridges always have auto-start and they will be started after initialization of the
        /// Timex computer is finished.</para>
        /// <para>"Timex Chess" and "Zebra OS_64" use this format.</para>
        /// </summary>
        LROS = 1,

        /// <summary>
        /// Timex Command Cartridge (TCC) DOCK memory bank type is in the AROS (Application ROM Oriented software) format.
        /// <para>AROS programs are mapped at address 32768 (8000H) in the DOCK bank, and may be either in Zilog 
        /// Z80 machine code or in Timex 2068 BASIC (the BASIC interpreter allows it running from the DOCK memory bank).</para>
        /// <para>The TS2068/TC2068 ROMs can't be replaced.</para>
        /// <para>AROS program cartridges may be or may not to be auto-start/auto-run.</para>
        /// <para>"Timex Crazy Bugs" use this format.</para>
        /// </summary>
        AROS = 2
    };
}
