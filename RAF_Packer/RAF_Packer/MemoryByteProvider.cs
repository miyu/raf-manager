using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Be.Windows.Forms;

namespace RAF_Packer
{
    public class MemoryByteProvider:IByteProvider
    {
        byte[] data = null;
        public MemoryByteProvider(byte[] data)
        {
            this.data = data;
        }
        byte IByteProvider.ReadByte(long index)
        {
            return data[index];
        }

        void IByteProvider.WriteByte(long index, byte value)
        {
        }

        void IByteProvider.InsertBytes(long index, byte[] bs)
        {
        }

        void IByteProvider.DeleteBytes(long index, long length)
        {
        }

        long IByteProvider.Length
        {
            get { return this.data.Length; }
        }

        event EventHandler IByteProvider.LengthChanged
        {
            add { }
            remove { }
        }

        bool IByteProvider.HasChanges()
        {
            return false;
        }

        void IByteProvider.ApplyChanges()
        {
        }

        event EventHandler IByteProvider.Changed
        {
            add { }
            remove { }
        }

        bool IByteProvider.SupportsWriteByte()
        {
            return false;
        }

        bool IByteProvider.SupportsInsertBytes()
        {
            return false;
        }

        bool IByteProvider.SupportsDeleteBytes()
        {
            return false;
        }
    }
}
