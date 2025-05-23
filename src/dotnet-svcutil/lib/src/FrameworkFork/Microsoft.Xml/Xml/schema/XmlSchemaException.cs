// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Xml.Schema
{
    using System;
    using System.IO;
    using System.Text;
    using System.Resources;
    using System.Globalization;
    using System.Diagnostics;
    // using System.Security.Permissions;

    /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException"]/*' />
    // [Serializable],
    public class XmlSchemaException : System.Exception
    {
        private string _res;
        private string[] _args;
        private string _sourceUri;
        private int _lineNumber;
        private int _linePosition;

        [NonSerialized]
        private XmlSchemaObject _sourceSchemaObject;

        // message != null for V1 exceptions deserialized in Whidbey
        // message == null for V2 or higher exceptions; the exception message is stored on the base class (Exception._message)
        private string _message = null;

        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.XmlSchemaException1"]/*' />
        public XmlSchemaException() : this(null)
        {
        }

        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.XmlSchemaException2"]/*' />
        public XmlSchemaException(String message) : this(message, ((Exception)null), 0, 0)
        {
#if DEBUG
            Debug.Assert(message == null || !message.StartsWith("Sch_", StringComparison.Ordinal), "Do not pass a resource here!");
#endif
        }

        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.XmlSchemaException0"]/*' />
        public XmlSchemaException(String message, Exception innerException) : this(message, innerException, 0, 0)
        {
        }

        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.XmlSchemaException3"]/*' />
        public XmlSchemaException(String message, Exception innerException, int lineNumber, int linePosition) :
            this((message == null ? ResXml.Sch_DefaultException : ResXml.Xml_UserException), new string[] { message }, innerException, null, lineNumber, linePosition, null)
        {
        }

        internal XmlSchemaException(string res, string[] args) :
            this(res, args, null, null, 0, 0, null)
        { }

        internal XmlSchemaException(string res, string arg) :
            this(res, new string[] { arg }, null, null, 0, 0, null)
        { }

        internal XmlSchemaException(string res, string arg, string sourceUri, int lineNumber, int linePosition) :
            this(res, new string[] { arg }, null, sourceUri, lineNumber, linePosition, null)
        { }

        internal XmlSchemaException(string res, string sourceUri, int lineNumber, int linePosition) :
            this(res, (string[])null, null, sourceUri, lineNumber, linePosition, null)
        { }

        internal XmlSchemaException(string res, string[] args, string sourceUri, int lineNumber, int linePosition) :
            this(res, args, null, sourceUri, lineNumber, linePosition, null)
        { }

        internal XmlSchemaException(string res, XmlSchemaObject source) :
            this(res, (string[])null, source)
        { }

        internal XmlSchemaException(string res, string arg, XmlSchemaObject source) :
            this(res, new string[] { arg }, source)
        { }

        internal XmlSchemaException(string res, string[] args, XmlSchemaObject source) :
            this(res, args, null, source.SourceUri, source.LineNumber, source.LinePosition, source)
        { }

        internal XmlSchemaException(string res, string[] args, Exception innerException, string sourceUri, int lineNumber, int linePosition, XmlSchemaObject source) :
            base(CreateMessage(res, args), innerException)
        {
            HResult = HResults.XmlSchema;
            _res = res;
            _args = args;
            _sourceUri = sourceUri;
            _lineNumber = lineNumber;
            _linePosition = linePosition;
            _sourceSchemaObject = source;
        }

        internal static string CreateMessage(string res, string[] args)
        {
            if (args == null)
            {
                return res;
            }

            try
            {
                return string.Format(res, args);
            }
            catch (MissingManifestResourceException)
            {
                return "UNKNOWN(" + res + ")";
            }
        }

        internal string GetRes
        {
            get
            {
                return _res;
            }
        }

        internal string[] Args
        {
            get
            {
                return _args;
            }
        }
        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.SourceUri"]/*' />
        public string SourceUri
        {
            get { return _sourceUri; }
        }

        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.LineNumber"]/*' />
        public int LineNumber
        {
            get { return _lineNumber; }
        }

        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.LinePosition"]/*' />
        public int LinePosition
        {
            get { return _linePosition; }
        }

        /// <include file='doc\XmlSchemaException.uex' path='docs/doc[@for="XmlSchemaException.SourceObject"]/*' />
        public XmlSchemaObject SourceSchemaObject
        {
            get { return _sourceSchemaObject; }
        }

        /*internal static XmlSchemaException Create(string res) { //Since internal overload with res string will clash with public constructor that takes in a message
            return new XmlSchemaException(res, (string[])null, null, null, 0, 0, null);
        }*/

        internal void SetSource(string sourceUri, int lineNumber, int linePosition)
        {
            _sourceUri = sourceUri;
            _lineNumber = lineNumber;
            _linePosition = linePosition;
        }

        internal void SetSchemaObject(XmlSchemaObject source)
        {
            _sourceSchemaObject = source;
        }

        internal void SetSource(XmlSchemaObject source)
        {
            _sourceSchemaObject = source;
            _sourceUri = source.SourceUri;
            _lineNumber = source.LineNumber;
            _linePosition = source.LinePosition;
        }

        internal void SetResourceId(string resourceId)
        {
            _res = resourceId;
        }

        public override string Message
        {
            get
            {
                return (_message == null) ? base.Message : _message;
            }
        }
    };
} // namespace Microsoft.Xml.Schema

