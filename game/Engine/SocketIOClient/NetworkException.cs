using System;

namespace Blok3Game.Engine.SocketIOClient
{
    public class NetworkException : Exception
    {
        public NetworkException(string message) : base(message)
        {
        }
    }
}