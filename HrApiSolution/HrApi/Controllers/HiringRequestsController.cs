﻿using AutoMapper;
using HrApi.Domain;
using HrApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.Controllers;

public class HiringRequestsController :ControllerBase
{

    private readonly IManageHiringRequests _hiringManager;

    public HiringRequestsController(IManageHiringRequests hiringManager)
    {
        _hiringManager = hiringManager;
    }

    [HttpPut("/hiring-requests/{id:int}/salary")]
    public async Task<ActionResult> AssignSalary(int id, [FromBody] HiringRequestSalaryModel model)
    {
        bool wasUpdated = await _hiringManager.AssignSalaryAsync(id, model.Salary);
        if(wasUpdated)
        {
            return NoContent();
        } else
        {
            return NotFound();
        }

    }

    [HttpPost("/hiring-requests")]
    public async Task<ActionResult<HiringRequestResponseModel>> AddAHiringRequest([FromBody] HiringRequestCreateModel request)
    {
        // Validate it (validation attributes, all that jazz)
        
        HiringRequestResponseModel response = await _hiringManager.CreateHiringRequestAsync(request);
        return CreatedAtRoute("hiring-requests-get-by-id", new { id = response.Id }, response);
    }

    [HttpGet("/hiring-requests/{id:int}", Name ="hiring-requests-get-by-id")]
    public async Task<ActionResult<HiringRequestResponseModel>> GetHiringRequest(int id)
    {
        HiringRequestResponseModel? response = await _hiringManager.GetHiringRequestByIdAsync(id);

        if(response == null)
        {
            return NotFound();
        }  else
        {
            return Ok(response);
        }
    }
}
