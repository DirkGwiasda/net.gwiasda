﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Gwiasda.Links
{
    public class LinkManager : ILinkManager
    {
        private readonly ILinkValidator _validator;
        private readonly ILinkRepository _repository;

        public LinkManager(ILinkValidator validator, ILinkRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public async Task CreateOrUpdateLinkAsync(Link link)
        {
            _validator.Validate(link);
            await _repository.CreateOrUpdateLinkAsync(link);
        }

        public async Task DeleteLinkAsync(Guid id)
            => await _repository.DeleteLinkAsync(id);

        public async Task<IEnumerable<Link>> GetLinksAsync()
        {
            var links = (await _repository.GeLinksAsync()).ToList();
            links.Sort((a, b) => a.Text.CompareTo(b.Text));
            return links;
        }
    }
}