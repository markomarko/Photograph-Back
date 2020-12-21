using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photograph.BLL.Services;
using Photograph.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Photograph.BLL.Dtos
{
    public class PhotoDto
    {
        public Guid Id { get; set; }
        public Guid AlbumId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
        public bool Selected { get; set; }

    }
}
