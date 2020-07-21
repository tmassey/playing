using System;
using System.Net;
using Phoenix.Api.Core.Logging.Models;

namespace Phoenix.Api.Core.Logging.Interfaces
{
    public interface IPhoenixLogger: IDisposable
    {   
        void Debug(string message, UserDetails userDetails = null);
        void Network(string method, HttpStatusCode responseCode, string path, int duration, string serviceId = null, UserDetails userDetails = null);
        void Info(string message, UserDetails userDetails = null);
        void Warning(string message, UserDetails userDetails = null);
        void Error(string message, UserDetails userDetails = null);
        void Fatal(Exception ex, UserDetails userDetails = null);        
    }
}