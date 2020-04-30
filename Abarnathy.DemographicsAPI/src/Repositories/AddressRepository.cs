﻿using Abarnathy.DemographicsAPI.Data;
using Abarnathy.DemographicsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Abarnathy.DemographicsAPI.Infrastructure;

namespace Abarnathy.DemographicsAPI.Repositories
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(DemographicsDbContext context) : base(context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<Address> GetByCompleteAddressAsync(AddressDTO dto)
        {
            var result =
                await base.GetByCondition(a =>
                    a.StreetName.Contains(dto.StreetName))
                    .FirstOrDefaultAsync();

            return result;
        }
        
        // &&
        // (NormaliseString(a.HouseNumber) == NormaliseString(dto.HouseNumber)) &&
        // (NormaliseString(a.Town) == NormaliseString(dto.Town)) &&
        // (NormaliseString(a.State) == NormaliseString(dto.State)) &&
        // (NormaliseString(a.Zipcode) == NormaliseString(dto.Zipcode))

        /**
         * Helper methods
         * 
         **/
        private string NormaliseString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            return str.Trim().ToLower();
        }
    }
}