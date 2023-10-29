using Microsoft.AspNetCore.Mvc;
using SharedKernel.Runtime.Exceptions;

namespace SharedKernel.Application;

public class PagingRequest
{
    private int _page = 0;
    private int _size = 20;

    [BindProperty(Name = "page")]
    public int Page
    {
        get
        {
            return _page;
        }

        set
        {
            if (value < 0)
            {
                // throw new BadRequestException("Page must be greater than or equal 0");
            }
            _page = value;
        }
    }

    [BindProperty(Name = "limit")]
    public int Size
    {
        get
        {
            return _size;
        }

        set
        {
            if (value <= 0 || value > 1000)
            {
                throw new BadRequestException("Size should be between 1 and 1000");
            }
            _size = value;
        }
    }

    [BindProperty(Name = "indexFrom")] 
    public int IndexForm { get; set; } = 1;
    
    
    [BindProperty(Name = "offset")]
    public int Offset => _page * _size;
    

    [BindProperty(Name = "filter")]
    public Filter Filter { get; set; }
    

    [BindProperty(Name = "sorts")]
    public List<SortModel> Sorts { get; set; } = new List<SortModel>();
    
    
    public PagingRequest(int page, int size)
    {
        Page = page;
        Size = size;
    }
}