using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerExample.TypedLang.VM
{
    enum OpCodes : byte
    {
        Nop = 0,
        Pop,
        fPop,
        sPop,
        Push,
        fPush,
        sPush,
        Add,
        fAdd,
        Sub,
        fSub,
        Mul,
        fMul,
        Div,
        fDiv,
        Mod,
        fMod,
        Cat,
        Load,
        fLoad,
        sLoad,
        Store,
        fStore,
        sStore,
        Neg,
        fNeg,
        Print,
        StrToInt,
        StrToFloat,
        FloatToInt,
        FloatToStr,
        IntToFloat,
        IntToStr
    }
}
