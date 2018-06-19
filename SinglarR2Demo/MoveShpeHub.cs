using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SinglarR2Demo
{
    public class MoveShpeHub:Hub
    {
        public void UpdateModel(ShapeModel clientModel)
        {
            clientModel.LastUpdateBy = Context.ConnectionId;

            Clients.AllExcept(clientModel.LastUpdateBy).updateShape(clientModel);
        }
    }

    public class ShapeModel
    {
        [JsonIgnore]
        public double Left { get; set; }

        [JsonIgnore]
        public double Top { get; set; }

        [JsonIgnore]
        public string LastUpdateBy { get; set; }
    }
}