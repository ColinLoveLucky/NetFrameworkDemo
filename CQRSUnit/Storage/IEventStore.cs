using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CQRSUnit.EventSourcing
{
	public interface IEventStore
	{
		void StoreEvetns(IEnumerable<SourceEvent> events);
		IEnumerable<SourceEvent> GetEvents(string aggregateRootId, Type aggregateRootType, long minVersion, long maxVersion);
	}
}