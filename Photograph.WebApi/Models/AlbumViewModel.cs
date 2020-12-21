using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photograph.WebApi.Models
{
    public class AlbumViewModel
    {
        public Guid Id { get; set; }

        public Guid OwnerId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public  List<Guid> UsersWithAccess { get; set; }
    }
}