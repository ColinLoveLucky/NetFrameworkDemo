using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Microsoft.AspNet.SignalR;

namespace QK.QAPP.SalesCenter.Hubs
{
    public class BaseHub : Hub
    {
        protected readonly ILifetimeScope _hubLifetimeScope;
        public BaseHub(ILifetimeScope lifetimeScope)
        {
            _hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _hubLifetimeScope != null)
                _hubLifetimeScope.Dispose();

            base.Dispose(disposing);
        }
    }
}