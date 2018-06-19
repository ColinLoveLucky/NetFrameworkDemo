using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.EventSourcing
{
	public abstract class SourceEvent
	{
		public virtual string UniqueId { get; set; }
		public virtual string AggregateRootId { get; set; }
		public virtual Type AggregateRootType { get; set; }
		public virtual string AggregateRootName { get; set; }
		public virtual long Version { get; set; }
		public virtual string Name { get; set; }
		public virtual object RawEvent { get; set; }
		public virtual string Data { get; set; }
		public virtual DateTime OccurredTime { get; set; }
	}
}