using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SysBiblioteca.API.Management;

namespace SysBiblioteca.API.Tests.Helpers;

public static class ReplyResultExtensions
{
    public static Reply ShouldBeOkReply(this IActionResult result)
    {
        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        return ok.Value.Should().BeOfType<Reply>().Subject;
    }

    public static Reply ShouldBeBadRequestReply(this IActionResult result)
    {
        var badRequest = result.Should().BeOfType<BadRequestObjectResult>().Subject;
        return badRequest.Value.Should().BeOfType<Reply>().Subject;
    }

    public static Reply ShouldBeConflictReply(this IActionResult result)
    {
        var conflict = result.Should().BeOfType<ConflictObjectResult>().Subject;
        return conflict.Value.Should().BeOfType<Reply>().Subject;
    }

    public static Reply ShouldBeNotFoundReply(this IActionResult result)
    {
        var notFound = result.Should().BeOfType<NotFoundObjectResult>().Subject;
        return notFound.Value.Should().BeOfType<Reply>().Subject;
    }
}
