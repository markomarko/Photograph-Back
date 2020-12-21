using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Photograph.WebApi.Adapters
{
    public class PagingParameterViewModelAdapter
    {
        public static PagingParameterDto BuildPagingDto(PagingParameterModel obj)
        {
            return obj.Map<PagingParameterModel, PagingParameterDto>();
        }

        public static PagingParameterModel BuildPagingModel(PagingParameterDto obj)
        {
            return obj.Map<PagingParameterDto, PagingParameterModel>();
        }
    }
}