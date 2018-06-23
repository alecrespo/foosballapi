using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Ecovadis.API.Services;
using Ecovadis.DAL.Models;
using Ecovadis.DL.Infrastructures;
using Ecovadis.DL.Models;
using Ecovadis.DL.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Ecovadis.API.Controllers
{
    /// <summary>
    /// Tasks operations
    /// </summary>
    [Route("api/[controller]")]
    public class MatchController : Controller
    {
        private readonly IErrorHandler errorHandler;
        private readonly IGameService game;
        public MatchController(IErrorHandler errorHandler, IGameService game)
        {
            this.errorHandler = errorHandler;
            this.game = game;
        }
        /// <summary>
        /// Get the match based on its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MatchDtoDetail), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Get([Required]int id)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResult(errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation)) { StatusCode = (Int32)HttpStatusCode.BadRequest };
            }
            var match = game.Detail(id);
            return new ObjectResult(match) { StatusCode = (Int32)HttpStatusCode.OK };
        }
        /// <summary>
        /// Get Matches Descending
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ICollection<MatchDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get()
        {
            IEnumerable<MatchDto> matches = game.List();
            return new ObjectResult(matches) { StatusCode = (Int32)HttpStatusCode.OK };
        }
        /// <summary>
        /// Creates a new Match
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(MatchDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Create()
        {
            var match = game.Create();
            return new ObjectResult(match) { StatusCode = (Int32)HttpStatusCode.OK };
        }
        /// <summary>
        /// Updates the match given its Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MatchDtoDetail), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Update([Required]int id, TableSide side)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResult(errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation)) { StatusCode = (Int32)HttpStatusCode.BadRequest };
            }
            var match = game.Score(id, side);
            return new ObjectResult(match) { StatusCode = (Int32)HttpStatusCode.OK };
        }
        /// <summary>
        /// Removes the match given its Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="side"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> Delete([Required]int id)
        {
            if (!ModelState.IsValid)
            {
                return new ObjectResult(errorHandler.GetMessage(ErrorMessagesEnum.ModelValidation)) { StatusCode = (Int32)HttpStatusCode.BadRequest };
            }
            game.Remove(id);
            return new ObjectResult(null) { StatusCode = (Int32)HttpStatusCode.OK };
        }
    }
}
