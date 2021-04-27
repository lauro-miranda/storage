using LM.Messages;
using System;
using System.Runtime.Serialization;

namespace LM.Storage.Messages.Requests
{
    [DataContract]
    public class Base64RequestMessage : RequestMessage
    {
        public Base64RequestMessage() { }
        public Base64RequestMessage(Guid requestCode)
            : base(requestCode) { }

        [DataMember]
        public string BucketName { get; set; }

        [DataMember]
        public string FolderName { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileExtension { get; set; }
    }
}