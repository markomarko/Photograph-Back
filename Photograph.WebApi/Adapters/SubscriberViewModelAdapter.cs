using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExpressMapper.Extensions;
using Photograph.BLL.Dtos;
using Photograph.WebApi.Models;

namespace Photograph.WebApi.Adapters
{
	public class SubscriberViewModelAdapter
	{
		public static SubscriberViewModel BuiSubscriberViewModel(SubscriberDto subscriberDto)
		{
			return subscriberDto.Map<SubscriberDto, SubscriberViewModel>();
		}

		public static SubscriberDto BuildSubscriberDto(SubscriberViewModel subscriberViewModel)
		{
			return subscriberViewModel.Map<SubscriberViewModel, SubscriberDto>();
		}
	}
}