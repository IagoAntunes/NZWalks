using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.Dto
{
    public class GetAllWalksQueryParameters
    {
        public string? FilterOn { get; set; }
        public string? FilterQuery { get; set; }
        public string? SortBy { get; set; }
        public bool IsAscending { get; set; } = true;

        [Range(1, int.MaxValue, ErrorMessage = "PageNumber must be greater than 0")]
        public int PageNumber { get; set; } = 1;

        [Range(1, 500, ErrorMessage = "PageSize must be between 1 and 500")]
        public int PageSize { get; set; } = 100;
    }
}
