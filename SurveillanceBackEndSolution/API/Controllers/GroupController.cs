using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Core.DTO;
using Core.DTO.Constants;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [Authorize]
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GroupListDto), description: "Get list of groups")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            var result = await _groupService.GetGroupListsAsync();

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok(result.Content as List<GroupListDto>);
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GroupDto), description: "Get group by Id")]
        [SwaggerResponse((int)ErrorsEnum.GroupNotFound, type: typeof(ErrorDto), description: "Group not found")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [HttpGet]
        [Route("{groupId}")]
        public async Task<IActionResult> Get([FromRoute] long groupId)
        {
            var result = await _groupService.GetGroupAsync(groupId);

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok(result.Content as GroupDto);
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GroupDto), description: "Add new group")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddGroup([FromBody] GroupCreateDto group)
        {
            var result = await _groupService.AddGroup(group);

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok(result.Content as GroupDto);
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GroupDto), description: "Update existing group")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [SwaggerResponse((int)ErrorsEnum.GroupNotFound, type: typeof(ErrorDto), description: "Group not found")]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> UpdateGroup([FromBody] GroupEditDto group)
        {
            var result = await _groupService.EditGroup(group);

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok(result.Content as GroupDto);
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Delete group")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [SwaggerResponse((int)ErrorsEnum.GroupNotFound, type: typeof(ErrorDto), description: "Group not found")]
        [HttpDelete]
        [Route("{groupId}")]
        public async Task<IActionResult> DeleteGroup(long groupId)
        {
            var result = await _groupService.DeleteGroup(groupId);

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok();
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GroupSecurityDeviceDto), description: "Get security device with keys")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [SwaggerResponse((int)ErrorsEnum.GroupSecurityDeviceNotFound, type: typeof(ErrorDto), description: "Security device not found")]
        [HttpGet]
        [Route("{groupId}/device/{deviceId}")]
        public async Task<IActionResult> GetGroupDevice([FromRoute] long groupId, [FromRoute] long deviceId)
        {
            var result = await _groupService.GetSecurityDevice(groupId, deviceId);

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok(result.Content as GroupSecurityDeviceDto);
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, type: typeof(GroupSecurityDeviceDto), description: "Add new security device to group")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [SwaggerResponse((int)ErrorsEnum.GroupNotFound, type: typeof(ErrorDto), description: "Group not found")]
        [HttpPost]
        [Route("{groupId}/device")]
        public async Task<IActionResult> AddGroupDevice([FromRoute] long groupId, [FromBody] GroupSecurityDeviceCreateDto device)
        {
            var result = await _groupService.AddSecurityDevice(groupId, device);

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok(result.Content as GroupSecurityDeviceDto);
        }

        [SwaggerResponse(statusCode: (int)HttpStatusCode.OK, description: "Delete security device from group")]
        [SwaggerResponse((int)ErrorsEnum.UserB2CMissing, type: typeof(ErrorDto), description: "User B2CId is missing")]
        [SwaggerResponse((int)ErrorsEnum.GroupSecurityDeviceNotFound, type: typeof(ErrorDto), description: "Device not found")]
        [HttpDelete]
        [Route("{groupId}/device/{deviceId}")]
        public async Task<IActionResult> DeleteGroupDevice([FromRoute] long groupId, [FromRoute] long deviceId)
        {
            var result = await _groupService.RemoveSecurityDevice(groupId, deviceId);

            if (!result.Succeeded)
                return BadRequest(result.ErrorResult);

            return Ok();
        }
    }
}