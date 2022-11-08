﻿using Locus.Data;
using Locus.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Locus.Controllers
{
    [Route("/")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly EntitiesDbContext _context;
        public TenantController(EntitiesDbContext context) => _context = context;

        [HttpGet("Tenants")]
        public async Task<IEnumerable<Tenant>> GetTenants() => await _context.Tenants.ToListAsync();

        [HttpGet("Tenants/id")]
        [ProducesResponseType(typeof(Tenant), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTenantById(int id)
        {
            var tenant = await _context.Tenants.FindAsync(id);
            return tenant == null ? NotFound() : Ok(tenant);
        }
        /// <summary>
        /// Creates a new tenant
        /// </summary>
        /// <param name="tenant">Object of type tenant</param>
        /// <returns>The tenant created</returns>
        [HttpPost("Tenants")]
        [ProducesResponseType(typeof(Tenant), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(Tenant tenant )
        {
            await _context.Tenants.AddAsync(tenant);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTenantById), new { id = tenant.Id }, tenant);
        }
    }
}