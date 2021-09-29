using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WahidaHossainComp306Lab01
{
    class BucketElements
    {
        public string ObjectId { get; set; }
        public string ObjectSize { get; set; }
        public string LastModified { get; set; }
        public string StorageClass { get; set; }

       

        public BucketElements(string objectId, string objectSize, string lastModified, string storageClass)
        {
            this.ObjectId = objectId;
            this.ObjectSize = objectSize;
            this.LastModified = lastModified;
            this.StorageClass = storageClass;
        }
    }
}
