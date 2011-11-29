/*
 * Copyright (c) Contributors, http://aurora-sim.org/, http://opensimulator.org/
 * See CONTRIBUTORS.TXT for a full list of copyright holders.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the Aurora-Sim Project nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE DEVELOPERS ``AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE CONTRIBUTORS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using Nwc.XmlRpc;
using System.Security.Authentication;

namespace OpenSim.Framework.Servers.HttpServer
{
    /// <summary>
    /// Interface to OpenSimulator's built in HTTP server.  Use this to register handlers (http, llsd, xmlrpc, etc.)
    /// for given URLs.
    /// </summary>
    public interface IHttpServer
    {
        uint Port { get; }

        /// <summary>
        /// Whether this server is running with HTTPS
        /// </summary>
        bool Secure { get; }

        /// <summary>
        /// A well-formed URI for the host region server (namely "http://ExternalHostName:Port)
        /// </summary>
        string ServerURI { get; }

        /// <summary>
        /// The hostname (external IP or dns name) that this server is on (without http(s)://)
        /// </summary>
        string HostName { get; set; }

        /// <summary>
        /// The hostname (external IP or dns name) that this server is on (with http(s)://)
        /// </summary>
        string FullHostName { get; }

        /// <summary>
        /// Set the settings needed to run with HTTPS enabled
        /// </summary>
        /// <param name="path"></param>
        /// <param name="password"></param>
        /// <param name="protocol"></param>
        void SetSecureParams (string path, string password, SslProtocols protocol);
        
        /// <summary>
        /// Add a handler for an HTTP request.
        /// </summary>
        /// 
        /// This handler can actually be invoked either as 
        /// 
        /// http://<hostname>:<port>/?method=<methodName> 
        /// 
        /// or
        /// 
        /// http://<hostname>:<port><method>
        /// 
        /// if the method name starts with a slash.  For example, AddHTTPHandler("/object/", ...) on a standalone region
        /// server will register a handler that can be invoked with either
        /// 
        /// http://localhost:9000/?method=/object/
        /// 
        /// or
        /// 
        /// http://localhost:9000/object/
        ///
        /// In addition, the handler invoked by the HTTP server for any request is the one when best matches the request
        /// URI.  So if a handler for "/myapp/" is registered and a request for "/myapp/page" is received, then
        /// the "/myapp/" handler is invoked if no "/myapp/page" handler exists.
        /// 
        /// <param name="methodName"></param>
        /// <param name="handler"></param>
        /// <returns>
        /// true if the handler was successfully registered, false if a handler with the same name already existed.
        /// </returns>
        bool AddHTTPHandler(string methodName, GenericHTTPMethod handler);
         
        bool AddPollServiceHTTPHandler(string methodName, GenericHTTPMethod handler, PollServiceEventArgs args);

        /// <summary>
        /// Adds a LLSD handler, yay.
        /// </summary>
        /// <param name="path">/resource/ path</param>
        /// <param name="handler">handle the LLSD response</param>
        /// <returns></returns>
        bool AddLLSDHandler(string path, LLSDMethod handler);
        
        /// <summary>
        /// Add a stream handler to the http server.  If the handler already exists, then nothing happens.
        /// </summary>
        /// <param name="handler"></param>
        void AddStreamHandler(IRequestHandler handler);

        bool AddXmlRPCHandler(string method, XmlRpcMethod handler);
        bool AddXmlRPCHandler(string method, XmlRpcMethod handler, bool keepAlive);

        /// <summary>
        /// Gets the XML RPC handler for given method name
        /// </summary>
        /// <param name="method">Name of the method</param>
        /// <returns>Returns null if not found</returns>
        XmlRpcMethod GetXmlRPCHandler(string method);
        
        /// <summary>
        /// Remove an HTTP handler
        /// </summary>
        /// <param name="httpMethod"></param>
        /// <param name="path"></param>
        void RemoveHTTPHandler(string httpMethod, string path);

        void RemovePollServiceHTTPHandler(string httpMethod, string path);
        
        bool RemoveLLSDHandler(string path, LLSDMethod handler);
        
        void RemoveStreamHandler(string httpMethod, string path);

        void RemoveXmlRPCHandler(string method);
        
        string GetHTTP404(string host);

        string GetHTTP500();
    }
}
