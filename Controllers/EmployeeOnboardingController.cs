using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Services;

namespace PaylocityAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class EmployeeOnboardingController : Controller
	{
		private readonly IAnnualCostService _svc;

		public EmployeeOnboardingController(IAnnualCostService svc)
		{
			_svc = svc ?? throw new ArgumentNullException(nameof(svc));
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
		[Route("AnnualCostPreview")]
		public async Task<IActionResult> GetEmployeeAnnualCost(string employeeList)
		{
			var res = await _svc.CostPreview(employeeList);

			return Ok(res);
		}
	}
}
