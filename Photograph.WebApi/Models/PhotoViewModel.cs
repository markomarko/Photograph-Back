using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Photograph.WebApi.Models
{
    public class PhotoViewModel
    {
        public Guid Id { get; set; }
        public Guid AlbumId { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Context { get; set; }
        public bool Selected { get; set; }
    }
}