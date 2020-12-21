using Photograph.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Photograph.DAL.Repository.Ports
{
	public interface IPhotoRepository
	{
		int Count(Guid id);
	}
}