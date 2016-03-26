using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Utilities;

namespace BLEntities.SQLServer
{
    [Serializable]
    public abstract class SQLImportEntityBase
    {
        public override string ToString()
        {
            return ReflectionHelper.ToString(this);
        }
        public SQLImportEntityBase DeepCopy()
        {
            var formatter = new BinaryFormatter();
            var stream = new MemoryStream();
            formatter.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            var result = (SQLImportEntityBase)formatter.Deserialize(stream);
            stream.Close();
            return result;
        }
    }
}
