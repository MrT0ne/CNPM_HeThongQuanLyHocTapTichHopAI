using Elearning.Common.Infrastructure.Auth;
using Elearning.Common.Infrastructure.Extensions;
using Elearning.Common.Presentation.Endpoints;
using Elearning.Common.Presentation.Results;
using Elearning.Modules.Program.Application.Program.GetRecentCourses;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Elearning.Modules.Program.Presentation.Program;

[RequireAuthAttribute("Student")]
internal sealed class GetRecentCourses : IEndpoint
{
  public void MapEndpoint(IEndpointRouteBuilder app)
  {
    app.MapGet("program/student/dashboard/recent-courses", async (
        [FromQuery] Guid? studentId,
        ISender sender,
        HttpContext httpContext) =>
    {
      // If studentId is not provided, try to get it from the current user
      if (studentId == null)
      {
        var userId = Guid.Parse(httpContext.User.GetUserId());
        studentId = userId;
      }

      var query = new GetRecentCoursesQuery { StudentId = studentId };
      var result = await sender.Send(query);

      return Results.Ok(new
      {
        status = StatusCodes.Status200OK,
        message = "Get recent courses successfully",
        data = result
      });
    })
    .AllowAnonymous() // Tạm thời cho phép truy cập không cần xác thực
    .WithTags("Student");
  }
}
