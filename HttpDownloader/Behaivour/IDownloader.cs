using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpDownloader.Behaivour
{
    public interface IDownloader
    {
        public Task<byte[]> Download(string url); 
    }
}
