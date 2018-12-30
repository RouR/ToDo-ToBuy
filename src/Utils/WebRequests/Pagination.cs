using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;

namespace Utils.WebRequests
{
    public class Pagination<T>: IPaginationSetting
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 25;
        public int TotalItems { get; set;}

        public IEnumerable<T> Items { get; set; } = new List<T>(0);

        public int TotalPages => (int) Math.Ceiling(((double) TotalItems) / PageSize);

        public int FirstItem => ((Page - 1) * PageSize) + 1;

        public int LastItem => FirstItem + Items.Count() - 1;

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;

        public Pagination()
        {
            //for mapping and deserialization
        }
        
        public Pagination(Pagination<T> from): this(from.Items, from.TotalItems, from)
        {
            
        }
        public Pagination(IEnumerable<T> items, int totalItems, IPaginationSetting settings)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            var page = settings.Page;
            var pageSize = settings.PageSize;

            if (page < 1)
                throw new ArgumentOutOfRangeException(nameof(page), page, "Value must be greater than zero.");

            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "Value must be greater than zero.");

            if (totalItems < 0)
                throw new ArgumentOutOfRangeException(nameof(totalItems), totalItems, "Value cannot be less than zero.");

            this.Items = items.ToList();
            this.Page = page;
            this.PageSize = pageSize;
            this.TotalItems = totalItems;
        }

        public override string ToString()
        {
            return string.Format("{0} Item(s)", TotalItems);
        }
    }
}