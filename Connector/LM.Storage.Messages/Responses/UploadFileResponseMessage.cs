using LM.Messages;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LM.Storage.Messages.Responses
{
    [DataContract]
    public class UploadFileResponseMessage : ResponseMessage
    {
        public UploadFileResponseMessage() { }
        public UploadFileResponseMessage(Guid responseCode)
            : base(responseCode) { }

        [DataMember]
        public HashSet<FileResponseMessage> Files { get; set; } = new HashSet<FileResponseMessage>();

        [DataContract]
        public class FileResponseMessage
        {
            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public string UploadFileURL { get; set; }
        }
    }
}