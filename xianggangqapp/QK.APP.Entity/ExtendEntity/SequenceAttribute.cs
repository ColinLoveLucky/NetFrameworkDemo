using System;

namespace QK.QAPP.Entity
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple=false,Inherited=true)]
    public sealed class SequenceAttribute : Attribute
    {
        /// <summary>
        /// Sequence名称
        /// </summary>
        public string SequenceName { get; set; }

        public SequenceAttribute() { }

        public SequenceAttribute(string sequenceName)
        {
            this.SequenceName = sequenceName;
        }

    }
}
