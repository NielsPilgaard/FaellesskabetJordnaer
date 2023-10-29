using Jordnaer.Server.Authorization;
using Jordnaer.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Jordnaer.Server.Features.Groups;

public static class GroupApi
{
    public static RouteGroupBuilder MapGroups(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("api/groups");

        // TODO: Rate limit

        group.MapGet("{id:guid}", GetGroupByIdAsync);

        group.MapPost("", CreateGroupAsync).RequireCurrentUser();

        group.MapPut("{id:guid}", UpdateGroupAsync).RequireCurrentUser();

        group.MapDelete("{id:guid}", DeleteGroupAsync).RequireCurrentUser();

        return group;
    }

    private static async Task<Results<Ok<GroupDto>, NotFound>>
        GetGroupByIdAsync(
        [FromRoute] Guid id,
        [FromServices] GroupService groupsService)
        => await groupsService.GetGroupByIdAsync(id);

    private static async Task<CreatedAtRoute> CreateGroupAsync(
        [FromBody] Group group,
        [FromServices] GroupService groupsService)
        => await groupsService.CreateGroupAsync(group);

    private static async Task<Results<NoContent, UnauthorizedHttpResult, NotFound, BadRequest>>
        UpdateGroupAsync(
            [FromRoute] Guid id,
            [FromBody] Group group,
            [FromServices] GroupService groupsService)
        => await groupsService.UpdateGroupAsync(id, group);

    private static async Task<Results<NoContent, UnauthorizedHttpResult, NotFound>>
        DeleteGroupAsync(
            [FromRoute] Guid id,
            [FromServices] GroupService groupsService)
        => await groupsService.DeleteGroupAsync(id);
}
