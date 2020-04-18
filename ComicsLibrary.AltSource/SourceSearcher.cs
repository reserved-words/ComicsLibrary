﻿using ComicsLibrary.Common.Api;
using ComicsLibrary.Common.Interfaces;
using ComicsLibrary.Common.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicsLibrary.AltSource
{
    public class SourceSearcher : ISourceSearcher
    {
        private readonly Comix.Services.SearchService _searchService;
        private readonly Comix.Services.UpdateService _updateService;

        public SourceSearcher(IConfiguration config)
        {
            _searchService = new Comix.Services.SearchService(config);
            _updateService = new Comix.Services.UpdateService(config);
        }

        public Task<PagedResult<Book>> GetBooks(int sourceItemID, int limit, int offset)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<SearchResult>> SearchByTitle(string title, int sortOrder, int limit, int page)
        {
            var results = await _searchService.Search(title);
            var list = new List<SearchResult>();

            foreach (var item in results)
            {
                list.Add(new SearchResult
                {
                    SourceId = 2,
                    SourceItemId = item.ID,
                    Title = item.Title,
                    Url = item.Url,
                    ImageUrl = item.ImageUrl
                });
            }

            return new PagedResult<SearchResult>(list, list.Count(), 1, list.Count());
        }
    }
}