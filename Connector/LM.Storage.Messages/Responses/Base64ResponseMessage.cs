using LM.Messages;
using System;
using System.Runtime.Serialization;

namespace LM.Storage.Messages.Responses
{
    [DataContract]
    public class Base64ResponseMessage : ResponseMessage
    {
        public Base64ResponseMessage() { }
        public Base64ResponseMessage(Guid responseCode)
            : base(responseCode) { }

        [DataMember]
        public string BucketName { get; set; }

        [DataMember]
        public string FolderName { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FileExtension { get; set; }

        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public string Base64 { get; set; }
    }
}