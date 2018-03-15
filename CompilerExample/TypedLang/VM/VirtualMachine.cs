using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompilerExample.TypedLang.VM
{
    public class VirtualMachine
    {
        private byte[] _programMem;
        private byte[] _dataMem;

        private int _sp;
        private int _pc;

        public VirtualMachine(byte[] program, int dataMemory = 1024)
        {
            _programMem = program;
            _dataMem = new byte[dataMemory];
            _sp = dataMemory;
            _pc = 0;
        }

        private void PushByte(byte b)
        {
            _dataMem[--_sp] = b;
        }

        private byte PopByte()
        {
            return _dataMem[_sp++];
        }

        private void PushInt(int val)
        {
            for (int i = 0; i < 4; i++)
                PushByte((byte)((val >> (24 - i * 8)) & 0xff));
        }

        private int PopInt()
        {
            return PopByte() | (PopByte() << 8) | (PopByte() << 16) | (PopByte() << 24);
        }

        private void PushFloat(double val)
        {
            long iVal = BitConverter.DoubleToInt64Bits(val);
            for (int i = 0; i < 8; i++)
                PushByte((byte)((iVal >> (56 - i * 8)) & 0xff));
        }

        private double PopFloat()
        {
            long i = PopByte() | (PopByte() << 8) | (PopByte() << 16) | (PopByte() << 24) |
                (PopByte() << 32) | (PopByte() << 40) | (PopByte() << 48) | (PopByte() << 56);
            return BitConverter.Int64BitsToDouble(i);
        }

        private void PushString(string val)
        {
            PushByte(0);
            for (int i = val.Length - 1; i >= 0; i--)
                PushByte((byte)val[i]);
        }

        private string PopString()
        {
            //var bytes = new List<byte>();
            //for (int i = 0; i < 256; i++)
            //{
            //    byte b = PopByte();
            //    if (b == 0)
            //        break;
            //    bytes.Add(b);
            //}
            //return Encoding.ASCII.GetString(bytes.ToArray());

            var sb = new StringBuilder();
            for (int i = 0; i < 256; i++)
            {
                byte b = PopByte();
                if (b == 0)
                    break;
                sb.Append(b);
            }
            return sb.ToString();
        }

        private int GetConstInt()
        {
            return _programMem[_pc++] | (_programMem[_pc++] << 8) | (_programMem[_pc++] << 16) | (_programMem[_pc++] << 24);
        }

        private double GetConstFloat()
        {
            long val = _programMem[_pc++] | (_programMem[_pc++] << 8) | (_programMem[_pc++] << 16) | (_programMem[_pc++] << 24) |
                (_programMem[_pc++] << 32) | (_programMem[_pc++] << 40) | (_programMem[_pc++] << 48) | (_programMem[_pc++] << 56);
            return BitConverter.Int64BitsToDouble(val);
        }

        private string GetConstString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 256; i++)
            {
                byte b = _programMem[_pc++];
                if (b == 0)
                    break;
            }
            return sb.ToString();
        }

        private int LoadInt(int adr)
        {
            return _dataMem[adr] | (_dataMem[adr + 1] << 8) | (_dataMem[adr + 2] << 16) | (_dataMem[adr + 3] << 24);
        }

        private double LoadFloat(int adr)
        {
            long val = _dataMem[adr] | (_dataMem[adr + 1] << 8) | (_dataMem[adr + 2] << 16) | (_dataMem[adr + 3] << 24)
                      | (_dataMem[adr + 4] << 32) | (_dataMem[adr + 5] << 40) | (_dataMem[adr + 6] << 48) | (_dataMem[adr + 7] << 56);
            return BitConverter.Int64BitsToDouble(val);
        }

        private string LoadString(int adr)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 256; i++)
            {
                char c = (char)_dataMem[adr + i];
                if (c == 0)
                    break;
                sb.Append(c);
            }
            return sb.ToString();
        }

        private void StoreInt(int adr, int val)
        {
            for (int i = 0; i < 4; i++)
            {
                _dataMem[adr + i] = (byte)(val & 0xff);
                val >>= 8;
            }
        }

        private void StoreFloat(int adr, double val)
        {
            long iVal = BitConverter.DoubleToInt64Bits(val);
            for (int i = 0; i < 8; i++)
            {
                _dataMem[adr + i] = (byte)(iVal & 0xff);
                iVal >>= 8;
            }
        }

        private void StoreString(int adr, string val)
        {
            for (int i = 0; i < val.Length; i++)
                _dataMem[adr + i] = (byte)val[i];
            _dataMem[adr + val.Length] = 0;
        }

        public void ExecuteStep()
        {
            var op = (OpCodes)_programMem[_pc++];
            int adr;
            switch (op)
            {
                // Addition Opcodes
                case OpCodes.Add:
                    PushInt(PopInt() + PopInt());
                    break;
                case OpCodes.fAdd:
                    PushFloat(PopFloat() + PopFloat());
                    break;
                case OpCodes.Cat:
                    PushString(PopString() + PopString());
                    break;

                // Subtraction Opcodes
                case OpCodes.Sub:
                    PushInt(PopInt() - PopInt());
                    break;
                case OpCodes.fSub:
                    PushFloat(PopFloat() - PopFloat());
                    break;

                // Multiplication Opcodes
                case OpCodes.Mul:
                    PushInt(PopInt() * PopInt());
                    break;
                case OpCodes.fMul:
                    PushFloat(PopFloat() * PopFloat());
                    break;

                // Div Opcodes
                case OpCodes.Div:
                    PushInt(PopInt() / PopInt());
                    break;
                case OpCodes.fDiv:
                    PushFloat(PopFloat() / PopFloat());
                    break;

                // Modulo Opcodes
                case OpCodes.Mod:
                    PushInt(PopInt() % PopInt());
                    break;
                case OpCodes.fMod:
                    PushFloat(PopFloat() % PopFloat());
                    break;

                // Neg Opcodes
                case OpCodes.Neg:
                    PushInt(-PopInt());
                    break;
                case OpCodes.fNeg:
                    PushFloat(-PopFloat());
                    break;

                // Load Opcodes
                case OpCodes.Load:
                    PushInt(LoadInt(GetConstInt()));
                    break;
                case OpCodes.fLoad:
                    PushFloat(LoadFloat(GetConstInt()));
                    break;
                case OpCodes.sLoad:
                    PushString(LoadString(GetConstInt()));
                    break;

                // Casting Opcodes
                case OpCodes.FloatToInt:
                    PushInt((int)PopFloat());
                    break;
                case OpCodes.StrToInt:
                    PushInt(int.Parse(PopString()));
                    break;
                case OpCodes.IntToFloat:
                    PushFloat(PopInt());
                    break;
                case OpCodes.StrToFloat:
                    PushFloat(double.Parse(PopString()));
                    break;
                case OpCodes.IntToStr:
                    PushString(PopInt().ToString());
                    break;
                case OpCodes.FloatToStr:
                    PushString(PopFloat().ToString());
                    break;
                    
                // Store Opcodes
                case OpCodes.Store:
                    StoreInt(GetConstInt(), PopInt());
                    break;
                case OpCodes.fStore:
                    StoreFloat(GetConstInt(), PopFloat());
                    break;
                case OpCodes.sStore:
                    StoreString(GetConstInt(), PopString());
                    break;

                // Push Opcodes <- Very possible these are wrong
                case OpCodes.Push:
                    PushInt(GetConstInt());
                    break;
                case OpCodes.fPush:
                    PushFloat(GetConstFloat());
                    break;
                case OpCodes.sPush:
                    PushString(GetConstString());
                    break;

                // Pop Opcodes
                case OpCodes.Pop:
                    PopInt();
                    break;
                case OpCodes.fPop:
                    PopFloat();
                    break;
                case OpCodes.sPop:
                    PopString();
                    break;

                // Print Opcode
                case OpCodes.Print:
                    Console.WriteLine(PopString());
                    break;
            }
        }

        public void Run()
        {
            _pc = 0;
            _sp = _dataMem.Length;
            while (_pc < _programMem.Length)
                ExecuteStep();
        }
    }
}
