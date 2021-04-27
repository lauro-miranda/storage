using LM.Messages;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LM.Storage.Messages.Requests
{
    [DataContract]
    public class UploadFileRequestMessage : RequestMessage
    {
        public UploadFileRequestMessage() { }
        public UploadFileRequestMessage(Guid requestCode)
            : base(requestCode) { }

        [DataMember]
        public HashSet<FileRequestMessage> Files { get; set; } = new HashSet<FileRequestMessage>();

        [DataContract]
        public class FileRequestMessage
        {
            [DataMember]
            public string Name { get; set; }

            [DataMember]
            public string Base64 { get; set; }

            [DataMember]
            public string FolderName { get; set; }
        }
    }
}