using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Models.Elasticsearch
{
 interface IFileDataSource
	{
		 FileStream Open(string path,FileMode mode);
	}
}
