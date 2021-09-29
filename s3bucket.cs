using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WahidaHossainComp306Lab01
{
    class s3bucket
    {
        public string BucketName { get; set; }
        public string CreationDate { get; set; }

        public s3bucket()
        {

        }

        public s3bucket(string bucketName, string creationDate)
        {
            this.BucketName = bucketName;
            this.CreationDate = creationDate;

        }
    }
}
