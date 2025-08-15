using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;
using NZWalks.API.Repositories.Interfaces;
using NZWalks.Core.Pagination;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(
            IMapper mapper,
            IWalkRepository walkRepository
        )
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // GET Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track&sortBy=Bane&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllWalksQueryParameters query)
        {
            var result = await walkRepository.GetAllAsync(query);

            var dtoItems = mapper.Map<List<WalkDto>>(result.Items);
            

            var response = new PagedResult<WalkDto>
            {
                Items = dtoItems,
                TotalCount = result.TotalCount,
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalPages = result.TotalPages,
            };

            return Ok(response);
        }


        // Get Walks By Id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk = await walkRepository.GetByIdAsync(id);
            if(walk == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(walk);
            return Ok(walkDto);
        }

        // CREATE WALK
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto request)
        {
            var walkDomainModel = mapper.Map<Walk>(request);
            await walkRepository.CreateAsync(walkDomainModel);

            var walkDto = mapper.Map<WalkDto>(walkDomainModel);
            return Created();
        }

        // Updade Walk By Id
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRequestDto request)
        {
            var walk = mapper.Map<Walk>(request);
            var updatedWalk = await walkRepository.UpdateWalkAsync(id, walk);
            if (updatedWalk == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(updatedWalk);
            return Ok(walkDto);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalk = await walkRepository.DeleteByIdAsync(id);
            if(deletedWalk == null)
            {
                return NotFound();
            }
            var walkDto = mapper.Map<WalkDto>(deletedWalk);
            return Ok(walkDto);
        }

    }
}
